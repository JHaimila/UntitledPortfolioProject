using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using InventorySystem.Inventories;
using Saving.Saving;
using Unity.VisualScripting;
using UnityEngine.Serialization;

namespace RPG.Combat
{
    public class WeaponHandler : MonoBehaviour
    {
        [field:SerializeField] public Weapon defaultWeapon {get; private set;} = null;
        [field:SerializeField] public Weapon currentWeapon {get; private set;} = null;
        [field:SerializeField] public Transform rightHandTransform {get; private set;} = null;
        [field:SerializeField] public Transform leftHandTransform {get; private set;} = null;
        [field:SerializeField] public Animator Animator {get; private set;} = null;
        [field:SerializeField] public Health Target {get; private set;} = null;
        // private void OnEnable() {
        //     GetComponent<BaseStats>().LevelUpEvent += UpdateAdditionalDamage;
        // }
        // private void OnDisable() {
        //     GetComponent<BaseStats>().LevelUpEvent -= UpdateAdditionalDamage;
        // }
        private void Start() {
            // currentWeapon = null;
            if (tag.Equals("Player"))
            {
                Equipment equipment = GameObject.FindWithTag("Player").GetComponent<Equipment>();
                currentWeapon = equipment.GetItemInSlot(EquipLocation.Weapon) as Weapon;
            }
            EquipWeapon(currentWeapon);
        }
        public void EquipWeapon(Weapon weapon)
        {
            if(weapon == null)
            {
                currentWeapon = defaultWeapon;
            }
            else
            {
                currentWeapon = weapon;
            }
            currentWeapon.Spawn(rightHandTransform, leftHandTransform, Animator, GetComponent<BaseStats>().GetStat(Stat.Damage), tag);
        }
        public void SetTarget(Health target)
        {
            this.Target = target;
        }
        
        public void ToggleMeleeAttackEnter()
        {
            if (!currentWeapon){return;}

            GameObject currentWeaponObj;
            if (rightHandTransform.childCount > 0)
            {
                currentWeaponObj = rightHandTransform.GetChild(0).gameObject;
            }
            else
            {
                currentWeaponObj = leftHandTransform.GetChild(0).gameObject;
            }
            currentWeapon.HandleMeleeEnter(gameObject,GetComponent<BaseStats>().GetStat(Stat.Damage), currentWeaponObj);
        }
        public void ToggleMeleeAttackExit()
        {
            if (!currentWeapon){return;}
            GameObject currentWeaponObj;
            if (rightHandTransform.childCount > 0)
            {
                currentWeaponObj = rightHandTransform.GetChild(0).gameObject;
            }
            else
            {
                currentWeaponObj = leftHandTransform.GetChild(0).gameObject;
            }

            if(currentWeaponObj.TryGetComponent(out CapsuleCollider weaponCollider))
            {
                weaponCollider.enabled = false;
            }
        }
        public void LaunchProjectile()
        {
            if (!currentWeapon){return;}
            currentWeapon.LaunchProjectile(rightHandTransform,leftHandTransform, Target, gameObject, GetComponent<BaseStats>().GetStat(Stat.Damage));
        }
    }
}

