using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private string _action;
        [SerializeField] private UnityEvent _onTrigger;
        [SerializeField] private List<DialogueAction> actions;
        
        
        public void Trigger(string actionToTrigger)
        {
            foreach (var action in actions)
            {
                if(actionToTrigger == action.actionTrigger)
                {
                    action.onTrigger?.Invoke();
                }   
            }
        }
    }
    [Serializable]
    class DialogueAction
    {
        public string actionTrigger;
        public UnityEvent onTrigger;
    }
}