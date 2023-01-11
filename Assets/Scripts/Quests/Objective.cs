using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class Objective
    {
        [SerializeField] private string _reference;
        [SerializeField] private string _description;
        [SerializeField] private bool _optional = false;
        [SerializeField] private bool _hasTrigger = false;

        public string GetReference(){return _reference;}
        public string GetDescription(){return _description;}
        public bool IsOptional(){return _optional;}
        public bool HasTrigger(){return _hasTrigger;}
    }
}