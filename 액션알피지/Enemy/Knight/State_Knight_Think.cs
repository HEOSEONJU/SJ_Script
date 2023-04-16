using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Knight_Think : StateMachineBehaviour
{
    Knight_Enemy KN;
    [SerializeField]
    float Max_Time = 2.0f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.TryGetComponent<Knight_Enemy>(out Knight_Enemy E))
            KN = E;
        KN._Knight_AI.Move_OFF();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("None") || !animator.GetCurrentAnimatorStateInfo(2).IsTag("None"))
        {
            return;
        }
        animator.SetFloat("ThinkTime",(animator.GetFloat("ThinkTime")+ Time.deltaTime));
            
        if (KN._Knight_AI.Attack_Think()|| animator.GetFloat("ThinkTime")> Max_Time)
        {
            animator.SetTrigger("Attack_Function");
            animator.SetFloat("ThinkTime", 0);
        }

    }
}
