using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cal_Boar_Attack_Delay : StateMachineBehaviour
{

    Boar_Enemy KN;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        KN = animator.GetComponent<Boar_Enemy>();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("None") || !animator.GetCurrentAnimatorStateInfo(2).IsTag("None"))
        {
            return;
        }
        KN._Boar_AI.Update_Attack_Delay(Time.deltaTime);

    }

}