using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DeadUI : MonoBehaviour
    {
        [SerializeField] private Button loadBtn, newGameBtn;

        [SerializeField] private GameObject appear;
        // Start is called before the first frame update
        void Start()
        {
            GameObject.FindWithTag("Player").GetComponent<Health>().DeathEvent += OnDeath;
            
            
        }

        // private void OnDisable()
        // {
        //     GameObject.FindWithTag("Player").GetComponent<Health>().DeathEvent -= OnDeath;
        // }

        private void OnDeath()
        {
            appear.SetActive(true);
            if (loadBtn.interactable)
            {
                loadBtn.onClick.AddListener(delegate { appear.SetActive(false); });
                loadBtn.gameObject.SetActive(true);
                newGameBtn.gameObject.SetActive(false);
            }
            else
            {
                newGameBtn.onClick.AddListener(delegate { appear.SetActive(false); });
                newGameBtn.gameObject.SetActive(true);
                loadBtn.gameObject.SetActive(false);
            }
        }
    }
}