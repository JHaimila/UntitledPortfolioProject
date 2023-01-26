using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using InventorySystem.Inventories;

namespace InventorySystem
{
    [RequireComponent(typeof(ItemDropper))]
    public class Container : MonoBehaviour, IInteractable
    {
        [SerializeField] private float interactRange;
        [SerializeField] private List<ContainerItem> items = new List<ContainerItem>();
        public float GetInteractRange()
        {
            return interactRange;
        }

        public bool IsInteractable()
        {
            return items.Count > 0;
        }

        public void OnInteract()
        {
            if(items.Count==0){return;}

            Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            RandomDropper itemDropper = gameObject.GetComponent<RandomDropper>();
            foreach(ContainerItem containerItem in items)
            {
                itemDropper.DropItem(containerItem.item, containerItem.quantity);
            }
            items.Clear();
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 dropPosition = new Vector3();
            dropPosition.x = Random.Range(0,2);
            dropPosition.y = Random.Range(0,2);
            dropPosition.x = 0;
            return dropPosition;
        }
    }
    [System.Serializable]
    public class ContainerItem
    {
        public InventoryItem item;
        public int quantity;
    }
}