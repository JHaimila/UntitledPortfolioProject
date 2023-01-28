using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class FaceCamera : MonoBehaviour
    {
        private Camera cam;
        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            transform.LookAt(cam.transform);
        }
    }   
}