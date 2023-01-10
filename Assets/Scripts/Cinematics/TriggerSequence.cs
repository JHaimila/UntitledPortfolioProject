using UnityEngine;
using UnityEngine.Playables;

public class TriggerSequence : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private bool multiTrigger;

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag.Equals("Player"))
        {
            director.Play();
            if(!multiTrigger)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
