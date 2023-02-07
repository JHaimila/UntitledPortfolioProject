using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class VolumeSetter : MonoBehaviour
    {
        private void OnEnable()
        {
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
    }
}