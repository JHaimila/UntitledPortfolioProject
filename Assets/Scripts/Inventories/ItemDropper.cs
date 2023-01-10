using System.Collections.Generic;
using UnityEngine;
using Saving.Saving;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace InventorySystem.Inventories
{
    /// <summary>
    /// To be placed on anything that wishes to drop pickups into the world.
    /// Tracks the drops for saving and restoring.
    /// </summary>
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        // STATE
        private List<Pickup> droppedItems = new List<Pickup>();
        private List<DropRecord> otherSceneDropItems = new List<DropRecord>();

        [SerializeField] float dropRadius = 1;

        // PUBLIC

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        /// <param name="number">
        /// The number of items contained in the pickup. Only used if the item
        /// is stackable.
        /// </param>
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }

        /// <summary>
        /// Create a pickup at the current position.
        /// </summary>
        /// <param name="item">The item type for the pickup.</param>
        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation(), 1);
        }

        // PROTECTED

        /// <summary>
        /// Override to set a custom method for locating a drop.
        /// </summary>
        /// <returns>The location the drop should be spawned.</returns>
        protected virtual Vector3 GetDropLocation()
        {
            // return transform.position;
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * dropRadius;
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            Debug.Log("Item dropper dropped");
            return transform.position;
        }

        // PRIVATE

        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            var pickup = item.SpawnPickup(spawnLocation, number);
            droppedItems.Add(pickup);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
            public int sceneBuildIndex;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new List<DropRecord>();
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            for (int i = 0; i < droppedItemsList.Count; i++)
            {
                var droppedItem = new DropRecord();
                droppedItem.itemID = droppedItems[i].GetItem().GetItemID();
                droppedItem.position = new SerializableVector3(droppedItems[i].transform.position);
                droppedItem.number = droppedItems[i].GetNumber();
                droppedItem.sceneBuildIndex = buildIndex;
                droppedItemsList.Add(droppedItem);
            }
            droppedItemsList.AddRange(otherSceneDropItems);
            return droppedItemsList;
        }

        void ISaveable.RestoreState(object state)
        {
            var droppedItemsList = (List<DropRecord>)state;
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            otherSceneDropItems.Clear();
            foreach (var item in droppedItemsList)
            {
                if(item.sceneBuildIndex != buildIndex)
                {
                    otherSceneDropItems.Add(item);
                    continue;
                }
                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
        }

        /// <summary>
        /// Remove any drops in the world that have subsequently been picked up.
        /// </summary>
        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }
    }
}