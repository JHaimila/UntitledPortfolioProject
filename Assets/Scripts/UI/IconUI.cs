using System.Collections;
using System.Collections.Generic;
using RPG.Dialogue;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI
{
    public class IconUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        // Start is called before the first frame update
        void Start()
        {
            icon.sprite = GameObject.FindGameObjectWithTag("Player").GetComponent<Speaker>().GetIcon();
        }
    }
}