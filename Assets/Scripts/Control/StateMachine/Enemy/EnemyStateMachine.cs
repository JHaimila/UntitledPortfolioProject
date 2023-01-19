using UnityEngine;
using UnityEngine.AI;
using Saving.Saving;
using RPG.Combat;
using RPG.Attributes;
using System.Collections.Generic;

namespace RPG.Control.EnemyController
{
    public class EnemyStateMachine : StateMachine
    {
        //Components
        [field:SerializeField] public NavMeshAgent Agent {get; private set;}
        [field:SerializeField] public Animator Animator {get; private set;}
        [field:SerializeField] public Health Health {get; private set;}
        
        //Ranges
        // [field:SerializeField] public float SightRange {get; private set;}

        // Movement Variables
        [field:SerializeField] public float WalkSpeed {get; private set;} = 3.5f;
        [field:SerializeField] public float RunSpeed {get; private set;} = 4.5f;

        // Searching Variables
        [field:SerializeField] public int PointsLookedAt {get; private set;} = 0;
        [field:SerializeField] public Sight Sight {get; private set;}
        [field:SerializeField] public bool TargetInSight {get; private set;}

        [field:SerializeField] public GameObject Target {get; private set;}
        [field:SerializeField] public PatrolPath PatrolPath {get; private set;} = null;
        [field:SerializeField] public StateHandler StateChecker {get; private set;}
        [field:SerializeField] public BehaviourState DefaultBehaviour {get; private set;}

        // [field:SerializeField] public Weapon weapon {get; private set;} = null;
        // [field:SerializeField] public Transform handTransform {get; private set;} = null;
        [field:SerializeField] public WeaponHandler WeaponHandler {get; private set;} = null;
        [field:SerializeField] public List<StateBehaviourMap> BehaviourMaps {get; private set;} = new List<StateBehaviourMap>();

        private int guarPointIndex = 0;
        [SerializeField] private float aggroRadius = 8f;
        [SerializeField] LayerMask agroLayer;
        public bool isChasing;
        private void OnEnable() 
        {
            StateChecker.OnBehaviourChange += ChangeState;
        }
        private void Start() {
            Target = GameObject.FindGameObjectWithTag("Player");
            Sight.SetTarget(Target);
            StateChecker.SetCurrentBehaviour(DefaultBehaviour);
            if(Health.GetHealth() > 0)
            {
                ChangeState();
            }
        }
        private void OnDisable() 
        {
            StateChecker.OnBehaviourChange -= ChangeState;
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
            StateChecker.Check(RPG.Control.Action.Attacked);
        }
        public void AggroNearByEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, aggroRadius, Vector3.up, 0, agroLayer);
            foreach(RaycastHit enemyHit in hits)
            {
                EnemyStateMachine nearByEnemy = enemyHit.transform.GetComponent<EnemyStateMachine>();
                if(nearByEnemy == null){continue;}
                if(nearByEnemy.Health.isDead){continue;}

                nearByEnemy.TriggerAggro();
            }
        }
        public void HandleSeesPlayer()
        {
            if(Sight.seesPlayer)
            {
                if(Target.GetComponent<Health>().enabled != false)
                {
                    WeaponHandler.SetTarget(Target.GetComponent<Health>());
                    SwitchState(new EnemyChasingState(this));
                    AggroNearByEnemies();
                }
                else
                {
                    SwitchState(new EnemyPatrollingState(this, PatrolPath.GetCurrentWaypoint()));
                }
            }
            else
            {
                
                SwitchState(new EnemySearchingState(this));
            }
        }
        public void ChangeState()
        {
            switch(StateChecker.GetCurrentBehaviour())
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
                    SwitchState(new EnemyChasingState(this));
                    break;
                }
                case BehaviourState.Searching:
                {
                    SwitchState(new EnemySearchingState(this));
                    break;
                }
                case BehaviourState.Dead:
                {
                    SwitchState(new EnemyDeathState(this));
                    break;
                }
            }
        }
        
        public bool PlayerWithinRange(float range)
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
            if(PatrolPath != null)
            {
                SwitchState(new EnemyPatrollingState(this, PatrolPath.GetCurrentWaypoint()));
            }
            else
            {
                SwitchState(new EnemyIdlingState(this));
            }
        }
        #if UNITY_EDITOR
        // private void OnDrawGizmos() {
        //     Gizmos.DrawWireSphere(transform.position, WeaponHandler.currentWeapon.Range);
        // }
        #endif
    }
}

