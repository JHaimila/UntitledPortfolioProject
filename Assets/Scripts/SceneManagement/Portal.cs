using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control.PlayerController;
using Saving.Saving;
using UnityEngine.Events;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour, ISaveable
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;

        [SerializeField] private float fadeInTime = 2;
        [SerializeField] private float fadeOutTime = 1;
        [SerializeField] private float fadeWaitTime = .5f;

        [SerializeField] private bool locked;
        [SerializeField] private ParticleSystem VFX;

        private void Start()
        {
            if (locked)
            {
                VFX.Pause();
                VFX.gameObject.SetActive(false);
            }
            else
            {
                VFX.Play();
                VFX.gameObject.SetActive(true);
            }
        }

        public void SetLocked(bool state)
        {
            locked = state;
            if (locked)
            {
                VFX.Pause();
                VFX.gameObject.SetActive(false);
            }
            else
            {
                VFX.Play();
                VFX.gameObject.SetActive(true);
            }
        }
        private void OnTriggerEnter(Collider other) 
        {
            if(locked){return;}
            
            if(other.tag.Equals("Player"))
            {
                StartCoroutine(Transition());
            }
        }
        private IEnumerator Transition()
        {
            if(sceneToLoad < 0)
            {
                yield break;
            }

            DontDestroyOnLoad(gameObject);
            
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().FreezePlayer();

            fader.FadeOut(fadeOutTime);
            
            //Save Current Level
            
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().FreezePlayer();

            //Load Current Level
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            savingWrapper.Save();


            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            
            GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().UnFreezePlayer();
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject Player = GameObject.FindWithTag("Player");
            Player.transform.position = otherPortal.spawnPoint.position;
            Player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            Player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) {continue;}
                if(portal.destination != this.destination){continue;}
                return portal;
            }
            return null;
        }

        public object CaptureState()
        {
            PortalRecord newRecord = new PortalRecord();
            newRecord.locked = locked;
            return newRecord;
        }

        public void RestoreState(object state)
        {
            PortalRecord restoredRecord = state as PortalRecord;
            this.locked = restoredRecord.locked;
            SetLocked(locked);
        }
    }

    [Serializable]
    class PortalRecord
    {
        public bool locked;
    }
}