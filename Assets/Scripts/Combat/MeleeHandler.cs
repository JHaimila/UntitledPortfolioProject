using System;
using System.Collections;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat
{
    public class MeleeHandler : MonoBehaviour
    {
        float damage;
        GameObject instigator;
        [SerializeField] AudioSource swingSfx;
        [SerializeField] AudioSource hitSfx;
        private Collider collider;
        private string userTag;

        private void OnEnable()
        {
            collider = GetComponent<Collider>();
        }

        public void SetDamage(float damage)
        {
            this.damage = damage;
        }
        public void SetInstigator(GameObject instigator)
        {
            this.instigator = instigator;
        }
        public void OnSwing()
        {
            swingSfx.Play();
            StartCoroutine(EnableCollider());
        }

        public void SetTag(string newTag)
        {
            userTag = newTag;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals(userTag)){return;}
            
            if(other.TryGetComponent(out Health target))
            {
                hitSfx.Play();
                target.OnAttacked(damage, instigator);
            }
        }

        private IEnumerator EnableCollider()
        {
            collider.enabled = true;
            yield return new WaitForSeconds(1);
            collider.enabled = false;
        }
    }
}