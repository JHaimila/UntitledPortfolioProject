using RPG.Control.Routine;
using UnityEngine;
using System.Collections;

namespace RPG.Control.NPCController
{
    public class NPCRoutineState : NPCBaseState
    {
        private readonly int WalkHash = Animator.StringToHash("1H_Walk_Forward");
        private readonly int RoutinePerforming = Animator.StringToHash("RoutineEnter");
        private readonly int RoutineEnter = Animator.StringToHash("RoutineEnter");
        private readonly int RoutineExit = Animator.StringToHash("RoutineExit");
        private readonly int IdleHash = Animator.StringToHash("Idle2");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;
        private RoutineNode node;
        public NPCRoutineState(NPCStateMachine stateMachine, RoutineNode node) : base(stateMachine)
        {
            this.node = node;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            

            stateMachine.Animator.CrossFadeInFixedTime(WalkHash, CrossFadeInFixedTime);
            stateMachine.Agent.destination = node.GetTransform().position;

            if(Vector3.Distance(node.GetTransform().position, stateMachine.transform.position) < 1)
            {
                stateMachine.BeginCorotine(PerformRoutine(node.GetWaitTime()));
            }
            else
            {
                stateMachine.SwitchState(new NPCTravelState(stateMachine, node.transform.position));
            }
            
        }
        public override void Tick(float deltaTime){}
        public override void Exit()
        {
            stateMachine.Agent.updateRotation = true;
            stateMachine.EndAllCorotines();
        }
        private IEnumerator PerformRoutine(float waitSeconds)
        {
            stateMachine.Agent.updateRotation = false;
            stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
            yield return new WaitForSeconds(1);

            stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
            SetYRotation(stateMachine.Agent, node.GetTransform(), Time.deltaTime, stateMachine.RotationSpeed);
            yield return new WaitForSeconds(0.5f);

            stateMachine.Animator.CrossFadeInFixedTime(RoutineEnter, CrossFadeInFixedTime);
            yield return new WaitForSeconds(1f);

            RuntimeAnimatorController originalController = stateMachine.Animator.runtimeAnimatorController;
            if(node.GetAnimation() != null)
            {
                stateMachine.Animator.runtimeAnimatorController = node.GetAnimation();
            }
            
            if(node.HasItem())
            {
                stateMachine.WeaponHandler.EquipWeapon(node.GetItem());
            }
            node.Interact();

            stateMachine.Animator.CrossFadeInFixedTime(RoutinePerforming, CrossFadeInFixedTime);
            yield return new WaitForSeconds(waitSeconds);
            
            stateMachine.Animator.CrossFadeInFixedTime(RoutineExit, CrossFadeInFixedTime);
            yield return new WaitForSeconds(0.5f);

            if(!stateMachine.RoutineHandler.GetRoutine().HasNext())
            {
                stateMachine.SwitchState(new NPCIdlingState(stateMachine));
            }
            else
            {
                stateMachine.RoutineHandler.GetRoutine().NextNode();
                stateMachine.SwitchState(new NPCIdlingState(stateMachine, node.GetWaitTime()));
            }
            stateMachine.Animator.runtimeAnimatorController = originalController;
            stateMachine.Agent.updateRotation = true;
        }

        
    }
}

