using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Only_Gravity : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
        PAC._Move.FallingStart();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //PAC.manager.Ground_Check();
        if (PAC.manager.IsGround)
        {
            //Debug.Log("ÂøÁö½ÃÀÛ");
            if(animator.GetFloat("Flight_Timer")>0.35)
                animator.CrossFade("Landing", 0f);
            
            return;
        }

        animator.SetFloat("Flight_Timer", animator.GetFloat("Flight_Timer") + Time.deltaTime);
        PAC._Move.Falling();

    }

}




