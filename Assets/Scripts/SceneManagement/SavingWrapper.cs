using System;
using System.Collections;
using System.Collections.Generic;
using Saving.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 1;

        // Thie load last scene is called in awake to avoid racing conditions with the experience. When it was in start it wouldn't update the level if you loaded from a save.
        private void Awake() 
        {
            // StartCoroutine(LoadLastScene());
        }

        public void LoadScene()
        {
            StartCoroutine(LoadLastScene());
        }
        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        public bool HasFile()
        {
            return GetComponent<SavingSystem>().HasFile(defaultSaveFile);
        }
        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}

