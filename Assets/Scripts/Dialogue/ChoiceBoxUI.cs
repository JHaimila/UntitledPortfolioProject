using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI
{
    public class ChoiceBoxUI : MonoBehaviour
    {
        [SerializeField] private TextBlockUI _textBlock;
        [SerializeField] private Transform _choiceContainer;
        [SerializeField] private GameObject _choicePrefab;

        private DialogueController _dialogueController;

        public void Initialize(DialogueController dialogueController)
        {
            _dialogueController = dialogueController;
            _textBlock.Initialize("", _dialogueController.GetSpeakerName(), _dialogueController.GetSpeakerIcon());
            foreach(DialogueNode choice in _dialogueController.GetChoices())
            {
                GameObject newChoice = Instantiate(_choicePrefab, _choiceContainer);
                    newChoice.GetComponent<ChoiceUI>().Initialize(delegate
                        {
                            OnChoiceSelected(choice.GetText());
                            _dialogueController.SelectChoice(choice);
                        }, 
                        choice.GetText());
            }
        }
        public void OnChoiceSelected(string chosenText)
        {
            Destroy(_choiceContainer.gameObject);
            _textBlock.SetDialogueText(chosenText);
        }
    }
}