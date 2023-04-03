using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Combo : StateMachineBehaviour
{


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("Combo_Stack", 0);



    }
}