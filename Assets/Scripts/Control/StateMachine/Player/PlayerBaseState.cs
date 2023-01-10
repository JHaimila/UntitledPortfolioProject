using UnityEngine;

namespace RPG.Control.PlayerController
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;

        

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public void MoveTowardTarget(Vector3 point)
        {
            stateMachine.Agent.destination = point;
        }
    }
}