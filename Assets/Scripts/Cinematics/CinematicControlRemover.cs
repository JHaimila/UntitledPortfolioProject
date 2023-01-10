using RPG.Control.PlayerController;
using UnityEngine;
using UnityEngine.Playables;

namespace Saving.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        private void OnDisable() 
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().FreezePlayer();
        }
        private void EnableControl(PlayableDirector pd)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>().UnFreezePlayer();
        }
    }
}

