using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Boar_Move : StateMachineBehaviour
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

        if (KN._boar_AI.Move_Target(animator))
        {
            animator.SetBool("Target", false);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        KN._RD.velocity = Vector3.zero;
    }
}