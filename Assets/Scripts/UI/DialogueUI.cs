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
        private DialogueController _dialogueController;
        [SerializeField] private TextMeshProUGUI speakerName;
        [SerializeField] private TextMeshProUGUI convoLine;
        [SerializeField] private TextMeshProUGUI aiLine;
        [SerializeField] private Button option1;
        [SerializeField] private Button nextButton;
        [SerializeField] private GameObject optionParent;
        [SerializeField] private GameObject aiTextParent;
        [SerializeField] private Transform optionContainer;
        [SerializeField] private GameObject optionPrefab;
        [SerializeField] private Image _icon;

        private void Awake() 
        {
            _dialogueController = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueController>();
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(Next);
        }
        private void Start()
        {
            UpdateUI();
            _dialogueController.UpdatedNode += UpdateUI;
        }
        private void OnDestroy() {
            _dialogueController.UpdatedNode -= UpdateUI;
        }

        private void Next()
        {
            _dialogueController.Next();
            UpdateUI();
        }
        private void UpdateUI()
        {
            if(!_dialogueController.IsActive()){return;}
            if(!gameObject.activeSelf){gameObject.SetActive(true);}

            speakerName.text = _dialogueController.GetSpeakerName();
            _icon.sprite = _dialogueController.GetSpeakerIcon();
            aiTextParent.SetActive(!_dialogueController.IsChoosing());
            optionParent.gameObject.SetActive(_dialogueController.IsChoosing());
            if(_dialogueController.IsChoosing())
            {
                convoLine.text = _dialogueController.GetText();
                if(optionContainer.childCount > 0)
                {
                    for(int i =0; i<optionContainer.childCount; i++)
                    {
                        Destroy(optionContainer.GetChild(i).gameObject);
                    }
                }
                foreach(DialogueNode choice in _dialogueController.GetChoices())
                {
                    GameObject newOption = Instantiate(optionPrefab, optionContainer);
                    newOption.GetComponent<Button>().onClick.AddListener(delegate 
                    {
                        _dialogueController.SelectChoice(choice);
                        UpdateUI();
                    });
                    newOption.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = choice.GetText();
                }
            }
            else
            {
                aiLine.text = _dialogueController.GetText();
            }
            if(!_dialogueController.HasNext())
            {
                nextButton.gameObject.SetActive(false);
            }
            else
            {
                if(!nextButton.IsActive())
                {
                    nextButton.gameObject.SetActive(true);
                }
            }
            
        }

        public void Close()
        {
            _dialogueController.EndDialogue();
            _dialogueController.UnfreezePlayer();
            gameObject.SetActive(false);
        }
    }
}