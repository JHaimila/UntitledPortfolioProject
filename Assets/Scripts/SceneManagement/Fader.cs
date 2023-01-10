using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        // https://www.gamedev.tv/courses/637539/lectures/11879291
        CanvasGroup canvasGroup;
        Coroutine activeFade = null;
        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            Debug.Log("FADE OUT");
            return Fade(1, time);
        }
        public Coroutine FadeIn(float time)
        {
            Debug.Log("FADE IN");
            return Fade(0, time);
        }

        public Coroutine Fade(float targetValue, float time)
        {
            if(activeFade != null)
            {
                StopCoroutine(activeFade);
            }
            activeFade = StartCoroutine(FadeRoutine(time, targetValue));
            return activeFade;
        }

        private IEnumerator FadeRoutine(float time, float targetValue)
        {
            while(!Mathf.Approximately(canvasGroup.alpha, targetValue))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetValue, Time.deltaTime / time);
                // tells unity to wait for the next frame
                yield return null;
            }
        }
    }
}

