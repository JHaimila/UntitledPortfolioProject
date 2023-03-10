using RPG.Core;
using UnityEngine;

namespace RPG.Control.PlayerController
{
    public class PlayerMovingState : PlayerBaseState
    {
        private readonly int MoveHash = Animator.StringToHash("Run_Forward");
        
        private const float CrossFadeInFixedTime = 0.1f;

        private Vector3 destination;
        private Transform target;
        private float range = 0;


        public PlayerMovingState(PlayerStateMachine stateMachine, Transform target, float range) : base(stateMachine)
        {
            this.target = target;
            destination = target.position;
            this.range = range;
        }
        public PlayerMovingState(PlayerStateMachine stateMachine, Transform target) : base(stateMachine)
        {
            this.target = target;
            destination = target.position;
        }
        public PlayerMovingState(PlayerStateMachine stateMachine, Vector3 desitnation) : base(stateMachine)
        {
            destination = desitnation;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            stateMachine.InteractionHandler.MoveEvent += NewLocation;
            stateMachine.Animator.CrossFadeInFixedTime(MoveHash, CrossFadeInFixedTime);
            NewLocation(destination);
            
            stateMachine.isInMovingState = true;
        }
        public override void Tick(float deltaTime)
        {
            if (target != null && stateMachine.Agent.destination != target.position)
            {
                stateMachine.Agent.destination = target.position;
            }
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

