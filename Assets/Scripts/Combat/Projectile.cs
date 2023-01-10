using RPG.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        Health target; 
        [SerializeField] float speed = 2;
        [SerializeField] bool followsTarget;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent HitEvent;
        float damage;
        GameObject instigator = null;        
        // Update is called once per frame
        void Update()
        {
            if(target == null) {return;}

            if(followsTarget && !target.isDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage, GameObject instigator)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if(targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * targetCollider.height / 2;
        }
        private void OnTriggerEnter(Collider other) 
        {
            if(other.GetComponent<Health>() != target){return;}
            
            
            if(target.isDead){
                Destroy(gameObject, 5f);
                return;
            }
            speed = 0;
            HitEvent?.Invoke();
            target.OnAttacked(damage, instigator);

            if(hitEffect != null)
            {
                GameObject t_effect = Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}