using RPG.Core;
using UnityEngine;

namespace RPG.Control.PlayerController
{
    public class PlayerMovingState : PlayerBaseState
    {
        private readonly int WalkForwardHash = Animator.StringToHash("Run_Forward");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        Vector3 destination;
        Transform target;
        float range = 0;


        public PlayerMovingState(PlayerStateMachine stateMachine, Transform target, float range) : base(stateMachine)
        {
            this.target = target;
            this.destination = target.position;
            this.range = range;
        }
        public PlayerMovingState(PlayerStateMachine stateMachine, Transform target) : base(stateMachine)
        {
            this.target = target;
            this.destination = target.position;
        }
        public PlayerMovingState(PlayerStateMachine stateMachine, Vector3 desitnation) : base(stateMachine)
        {
            this.destination = desitnation;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            stateMachine.InteractionHandler.MoveEvent += NewLocation;
            stateMachine.Animator.CrossFadeInFixedTime(WalkForwardHash, CrossFadeInFixedTime);
            NewLocation(destination);
            
            stateMachine.isInMovingState = true;
        }
        public override void Tick(float deltaTime)
        {
            if(range == 0f)
            {
                if(stateMachine.transform.position == stateMachine.Agent.destination)
                {
                    stateMachine.SwitchState(new PlayerIdlingState(stateMachine));
                }
            }
            else
            {
                
                if(Vector3.Distance(stateMachine.transform.position, stateMachine.Agent.destination) <= range)
                {
                    stateMachine.AtDestination(target);
                } 
            }
        }
        public override void Exit()
        {
            stateMachine.InteractionHandler.MoveEvent -= NewLocation;
            stateMachine.isInMovingState = false;
        }
        private void NewLocation(Vector3 position)
        {
            this.destination = position;
            stateMachine.Agent.SetDestination(position);
        }
    }
}

