using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace InventorySystem.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [SerializeField] float scatterDistance = 2;

        const int ATTEMPTS = 30;
        protected override Vector3 GetDropLocation()
        {
            for(int i = 0; i < ATTEMPTS; i++)
            {   
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
            
                NavMeshHit hit;
                if(NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position + new Vector3(1,1,1);
        }
    }
}

