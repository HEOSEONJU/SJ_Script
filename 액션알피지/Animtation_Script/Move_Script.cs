using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Script : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("IsInteracting")|| !PAC.manager.IsGround)
        {
            return;
        }
        PAC._Move.CharRotate(Time.deltaTime);
        PAC._Move.GroundMove();
        
        //_Move.Speed_Control();
    }

    
}
