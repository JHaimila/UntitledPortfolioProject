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
        RaycastHit hit;
        float range = 0;


        public PlayerMovingState(PlayerStateMachine stateMachine, RaycastHit hit, float range) : base(stateMachine)
        {
            this.hit = hit;
            this.range = range;
        }
        public PlayerMovingState(PlayerStateMachine stateMachine, RaycastHit hit) : base(stateMachine)
        {
            this.hit = hit;
        }
        public PlayerMovingState(PlayerStateMachine stateMachine, Vector3 position) : base(stateMachine)
        {
            this.destination = position;
        }
        public PlayerMovingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
            
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            stateMachine.InteractionHandler.MoveEvent += NewLocation;
            stateMachine.Animator.CrossFadeInFixedTime(WalkForwardHash, CrossFadeInFixedTime);
            NewLocation(hit);
            stateMachine.isInMovingState = true;
        }
        public void Enter(RaycastHit hit)
        {
            
            this.hit = hit;
        }
        public void Enter(RaycastHit hit, float range)
        {
            this.hit = hit;
            this.range = range;
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
                    if(hit.transform.TryGetComponent<IAttackable>(out IAttackable target))
                    {
                        stateMachine.HandleAttack(hit);
                    }
                    else if(hit.transform.TryGetComponent<IInteractable>(out IInteractable interact))
                    {
                        stateMachine.HandleInteraction(hit);
                    }
                    else
                    {
                        stateMachine.SwitchState(new PlayerIdlingState(stateMachine));
                    }
                } 
            }
              
            
        }
        public override void Exit()
        {
            stateMachine.InteractionHandler.MoveEvent -= NewLocation;
            stateMachine.isInMovingState = false;
        }
        
        
        private void NewLocation(RaycastHit hit)
        {
            this.hit = hit;
            stateMachine.Agent.SetDestination(hit.point);
        }
        private void NewLocation(Vector3 position)
        {
            this.destination = position;
            stateMachine.Agent.SetDestination(position);
        }
    }
}

