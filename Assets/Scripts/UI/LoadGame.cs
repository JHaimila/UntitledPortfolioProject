using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class LoadGame : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SavingWrapper savingWrapper = GameObject.FindObjectOfType<SavingWrapper>();
            Button button = GetComponent<Button>();
            
            button.interactable = savingWrapper.HasFile();
            if (button.interactable)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(savingWrapper.LoadScene);
            }
        }
    }
}