using RPG.Attributes;
using UnityEngine;
using InventorySystem.Inventories;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName ="Weapon", menuName ="Weapon/Weapon", order = 0)]
    public class Weapon : EquipableItem, IModifierProvider
    {
        // [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [field:SerializeField] public float Range{get; private set;}
        [field:SerializeField] public float Damage{get; private set;}
        [field:SerializeField] public float PercentageBonus{get; private set;} = 0;
        [field:SerializeField] public bool isRightHand{get; private set;}
        [field:SerializeField] public Projectile Projectile{get; private set;} = null;
        [field:SerializeField] public string AnimationString{get; private set;} = null;

        private bool colliderState = false;
        private float additionalDamage;

        // private GameObject loadedWeapon;
        private GameObject instigator;
        const string weaponName = "Weapon";


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator, float totalDamage, string newTag)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Transform handTransform = GetTransform(rightHand, leftHand);

            if (equippedPrefab != null)
            {
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                this.additionalDamage = totalDamage;
                if(!HasProjectile())
                {
                    MeleeHandler meleeHandler = weapon.GetComponent<MeleeHandler>();
                    meleeHandler.SetDamage(totalDamage);
                    meleeHandler.SetTag(newTag);
                }

                weapon.name = weaponName;

            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null){return;}
            // Chaning the name makes it so that theres no confusion for if we want to destroy the old one or the one that is replacing it. Unity be weird sometimes
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHand)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return Projectile != null;
        }
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float totalDamage)
        {
            if(Projectile != null)
            {
                Projectile _projectile = Instantiate(Projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
                CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
                if(targetCollider == null)
                {
                    _projectile.transform.LookAt(target.transform.position);
                }
                else
                {   
                    _projectile.transform.LookAt(target.transform.position + Vector3.up * targetCollider.height / 2);
                }
                
                _projectile.SetTarget(target, totalDamage, instigator);
            }
        }
        public void HandleMeleeEnter(GameObject instigator, float totalDamage, GameObject currentWeapon)
        {
            if (!currentWeapon.TryGetComponent(out CapsuleCollider collider))
            {
                Debug.LogError("Weapon collider was null for "+currentWeapon.name);
                return;
            }
            MeleeHandler meleeHandler2 = collider.GetComponent<MeleeHandler>();
            meleeHandler2.SetInstigator(instigator);
            meleeHandler2.SetDamage(totalDamage);
            meleeHandler2.OnSwing();
            // collider.enabled = true;
        }
        public float GetDamage()
        {
            return Damage;
        }
        public float GetPercentage()
        {
            return PercentageBonus;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return PercentageBonus;
            }
        }
        // public void SetAdditionalDamage(float newDamage)
        // {
        //     additionalDamage = newDamage;
        // }
    }
}

