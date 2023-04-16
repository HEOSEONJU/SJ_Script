using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Boar_Search : StateMachineBehaviour
{
    Boar_Enemy KN;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent<Boar_Enemy>(out Boar_Enemy E))
            KN = E;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("None") || !animator.GetCurrentAnimatorStateInfo(2).IsTag("None"))
        {
            return;
        }
        if (KN._boar_AI.Detection())
        {
            animator.SetBool("Target", true);
        }

    }
}