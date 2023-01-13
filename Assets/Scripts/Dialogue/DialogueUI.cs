using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        
        [SerializeField] private Transform _dialogueContainer;
        [SerializeField] private Transform _choiceContainer;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameObject _textBlockPrefab;
        [SerializeField] private GameObject _choicePrefab;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private GameObject _appear;

        private DialogueController _dialogueController;

        private void Awake() 
        {
            _dialogueController = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueController>();
        }
        private void Start()
        {
            UpdateUI();
            _dialogueController.UpdatedNode += UpdateUI;
        }
        private void OnDestroy() {
            _dialogueController.UpdatedNode -= UpdateUI;
        }

        public void Next()
        {
            _dialogueController.Next();
        }
        private void UpdateUI()
        {
            if(!_dialogueController.IsActive()){return;}
            if(!_appear.activeSelf)
            {
                _appear.SetActive(true);
            }
            
            if(_dialogueController.IsChoosing())
            {
                Debug.Log("Is Hcoosing");
                _nextButton.gameObject.SetActive(false);

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
            else
            {
                _nextButton.gameObject.SetActive(true);
                GameObject newTextBlock = Instantiate(_textBlockPrefab, _dialogueContainer);
                newTextBlock.GetComponent<TextBlockUI>().Initialize(_dialogueController.GetText(), _dialogueController.GetSpeakerName(), _dialogueController.GetSpeakerIcon());
            }

            if(!_dialogueController.HasNext())
            {
                _nextButton.gameObject.SetActive(false);
                _exitButton.gameObject.SetActive(true);
            }
            else
            {
                if(!_nextButton.IsActive() && !_dialogueController.IsChoosing())
                {
                    _nextButton.gameObject.SetActive(true);
                }
                if(_exitButton.gameObject.activeSelf)
                {
                    _exitButton.gameObject.SetActive(false);
                }
            }
            ScrollToBottom();
        }

        public void Close()
        {
            _dialogueController.EndDialogue();
            _dialogueController.UnfreezePlayer();
            for(int i = 0; i < _dialogueContainer.transform.childCount; i++)
            {
                Destroy(_dialogueContainer.transform.GetChild(i).gameObject);
            }
            _appear.SetActive(false);
        }
        public void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();

            _dialogueContainer.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical() ;
            _dialogueContainer.GetComponent<ContentSizeFitter>().SetLayoutVertical() ;

            _scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical() ;
            _scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical() ;

            _scrollRect.verticalNormalizedPosition = 0 ;
        }
        private void OnChoiceSelected(string chosenText)
        {
            GameObject newTextBlock = Instantiate(_textBlockPrefab, _dialogueContainer);
            newTextBlock.GetComponent<TextBlockUI>().Initialize(chosenText, _dialogueController.GetSpeakerName(), _dialogueController.GetSpeakerIcon());

            for(int i = 0; i < _choiceContainer.transform.childCount; i++)
            {
                Destroy(_choiceContainer.transform.GetChild(i).gameObject);
            }
        }
    }
}