using UnityEngine;
using UnityEngine.AI;
using Saving.Saving;
using RPG.Combat;
using RPG.Attributes;
using System.Collections.Generic;
using RPG.Control.Routine;
using System.Collections;

namespace RPG.Control.NPCController
{
    public class NPCStateMachine : StateMachine
    {
        //Components
        [field:SerializeField] public NavMeshAgent Agent {get; private set;}
        
        [field:SerializeField, Header("Senses")] public Health Health {get; private set;}
        [field:SerializeField] public Sight Sight {get; private set;}

        [field:SerializeField, Header("Animation")] public Animator Animator {get; private set;}
        
        //Ranges
        // [field:SerializeField] public float SightRange {get; private set;}

        // Movement Variables
        [field:SerializeField] public float WalkSpeed {get; private set;} = 3.5f;
        [field:SerializeField] public float RunSpeed {get; private set;} = 4.5f;

        // Searching Variables
        [Header("Searching")]public int searchedCount = 0;
        private bool isSearching = false;
        [field:SerializeField] public int MaxSearchCount = 3;
        
        [field:SerializeField] public float RotationSpeed {get; private set;} = 3;
        [field:SerializeField] public GameObject Target {get; private set;}
        [field:SerializeField] public RoutineHandler RoutineHandler {get; private set;} = null;
        [field:SerializeField] public StateHandler StateHandler {get; private set;}
        [field:SerializeField] public BehaviourState DefaultBehaviour {get; private set;}
        [field:SerializeField] public WeaponHandler WeaponHandler {get; private set;} = null;
        [field:SerializeField] public List<StateBehaviourMap> BehaviourMaps {get; private set;} = new List<StateBehaviourMap>();
        [SerializeField] private float aggroRadius = 8f;
        [SerializeField] LayerMask agroLayer;
        public bool isChasing;
        private void OnEnable() 
        {
            StateHandler.OnBehaviourChange += ChangeState;
        }
        private void Start() {
            Target = GameObject.FindGameObjectWithTag("Player");
            if(Sight != null)
            {
                Sight.SetTarget(Target);
            }
            
            StateHandler.SetCurrentBehaviour(DefaultBehaviour);
            if(Health != null)
            {
                if(Health.GetHealth() > 0)
                {
                    ChangeState();
                }
                Health.DeathEvent += HandleDeath;
            }
            
        }
        private void OnDisable() 
        {
            StateHandler.OnBehaviourChange -= ChangeState;
            Health.DeathEvent -= HandleDeath;
        }

        private void HandleRevive()
        {
            Health.enabled = true;
            Agent.enabled = true;
            GetComponent<CapsuleCollider>().enabled = true;
            Sight.enabled = true;
        }
        public void TriggerAggro()
        {
            WeaponHandler.SetTarget(Target.GetComponent<Health>());
            StateHandler.Check(RPG.Control.Action.Attacked);
        }
        private void HandleDeath()
        {
            SwitchState(new NPCDeathState(this));
        }
        public void AggroNearByEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, aggroRadius, Vector3.up, 0, agroLayer);
            foreach(RaycastHit enemyHit in hits)
            {
                if (enemyHit.transform.TryGetComponent<StateHandler>(out StateHandler nearByEnemy))
                {
                    nearByEnemy.Check(Action.Attacked);
                }
            }
        }
        public void ChangeState()
        {
            switch(StateHandler.GetCurrentBehaviour())
            {
                case BehaviourState.Neutral:
                {
                    SetNeutralState();
                    break;
                }
                case BehaviourState.Fighting:
                {
                    AggroNearByEnemies();
                    WeaponHandler.SetTarget(Target.GetComponent<Health>());
                    SwitchState(new NPCChasingState(this));
                    break;
                }
                case BehaviourState.Searching:
                {
                    isSearching = true;
                    SwitchState(new NPCSearchingState(this));
                    break;
                }
                case BehaviourState.Fleeing:
                {
                    if(Vector3.Distance(Target.transform.position, transform.position) > 10)
                    {
                        StateHandler.Check(Action.LostTarget);
                        break;
                    }
                    SwitchState(new NPCFleeingState(this));
                    break;
                }
                case BehaviourState.Idle:
                {
                    SwitchState(new NPCIdlingState(this));
                    break;
                }
            }
            if(StateHandler.GetCurrentBehaviour() != BehaviourState.Searching && isSearching)
            {
                isSearching = false;
                searchedCount = 0;
            }
        }
        
        public bool TargetWithinRange(float range)
        {
            if(Target.TryGetComponent<Health>(out Health targetHealth))
            {
                if(!targetHealth.enabled)
                {
                    return false;
                }
            }
            if(Vector3.Distance(Target.transform.position, transform.position) < range)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public override void RestoreState(object state)
        {
            SerializableVector3 savedPos = (SerializableVector3)state;
            transform.position = savedPos.ToVector();
            // Agent.Warp(transform.position);
        }

        public void SetNeutralState()
        {
            if(RoutineHandler.GetRoutine() != null)
            {
                SwitchState(new NPCRoutineState(this, RoutineHandler.GetRoutine().GetCurrentNode()));
            }
            else
            {
                SwitchState(new NPCIdlingState(this));
            }
        }
    }
}

