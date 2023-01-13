using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI
{
    public class TextBlockUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;

        public void Initialize(string dialogueText, string nameText, Sprite icon)
        {
            _dialogueText.text = dialogueText;
            _nameText.text = nameText;
            _icon.sprite = icon;
        }
        public void SetDialogueText(string text)
        {
            _dialogueText.text = text;
        }
    }
}


