using RPG.Core;
using UnityEngine;

namespace RPG.Control.PlayerController
{
    public class PlayerInteractState : PlayerBaseState
    {
        IInteractable interactable;
        public PlayerInteractState(PlayerStateMachine stateMachine, IInteractable interact) : base(stateMachine)
        {
            this.interactable = interact;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = true;
            interactable.OnInteract();
            stateMachine.SwitchState(new PlayerIdlingState(stateMachine));
        }
        public override void Tick(float deltaTime)
        {
            
        }
        public override void Exit()
        {
            
        }

        
    }
}