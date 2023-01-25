using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using InventorySystem.Inventories;

namespace InventorySystem
{
    public class Container : MonoBehaviour, IInteractable
    {
        [SerializeField] private float range;
        [SerializeField] private List<ContainerItem> items = new List<ContainerItem>();
        [SerializeField] private int quantity;
        public float GetInteractRange()
        {
            return range;
        }

        public void OnInteract()
        {
            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            foreach(ContainerItem containerItem in items)
            {
                inventory.AddToFirstEmptySlot(containerItem.item, containerItem.quantity);
            }
            
        }
    }
    [System.Serializable]
    public class ContainerItem
    {
        public InventoryItem item;
        public int quantity;
    }
}