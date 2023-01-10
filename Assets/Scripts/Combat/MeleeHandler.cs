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
        }
        private void OnTriggerEnter(Collider other) {
            if(other.TryGetComponent<Health>(out Health target))
            {
                if(GetComponent<Collider>().enabled)
                {
                    hitSfx.Play();
                    target.OnAttacked(damage, instigator);
                }
            }
        }
    }
}