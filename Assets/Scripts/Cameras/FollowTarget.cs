using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] GameObject target;

    private void Update() {
        if(target.transform.position != gameObject.transform.position)
        {
            transform.position = target.transform.position;
        }
    }
}
