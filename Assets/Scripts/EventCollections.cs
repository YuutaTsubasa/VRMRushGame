using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Yuuta.VRMGo
{
    public class EventCollections : MonoBehaviour
    {
        [SerializeField] private UnityEvent _events = new UnityEvent();

        public event UnityAction Events
        {
            add => _events.AddListener(value);
            remove => _events.RemoveListener(value);
        }

        public void Invoke()
        {
            _events?.Invoke();
        }
    }
}