using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AISpeaker : Speaker, IInteractable
    {
        [SerializeField] private float _interactRange;
        [SerializeField] private Dialogue _dialouge;
        private PlayerSpeaker _playerSpeaker;
        private void OnEnable() {
            _playerSpeaker = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSpeaker>();
        }
        public float GetInteractRange()
        {
            return _interactRange;
        }

        public void OnInteract()
        {
            if(_playerSpeaker == null)
            {
                _playerSpeaker = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSpeaker>();
            }
            _playerSpeaker.StartDialogue(_dialouge, this);
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