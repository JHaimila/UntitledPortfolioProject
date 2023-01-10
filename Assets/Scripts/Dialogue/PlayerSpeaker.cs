using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Control.PlayerController;
using UnityEngine;
using RPG.Core;

namespace RPG.Dialogue
{
    public class PlayerSpeaker : Speaker
    {
        [SerializeField] private Dialogue _testDialogue;
        private Dialogue currentDialogue;
        private DialogueNode currentNode;
        public event Action UpdatedNode;
        private bool isChoosing = false;
        private AISpeaker _currentSpeaker;

        public void StartDialogue(Dialogue newDialogue, AISpeaker speaker)
        {
            _currentSpeaker = speaker;
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
                Debug.Log("NO DIalogue is selected rn");
                return "";
            }
            return currentNode.GetText();
        }
        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
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
            isChoosing = false;
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
        }

        private void TriggerEnterAction()
        {
            Debug.Log("EnterAction");
            if(currentNode != null)
            {
                TriggerAction(currentNode.GetEnterActions());
            }
        }
        private void TriggerExitAction()
        {
            Debug.Log("ExitAction");
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
                Debug.Log("ACTION TRIGGERED");
                trigger.Trigger(action);
            }
        }
        public void UnfreezePlayer()
        {
            gameObject.GetComponent<PlayerStateMachine>().UnFreezePlayer();
        }

        internal string GetSpeakerName()
        {
            if(isChoosing)
            {
                return speakerName;
            }
            else
            {
                return _currentSpeaker.GetName();
            }
        }
        internal Sprite GetSpeakerIcon()
        {
            if(isChoosing)
            {
                return speakerIcon;
            }
            else
            {
                return _currentSpeaker.GetIcon();
            }
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