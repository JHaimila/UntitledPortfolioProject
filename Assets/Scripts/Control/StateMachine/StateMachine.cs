using UnityEngine;
using Saving.Saving;
using System.Collections;

namespace RPG.Control
{
    public abstract class StateMachine : MonoBehaviour, ISaveable
    {
        protected State currentState;

        // Update is called once per frame
        void Update()
        {
            if(currentState != null)
            {
                currentState.Tick(Time.deltaTime);
            }
        }

        public void SwitchState(State newState)
        {
            if(currentState != null)
            {
                currentState.Exit();
            }
            currentState = newState;
            if(currentState != null)
            {
                currentState.Enter();
            }
        }
        public void BeginCorotine(IEnumerator enumerator)
        {
            StartCoroutine(enumerator);
        }
        public void EndAllCorotines()
        {
            StopAllCoroutines();
        }

        public abstract object CaptureState();

        public abstract void RestoreState(object state);
    }
}

