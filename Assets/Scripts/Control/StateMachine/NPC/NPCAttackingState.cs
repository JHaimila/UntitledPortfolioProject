using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Control.NPCController
{
    public class NPCAttackingState : NPCBaseState
    {
        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeInFixedTime = 0.1f;

        private GameObject _target;
        float previousFrameTime;

        public NPCAttackingState(NPCStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Agent.isStopped = true;
            // stateMachine.LastAttack = DateTime.Now;
            stateMachine.Animator.CrossFadeInFixedTime(Animator.StringToHash(stateMachine.WeaponHandler.currentWeapon.AnimationString), CrossFadeInFixedTime);
            _target = stateMachine.Target;
        }
        public override void Tick(float deltaTime)
        {
            stateMachine.transform.LookAt(stateMachine.Target.transform, Vector3.up);
            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
            if(normalizedTime > 0.8f)
            {
                stateMachine.ChangeState();
            }
            previousFrameTime = normalizedTime;
        }
        public override void Exit()
        {
            
        }

        
    }
}

