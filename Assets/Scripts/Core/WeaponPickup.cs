using RPG.Combat;
using UnityEngine;

namespace RPG.Core
{
    public class WeaponPickup : Pickup
    {
        [field:SerializeField] public Weapon weapon{get; private set;}

        public override void OnInteract()
        {
            Destroy(gameObject);
        }
    }
}