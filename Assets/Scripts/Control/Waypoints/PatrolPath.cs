using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> wayPoints;
        private int currentWaypointIndex = 0;

        #if UNITY_EDITOR
        [SerializeField] private float WaypointGizmoRadius = 1;
        #endif
        private void Awake() {
            wayPoints = new List<Waypoint>();
            currentWaypointIndex = 0;
            if(transform.childCount > 0)
            {
                for(int i = 0; i < transform.childCount; i++ )
                {
                    wayPoints.Add(transform.GetChild(i).gameObject.GetComponent<Waypoint>());
                }
            }
        }
        public Waypoint GetCurrentWaypoint()
        {
            return wayPoints[currentWaypointIndex];
        }
        public void SwitchWaypoint()
        {
            if(wayPoints.Count > currentWaypointIndex + 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                if(!wayPoints[currentWaypointIndex].stay)
                {
                    currentWaypointIndex = 0;
                }
            }
        }
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected() 
        {
            for(int i = 0; i < transform.childCount; i++ )
            {
                GameObject waypoint = transform.GetChild(i).gameObject;
                if(transform.GetChild(i) == transform.GetChild(0))
                {
                    Gizmos.color = Color.green;
                }
                else if(i + 1 >= transform.childCount)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.gray;
                }
                Gizmos.DrawSphere(waypoint.transform.position, WaypointGizmoRadius);

                Gizmos.color = Color.gray;

                if(i+1 < transform.childCount)
                {
                    if(!transform.GetChild(i).GetComponent<Waypoint>().stay)
                    {
                        Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i+1).transform.position);
                    }
                }
                else
                {
                    if(!transform.GetChild(i).GetComponent<Waypoint>().stay)
                    {
                        Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(0).transform.position);
                    }
                }
            }
        }
        #endif
    }
}

