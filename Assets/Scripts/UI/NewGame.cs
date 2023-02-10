using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG.UI
{
    public class NewGame : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
            Button button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(StartNewGame);
        }

        private void StartNewGame()
        {
            SavingWrapper savingWrapper = GameObject.FindObjectOfType<SavingWrapper>();
            savingWrapper.Delete();
            SceneManager.LoadSceneAsync(1);
        }
    }
}