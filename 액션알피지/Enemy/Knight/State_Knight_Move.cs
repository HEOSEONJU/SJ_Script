using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Knight_Move : StateMachineBehaviour
{
    Knight_Enemy KN;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        KN=animator.GetComponent<Knight_Enemy>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if(!animator.GetCurrentAnimatorStateInfo(1).IsTag("None") || !animator.GetCurrentAnimatorStateInfo(2).IsTag("None"))
        {
            return;
        }
        
        if (KN._Knight_AI.Move_Target(animator))
        {
            animator.SetBool("Target", false);
        }
    }
}
