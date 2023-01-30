using RPG.Control.Routine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPG.Control.NPCController
{
    public class NPCRoutineState : NPCBaseState
    {
        private readonly int WalkHash = Animator.StringToHash("1H_Walk_Forward");
        private readonly int RoutineHash = Animator.StringToHash("Routine");
        private readonly int IdleHash = Animator.StringToHash("Idle2");

        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;
        private RoutineNode node;
        private List<RoutineNodeAnimation> animations;
        public NPCRoutineState(NPCStateMachine stateMachine, RoutineNode node) : base(stateMachine)
        {
            this.node = node;
        }

        public override void Enter()
        {
            stateMachine.Agent.isStopped = false;
            stateMachine.Animator.CrossFadeInFixedTime(WalkHash, CrossFadeInFixedTime);
            stateMachine.Agent.destination = node.GetTransform().position;
            if (node.HasAnimations())
            {
                animations = node.GetAnimations();
            }
            if(Vector3.Distance(node.GetTransform().position, stateMachine.transform.position) < 1)
            {
                stateMachine.BeginCorotine(PerformRoutine());
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
        private IEnumerator PerformRoutine()
        {
            stateMachine.Agent.updateRotation = false;
            stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeInFixedTime);
            
            yield return new WaitForSeconds(0.5f);
            SetYRotation(stateMachine.Agent, node.GetTransform(), Time.deltaTime, stateMachine.RotationSpeed);
            
            yield return new WaitForSeconds(0.5f);
           
            RuntimeAnimatorController originalController = stateMachine.Animator.runtimeAnimatorController;
            node.Interact();

            if (node.HasAnimations())
            {
                foreach (RoutineNodeAnimation animation in animations)
                {
                    if (animation.interactItem != null)
                    {
                        stateMachine.WeaponHandler.EquipWeapon(animation.interactItem);
                    }

                    if (animation.nodeAnimation != null)
                    {
                        stateMachine.Animator.runtimeAnimatorController = animation.nodeAnimation;
                        stateMachine.Animator.CrossFadeInFixedTime(RoutineHash, CrossFadeInFixedTime);
                    }
                    
                    animation.triggers?.Invoke();
                    yield return new WaitForSeconds(animation.waitSeconds);
                }
            }

            if(!stateMachine.RoutineHandler.HasNext())
            {
                stateMachine.SwitchState(new NPCIdlingState(stateMachine));
            }
            else
            {
                stateMachine.RoutineHandler.NextNode();
                stateMachine.SwitchState(new NPCIdlingState(stateMachine, 1));
            }
            stateMachine.Animator.runtimeAnimatorController = originalController;
            stateMachine.Agent.updateRotation = true;
        }

        
    }
}

