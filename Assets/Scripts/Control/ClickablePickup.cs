using RPG.Core;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(InventorySystem.Inventories.Pickup))]
    public class ClickablePickup : MonoBehaviour, IInteractable
    {
        [SerializeField] float interactionRange;

        private InventorySystem.Inventories.Pickup pickup;

        private void Awake() 
        {
            pickup = GetComponent<InventorySystem.Inventories.Pickup>();
        }

        

        public float GetInteractRange()
        {
            return interactionRange;
        }

        public void OnInteract()
        {
            pickup.PickupItem();
        }

        public bool IsInteractable()
        {
            return true;
        }
    }
}