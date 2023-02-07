using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class Volume : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume");
        }

        public void ChangeVolume(float newVolume)
        {
            PlayerPrefs.SetFloat("volume", newVolume);
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
        }
    }  
}