using UnityEngine;

namespace RPG.Control.EnemyController
{
    public class EnemyPatrollingState : EnemyBaseState
    {
        private readonly int PatrolHash = Animator.StringToHash("1H_Walk_Forward");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;
        private Waypoint _target;
        public EnemyPatrollingState(EnemyStateMachine stateMachine, Waypoint target) : base(stateMachine)
        {
            _target = target;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            stateMachine.Animator.CrossFadeInFixedTime(PatrolHash, CrossFadeInFixedTime);
            stateMachine.Agent.destination = _target.transform.position;
            
        }
        public override void Tick(float deltaTime)
        {
            if(Vector3.Distance(_target.transform.position, stateMachine.transform.position) < 1)
            {
                if(_target.stay)
                {
                    if(_target.guard)
                    {
                        // Set Guard State
                    }
                    else
                    {
                        stateMachine.SwitchState(new EnemyIdlingState(stateMachine));
                    }
                }
                else
                {
                    stateMachine.PatrolPath.SwitchWaypoint();
                    if(_target.guard)
                    {
                        // Set Guard State
                    }
                    else
                    {
                        stateMachine.SwitchState(new EnemyIdlingState(stateMachine, _target.waitSecs));
                    }
                }
                
                return;
            }
        }
        public override void Exit()
        {
            
        }

        
    }
}

