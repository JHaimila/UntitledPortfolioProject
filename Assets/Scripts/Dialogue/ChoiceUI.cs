using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI
{
    public class ChoiceUI : MonoBehaviour
    {
        [SerializeField] private Button _choiceBtn;
        [SerializeField] private TextMeshProUGUI _choiceText;

        public void Initialize(UnityEngine.Events.UnityAction buttonAction, string buttonText)
        {
            _choiceBtn.onClick.AddListener(buttonAction);
            _choiceText.text = buttonText;
        }
    }
}

