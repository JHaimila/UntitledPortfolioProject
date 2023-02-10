using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class ExitGame : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Application.Quit);
        }
    }
}