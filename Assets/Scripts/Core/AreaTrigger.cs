using System;
using RPG.Control;
using Saving.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace  RPG.Core
{
    [RequireComponent(typeof(BoxCollider))]
    public class AreaTrigger : MonoBehaviour, ISaveable
    {
        [SerializeField] private UnityEvent triggers;
        [SerializeField] private bool singleUse = true;

        private void Awake()
        {
            transform.GetComponent<Collider>().isTrigger = true;
        }

        [SerializeField] private bool triggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if(singleUse && triggered){return;}
            
            if (other.tag.Equals("Player"))
            {
                triggers?.Invoke();
                triggered = true;
            }
        }

        public object CaptureState()
        {
            AreaTriggerRecord saveState = new AreaTriggerRecord();
            saveState.triggered = triggered;
            return saveState;
        }

        public void RestoreState(object state)
        {
            AreaTriggerRecord record = state as AreaTriggerRecord;
            if (record != null)
            {
                this.triggered = record.triggered;
            }
            
        }
    }
    [System.Serializable]
    class AreaTriggerRecord
    {
        public bool triggered;
    }
}


