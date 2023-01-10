using System.Collections;
using System.Collections.Generic;
using RPG.Control.PlayerController;
using UnityEngine;

namespace InventorySystem.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        InputReader inputReader;
        InteractionHandler interactionHandler;
        private bool isActive = false;
        private void Awake() {
            var player = GameObject.FindWithTag("Player");
            inputReader = player.GetComponent<InputReader>();
            inputReader.ToggleInventory += ToggleInventory;
            interactionHandler = player.GetComponent<InteractionHandler>();
            transform.GetChild(0).gameObject.SetActive(isActive);
        }

        private void OnDestroy() {
            inputReader.ToggleInventory -= ToggleInventory;
        }

        public void ToggleInventory()
        {
            isActive = !isActive;
            interactionHandler.uiOpen = isActive;
            transform.GetChild(0).gameObject.SetActive(isActive);
        }
    }
}