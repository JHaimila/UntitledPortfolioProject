using System;
using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.Core
{
    public class WeaponPickup : Pickup
    {
        [field:SerializeField] public Weapon weapon{get; private set;}

        [SerializeField] private TextMeshProUGUI nameText;

        private void OnEnable()
        {
            if (weapon)
            {
                nameText.text = weapon.GetDisplayName();
            }
        }

        public override void OnInteract()
        {
            Destroy(gameObject);
        }
    }
}