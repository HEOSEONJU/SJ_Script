using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Search : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent<Player_Animaotr_Controller>(out PAC);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (!PAC.manager._isGround && animator.GetFloat("Flight_Timer") > 0.1f)
        {



            animator.CrossFade("Falling", 0f);

            return;
        }
        if (!PAC.manager._isGround)
        {
            animator.SetFloat("Flight_Timer", animator.GetFloat("Flight_Timer") + Time.deltaTime);
            //Debug.Log("¿€µø");
        }
        
        



    }
}
