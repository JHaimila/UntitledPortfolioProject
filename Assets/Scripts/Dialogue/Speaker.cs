using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Speaker : MonoBehaviour
    {
        [SerializeField] private string speakerName;
        [SerializeField] private Sprite speakerIcon;

        public void ChangeIcon(Sprite icon)
        {
            speakerIcon = icon;
        }
        public string GetName()
        {
            return speakerName;
        }
        public Sprite GetIcon()
        {
            return speakerIcon;
        }
        
    }
}

