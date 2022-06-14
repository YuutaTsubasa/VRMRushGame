using UnityEngine;

namespace Yuuta.VRMGo
{
    public class AddScoreItem : MonoBehaviour
    {
        [SerializeField] private int _score;

        public int Score => _score;
    }
}