using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Yuuta.VRMGo
{
    public class CharacterTriggerEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _triggerEvents;

        public void Invoke()
        {
            _triggerEvents?.Invoke();
        }
    }
}