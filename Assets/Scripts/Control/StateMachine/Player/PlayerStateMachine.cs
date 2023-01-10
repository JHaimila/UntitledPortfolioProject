using System;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using Saving.Saving;
using RPG.Combat;
using RPG.Attributes;
using InventorySystem.Inventories;

namespace RPG.Control.PlayerController
{
    public class PlayerStateMachine : StateMachine
    {
        [field:SerializeField] public Camera MainCamera {get; private set;}
        [field:SerializeField] public NavMeshAgent Agent {get; private set;}
        [field:SerializeField] public Animator Animator {get; private set;}
        [field:SerializeField] public InteractionHandler InteractionHandler {get; private set;}
        // [field:SerializeField] public InputReader InputReader {get; private set;}
        [field:SerializeField] public float InteractionRange {get; private set;}
        [field:SerializeField] public float RotationSpeed {get; private set;}
        [field:SerializeField] public DateTime LastAttack;
        [field:SerializeField] public float AttackCooldownTime {get; private set;}
        [field:SerializeField] public Health Health {get; private set;}
        protected Equipment equipment;

        [field:SerializeField] public WeaponHandler WeaponHandler {get; private set;} = null;

        public RaycastHit currentTarget;

        // private PlayerIdlingState PlayerIdleState;
        // private PlayerAttackingState PlayerAttackingState;
        // private PlayerMovingState PlayerMovingState;


        public bool isInMovingState = false;
        private void OnEnable() 
        {
            InteractionHandler.MoveEvent += HandleMove;
            InteractionHandler.AttackEvent += HandleAttack;
            InteractionHandler.InteractEvent += HandleInteraction;
            Health.DeathEvent += HandleDeath;
            Health.ReviveEvent += HandleRevive;
            equipment = GetComponent<Equipment>();
            if(equipment)
            {
                equipment.equipmentUpdated += UpdateWeapon;
            }
        }
        private void UpdateWeapon()
        {
            Weapon equipWeapon = equipment.GetItemInSlot(EquipLocation.Weapon) as Weapon;
            if(equipWeapon == null)
            {
                WeaponHandler.EquipWeapon(WeaponHandler.defaultWeapon);
                return;
            }
            WeaponHandler.EquipWeapon(equipWeapon);
        }
        public void HandleInteraction(RaycastHit obj)
        {
            if(Vector3.Distance(transform.position, obj.transform.position) <= obj.transform.GetComponent<IInteractable>().GetInteractRange())
            {
                SwitchState(new PlayerInteractState(this, obj));
            }
            else
            {
                HandleMove(obj, obj.transform.GetComponent<IInteractable>().GetInteractRange());
            }
        }

        private void Start() 
        {
            MainCamera = Camera.main;
            // PlayerIdleState = new PlayerIdlingState(this);
            // PlayerMovingState = new PlayerMovingState(this);
            // PlayerAttackingState = new PlayerAttackingState(this);
        }

        

        private void OnDisable() 
        {
            InteractionHandler.MoveEvent -= HandleMove;
            InteractionHandler.AttackEvent -= HandleAttack;
            InteractionHandler.InteractEvent -= HandleInteraction;
            Health.DeathEvent -= HandleDeath;
            Health.ReviveEvent -= HandleRevive;
        }

        private void HandleRevive()
        {
            Health.enabled = true;
            enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            SwitchState(new PlayerIdlingState(this));
        }

        private void HandleMove(RaycastHit hit)
        {
            if(!isInMovingState)
            {
                SwitchState(new PlayerMovingState(this, hit));
            }
        }
        public void HandleMove(RaycastHit hit, float range)
        {
            if(!isInMovingState)
            {
                SwitchState(new PlayerMovingState(this, hit, range));
            }
        }
        public void HandleMove(Vector3 position)
        {
            if(!isInMovingState)
            {
                SwitchState(new PlayerMovingState(this, position));
            }
        }
        public void HandleAttack(RaycastHit hit)
        {
            currentTarget = hit;
            WeaponHandler.SetTarget(currentTarget.transform.GetComponent<Health>());
            if(Vector3.Distance(transform.position, hit.transform.position) > WeaponHandler.currentWeapon.Range)
            {
                HandleMove(hit, WeaponHandler.currentWeapon.Range);
            }
            else if((DateTime.Now - LastAttack).TotalSeconds > AttackCooldownTime)
            {
                SwitchState(new PlayerAttackingState(this, hit));
            }
        }
        public void FreezePlayer()
        {
            
            // SwitchState(new PlayerIdlingState(this));
            Debug.Log("Freeze Player");
            // InteractionHandler.enabled = false;
            InteractionHandler.uiOpen = true;

            // Agent.isStopped = true;
        }
        public void UnFreezePlayer()
        {
            // InteractionHandler.enabled = true;
            InteractionHandler.uiOpen = false;
            Debug.Log("Unfreeze Player");
            // Agent.isStopped = false;
            Agent.destination = transform.position;
        }
        private void HandleDeath()
        {
            SwitchState(new PlayerDeathState(this));
        }
        
        public override object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public override void RestoreState(object state)
        {
            SerializableVector3 savedPos = (SerializableVector3)state;
            transform.position = savedPos.ToVector();
            Agent.Warp(transform.position);
        }

        

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected() {
            Gizmos.DrawWireSphere(transform.position, InteractionRange);
        }
        #endif
    }
}

