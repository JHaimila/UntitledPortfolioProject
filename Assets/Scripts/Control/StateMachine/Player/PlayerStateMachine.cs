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
            // Health.ReviveEvent += HandleRevive;
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
        

        private void Start() 
        {
            MainCamera = Camera.main;
        }

        

        private void OnDisable() 
        {
            InteractionHandler.MoveEvent -= HandleMove;
            InteractionHandler.AttackEvent -= HandleAttack;
            InteractionHandler.InteractEvent -= HandleInteraction;
            Health.DeathEvent -= HandleDeath;
            // Health.ReviveEvent -= HandleRevive;
        }

        private void HandleRevive()
        {
            Health.enabled = true;
            enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            SwitchState(new PlayerIdlingState(this));
        }

        public void HandleMove(Transform target, float range)
        {
            WeaponHandler.ToggleMeleeAttackExit();
            if(!isInMovingState)
            {
                SwitchState(new PlayerMovingState(this, target, range));
            }
        }
        public void HandleMove(Vector3 position)
        {
            WeaponHandler.ToggleMeleeAttackExit();
            if(!isInMovingState)
            {
                SwitchState(new PlayerMovingState(this, position));
            }
        }
        public void AtDestination(Transform target)
        {
            if(target.TryGetComponent<IInteractable>(out IInteractable interact))
            {
                HandleInteraction(target);
                return;
            }
            if(target.TryGetComponent<IAttackable>(out IAttackable attackable))
            {
                HandleAttack(target);
                return;
            }
            SwitchState(new PlayerIdlingState(this));
        }
        public void HandleInteraction(Transform target)
        {
            if(!target.TryGetComponent<IInteractable>(out IInteractable interact)){return;}
            WeaponHandler.ToggleMeleeAttackExit();
            if(Vector3.Distance(transform.position, target.position) > interact.GetInteractRange())
            {
                HandleMove(target, target.GetComponent<IInteractable>().GetInteractRange());
            }
            else
            {
                SwitchState(new PlayerInteractState(this, interact));
            }
        }
        public void HandleAttack(Transform target)
        {
            if(!target.TryGetComponent<Health>(out Health targetHealth)){return;}
            if(!targetHealth.Attackable()){return;}

            WeaponHandler.SetTarget(targetHealth);
            if(Vector3.Distance(transform.position,target.position) > WeaponHandler.currentWeapon.Range)
            {
                HandleMove(target, WeaponHandler.currentWeapon.Range);
            }
            else if((DateTime.Now - LastAttack).TotalSeconds > AttackCooldownTime)
            {
                SwitchState(new PlayerAttackingState(this, target));
            }
        }
        public void FreezePlayer()
        {
            
            // SwitchState(new PlayerIdlingState(this));
            // InteractionHandler.enabled = false;
            InteractionHandler.uiOpen = true;

            // Agent.isStopped = true;
        }
        public void UnFreezePlayer()
        {
            // InteractionHandler.enabled = true;
            InteractionHandler.uiOpen = false;
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

