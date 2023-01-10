using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    public class Speaker : MonoBehaviour
    {
        public string speakerName;
        [SerializeField] protected Sprite speakerIcon;

        public void ChangeIcon(Sprite icon)
        {
            speakerIcon = icon;
        }
    }
}

