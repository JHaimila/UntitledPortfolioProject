using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void Exit();

        // Gets the normalized time left in an animation. Handles inbetween animations states
        protected float NormalizedAnimationTime(Animator animator, string tag)
        {
            //Unity animator can have you inbetween states. So you need to check the current one and the next one as well. 
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

            if(animator.IsInTransition(0) && nextInfo.IsTag(tag))
            {
                return nextInfo.normalizedTime;
            }
            else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            {
                return currentInfo.normalizedTime;
            }
            else
            {
                return 0;
            }
        }
        public void RotateTowards (NavMeshAgent agent, Transform target, float deltaTime, float rotationSpeed) 
        {
            agent.isStopped = false;
            Vector3 direction = (target.position - agent.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, deltaTime * rotationSpeed);
        }
        public void SetYRotation(NavMeshAgent agent, Transform target, float deltaTime, float rotationSpeed)
        {
            agent.isStopped = false;
            agent.transform.rotation = Quaternion.Euler(agent.transform.localEulerAngles.x, target.localEulerAngles.y, agent.transform.localEulerAngles.z);
        }
    }
}