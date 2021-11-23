using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Yuuta.VRMGo
{
    public class CyclicMove : MonoBehaviour
    {
        [SerializeField] private Vector3 _destinationPosition;
        [SerializeField] private float _speed = 0.02f;

        private bool _shouldGoToDestination = true;
        private void Start()
        {
            var originalPosition = transform.localPosition;
            var speedVector = (_destinationPosition - originalPosition).normalized * _speed; 
                
            Observable.EveryFixedUpdate()
                .Subscribe(_ =>
                {
                    transform.localPosition += (_shouldGoToDestination ? 1 : -1) * 
                                               speedVector * Time.deltaTime;
                    
                    if (_IsOverLine(originalPosition, _destinationPosition))
                        _shouldGoToDestination = !_shouldGoToDestination;
                });
        }

        private bool _IsOverLine(Vector3 point1, Vector3 point2)
            => Mathf.Abs((transform.localPosition - point1).magnitude +
                (transform.localPosition - point2).magnitude - 
                (point1 - point2).magnitude) > 0.05f;
    }
}

