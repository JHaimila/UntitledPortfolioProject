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
        PlayerSpeaker playerSpeaker;
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
            playerSpeaker = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSpeaker>();
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(Next);
        }
        private void Start()
        {
            UpdateUI();
            playerSpeaker.UpdatedNode += UpdateUI;
        }
        private void OnDestroy() {
            playerSpeaker.UpdatedNode -= UpdateUI;
        }

        private void Next()
        {
            playerSpeaker.Next();
            UpdateUI();
        }
        private void UpdateUI()
        {
            if(!playerSpeaker.IsActive()){return;}
            if(!gameObject.activeSelf){gameObject.SetActive(true);}

            speakerName.text = playerSpeaker.GetSpeakerName();
            _icon.sprite = playerSpeaker.GetSpeakerIcon();
            aiTextParent.SetActive(!playerSpeaker.IsChoosing());
            optionParent.gameObject.SetActive(playerSpeaker.IsChoosing());
            if(playerSpeaker.IsChoosing())
            {
                convoLine.text = playerSpeaker.GetText();
                if(optionContainer.childCount > 0)
                {
                    for(int i =0; i<optionContainer.childCount; i++)
                    {
                        Destroy(optionContainer.GetChild(i).gameObject);
                    }
                }
                foreach(DialogueNode choice in playerSpeaker.GetChoices())
                {
                    GameObject newOption = Instantiate(optionPrefab, optionContainer);
                    newOption.GetComponent<Button>().onClick.AddListener(delegate 
                    {
                        playerSpeaker.SelectChoice(choice);
                        UpdateUI();
                    });
                    newOption.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = choice.GetText();
                }
            }
            else
            {
                aiLine.text = playerSpeaker.GetText();
            }
            if(!playerSpeaker.HasNext())
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
            playerSpeaker.EndDialogue();
            playerSpeaker.UnfreezePlayer();
            gameObject.SetActive(false);
        }
    }
}