using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniGLTF;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UIElements;
using VRM;

namespace Yuuta.VRMGo
{
    public class Character : MonoBehaviour
    {
        private static readonly Vector3 ROTATION_VECTOR = new Vector3(0, 1, 0);
        private const string FORWARD_ANIMATOR_KEY = "Forward";
        private const string TURN_ANIMATOR_KEY = "Turn";
        private const string JUMP_ANIMATOR_KEY = "Jump";
        private const string GROUND_ANIMATOR_KEY = "OnGround";
        private const string FLOOR_TAG = "Floor";
        private const float COLLIDER_HEIGHT = 1.4f;
        private const float COLLIDER_RADIUS = 0.3f;
        
        [SerializeField] RuntimeAnimatorController _animationController;
        [SerializeField] private Vector3 _cameraPosition;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _mouseSpeed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _minY = -5f;

        private GameObject _modelGameObject;
        private Transform _modelCenterTransform;
        private Camera _camera;
        private float _cameraDistance;
        private Animator _characterAnimator;
        private bool _onGround = false;

        private KeyCode[] _moveInputs = new[]
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.UpArrow,
            KeyCode.DownArrow
        };

        private KeyCode[] _rotateInputs = new[]
        {
            KeyCode.A,
            KeyCode.D,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };
        
        void Start()
        {
            SetModel(DataContainer.GetCurrentModelObjectDuplication());
            
            Observable.EveryUpdate()
                .Where(_ => _camera != null && _modelGameObject != null)
                .Subscribe(_ =>
                {
                    _camera.transform.LookAt(_modelCenterTransform);
                }).AddTo(this);

            foreach (var moveInput in _moveInputs)
            {
                Observable.EveryUpdate()
                    .Where(_ => _modelGameObject != null && Input.GetKey(moveInput))
                    .Subscribe(_ =>
                    {
                        transform.position +=
                            _modelGameObject.transform.forward * _moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");
                        _characterAnimator.SetFloat(FORWARD_ANIMATOR_KEY,
                            Mathf.Abs(_moveSpeed * Input.GetAxis("Vertical")) / _moveSpeed);
                    }).AddTo(this);
            }
            
            foreach (var rotateInput in _rotateInputs)
            {
                Observable.EveryUpdate()
                    .Where(_ => _modelGameObject != null && Input.GetKey(rotateInput))
                    .Subscribe(_ =>
                    {
                        _modelGameObject.transform.Rotate(ROTATION_VECTOR,_rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
                        _characterAnimator.SetFloat(TURN_ANIMATOR_KEY,
                            Input.GetAxis("Horizontal"));
                    }).AddTo(this);
            }

            Observable.EveryUpdate()
                .Where(_ => _modelGameObject != null && Input.GetMouseButton((int) MouseButton.RightMouse))
                .Subscribe(_ =>
                {
                    _camera.transform.position
                        = _camera.transform.position +
                          -_camera.transform.right * _mouseSpeed * Time.deltaTime * Input.GetAxis("Mouse X") +
                          -_camera.transform.up * _mouseSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");

                    var directionVector = (_camera.transform.localPosition - _modelCenterTransform.localPosition).normalized;
                    _camera.transform.localPosition = _modelCenterTransform.localPosition +
                                                      directionVector * _cameraDistance;
                }).AddTo(this);
            
            Observable.EveryUpdate()
                .Where(_ => _modelGameObject != null && _onGround && Input.GetKeyDown(KeyCode.Space))
                .Subscribe(_ =>
                {
                    GetComponentInChildren<Rigidbody>().velocity += Vector3.up * _jumpSpeed;
                }).AddTo(this);

            Observable.EveryUpdate()
                .Where(_ => _modelGameObject != null)
                .Subscribe(_ =>
                {
                    if (_moveInputs.All(input => !Input.GetKey(input)))
                    {
                        _characterAnimator.SetFloat(FORWARD_ANIMATOR_KEY, 0);
                    }
                    
                    if (_rotateInputs.All(input => !Input.GetKey(input)))
                    {
                        _characterAnimator.SetFloat(TURN_ANIMATOR_KEY, 0);
                    }

                    var rigidBody = GetComponentInChildren<Rigidbody>();
                    _characterAnimator.SetFloat(JUMP_ANIMATOR_KEY, rigidBody.velocity.y);
                    _characterAnimator.SetBool(GROUND_ANIMATOR_KEY, _onGround);

                    if (rigidBody.transform.position.y < _minY)
                        rigidBody.transform.position = GameObject.Find("Start").transform.position;
                }).AddTo(this);
        }
        
        
        public void SetModel(GameObject modelGameObject)
        {
            modelGameObject.SetActive(true);
            SetupPlayer(modelGameObject);
            
            var cameraGameObject = new GameObject("CharacterCamera");
            _camera = cameraGameObject.AddComponent<Camera>();
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _camera.backgroundColor = new Color(231f / 255f, 209f / 255f, 130f / 255f);
            cameraGameObject.transform.parent = modelGameObject.transform;
            cameraGameObject.transform.localPosition = _cameraPosition;
            _cameraDistance = _cameraPosition.magnitude;
            
            _modelGameObject = modelGameObject;
            _modelCenterTransform = _modelGameObject.transform.FindDescendant("J_Bip_C_UpperChest").transform;
            modelGameObject.transform.parent = transform;
            _characterAnimator = modelGameObject.GetComponent<Animator>();
            
            var capsuleCollider = _modelGameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0, COLLIDER_HEIGHT / 2, 0);
            capsuleCollider.height = COLLIDER_HEIGHT;
            capsuleCollider.radius = COLLIDER_RADIUS;
            capsuleCollider.OnCollisionEnterAsObservable()
                .Merge(capsuleCollider.OnCollisionStayAsObservable())
                .Where(collision => collision.collider.gameObject.tag.Contains(FLOOR_TAG))
                .Subscribe(collision =>
                {
                    var colliderGameObject = collision.collider.gameObject;
                    _onGround = true;
                    transform.parent = colliderGameObject.transform;
                }).AddTo(this);
            capsuleCollider.OnCollisionExitAsObservable()
                .Where(collision => collision.collider.gameObject.tag.Contains(FLOOR_TAG))
                .Subscribe(collision =>
                {
                    _onGround = false;
                    transform.parent = null;
                }).AddTo(this);
            capsuleCollider.OnCollisionEnterAsObservable()
                .Select(collision => collision.collider.GetComponent<CharacterTriggerEvent>())
                .Where(triggerEvent => triggerEvent != null)
                .Subscribe(triggerEvent => triggerEvent.Invoke()).AddTo(this);
            
            var rigidbody = _modelGameObject.AddComponent<Rigidbody>();
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        void SetupPlayer(GameObject characterModel)
        {
            // humanPoseTransfer 追加
            var humanPoseTransfer = characterModel.AddComponent<UniHumanoid.HumanPoseTransfer>();

            var blendShape = humanPoseTransfer.GetComponent<VRMBlendShapeProxy>();
            // ToDo: blendShape コントローラーへの紐づけ 

            var firstPerson = humanPoseTransfer.GetComponent<VRMFirstPerson>();
            firstPerson.Setup();

            // AnimationController の紐づけ
            var animator = humanPoseTransfer.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = _animationController;
            }
        }
    }
}
