using System;
using UnityEngine;
using UnityEngine.Events;

namespace  RPG.Core
{
    [RequireComponent(typeof(BoxCollider))]
    public class AreaTrigger : MonoBehaviour
    {
        [SerializeField] private UnityEvent triggers;
        [SerializeField] private bool singleUse = true;

        private void Awake()
        {
            transform.GetComponent<Collider>().isTrigger = true;
        }

        private bool triggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if(singleUse && triggered){return;}
            
            if (other.tag.Equals("Player"))
            {
                triggers?.Invoke();
                triggered = true;
            }
        }
    }
}


