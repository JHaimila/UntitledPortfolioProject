using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AISpeaker : Speaker, IInteractable
    {
        [SerializeField] private float _interactRange;
        [SerializeField] private Dialogue _dialouge;
        private DialogueController _dialogueController;
        private void OnEnable() 
        {
            _dialogueController = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueController>();
            _dialouge.CreateLookupTable();
        }
        public float GetInteractRange()
        {
            return _interactRange;
        }

        public void OnInteract()
        {
            if(_dialogueController == null)
            {
                _dialogueController = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueController>();
            }
            _dialogueController.StartDialogue(_dialouge, this);
            if (gameObject.TryGetComponent<StateHandler>(out StateHandler stateHandler))
            {
                stateHandler.Check(Action.InteractedStart);
            }
        }

        public void OnInteractEnd()
        {
            if (gameObject.TryGetComponent<StateHandler>(out StateHandler stateHandler))
            {
                stateHandler.Check(Action.InteractedEnd);
            }
        }

        public bool IsInteractable()
        {
            if(transform.TryGetComponent(out Health health))
            {
               return !health.isDead && !health.Attackable();
            }
            return true;
        }
    }
}