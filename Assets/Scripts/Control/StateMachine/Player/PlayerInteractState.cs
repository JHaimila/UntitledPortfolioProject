using RPG.Core;
using UnityEngine;

namespace RPG.Control.PlayerController
{
    public class PlayerInteractState : PlayerBaseState
    {
        RaycastHit interactObj;
        public PlayerInteractState(PlayerStateMachine stateMachine, RaycastHit interact) : base(stateMachine)
        {
            this.interactObj = interact;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = true;
            interactObj.transform.GetComponent<IInteractable>().OnInteract();
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