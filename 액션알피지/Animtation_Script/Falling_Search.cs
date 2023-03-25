using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling_Search : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (!PAC.manager.IsGround && animator.GetFloat("Flight_Timer") > 0.1f)
        {



            animator.CrossFade("Falling", 0f);

            return;
        }
        if (!PAC.manager.IsGround)
        {
            animator.SetFloat("Flight_Timer", animator.GetFloat("Flight_Timer") + Time.deltaTime);
            //Debug.Log("¿€µø");
        }
        
        



    }
}
