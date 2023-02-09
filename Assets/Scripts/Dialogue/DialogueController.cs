using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Control.PlayerController;
using UnityEngine;
using RPG.Core;

namespace RPG.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        private Dialogue currentDialogue;
        private DialogueNode currentNode;
        public event Action UpdatedNode;
        private bool isChoosing = false;
        private Speaker _currentSpeaker;
        private AISpeaker _aiSpeaker;
        private Speaker _playerSpeaker;
        
        public void StartDialogue(Dialogue newDialogue, AISpeaker speaker)
        {
            _aiSpeaker = speaker;
            if(_playerSpeaker == null)
            {
                _playerSpeaker = GameObject.FindGameObjectWithTag("Player").GetComponent<Speaker>();
            }
            _currentSpeaker = _aiSpeaker;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            gameObject.GetComponent<PlayerStateMachine>().FreezePlayer();
            UpdatedNode?.Invoke();
        }
        public string GetText()
        {
            if(currentDialogue == null)
            {
                return "";
            }
            return currentNode.GetText();
        }
        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if(numPlayerResponses > 0)
            {
                SetIsChoosing(true);
                TriggerExitAction();
                UpdatedNode?.Invoke();
                return;
            }
            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            UpdatedNode?.Invoke();
        }
        public void SelectChoice(DialogueNode choice)
        {
            currentNode = choice;
            TriggerEnterAction();
            SetIsChoosing(false);
            Next();
        }
        public bool HasNext()
        {
            if(FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsActive()
        {
            return currentDialogue != null;
        }
        public bool IsChoosing()
        {
            return isChoosing;
        }
        private void SetIsChoosing(bool state)
        {
            isChoosing = state;
            ChangeSpeaker();
        }
        private void ChangeSpeaker()
        {
            if(isChoosing)
            {
                _currentSpeaker = _playerSpeaker;
            }
            else
            {
                _currentSpeaker = _aiSpeaker;
            }
        }
        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        }
        public void EndDialogue()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentNode = null;
            isChoosing = false;
            _aiSpeaker.OnInteractEnd();
        }

        private void TriggerEnterAction()
        {
            if(currentNode != null)
            {
                TriggerAction(currentNode.GetEnterActions());
            }
        }
        private void TriggerExitAction()
        {
            if(currentNode != null)
            {
                TriggerAction(currentNode.GetExitActions());
            }
        }
        private void TriggerAction(string action)
        {
            if(String.IsNullOrEmpty(action)){return;}

            foreach(DialogueTrigger trigger in _currentSpeaker.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }
        public void UnfreezePlayer()
        {
            gameObject.GetComponent<PlayerStateMachine>().UnFreezePlayer();
        }

        internal string GetSpeakerName()
        {
            return _currentSpeaker.GetName();
        }
        internal Sprite GetSpeakerIcon()
        {
            return _currentSpeaker.GetIcon();
        }
        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach(var node in inputNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }
    }
}