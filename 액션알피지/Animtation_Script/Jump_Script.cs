using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Jump_Script : StateMachineBehaviour
{

    Player_Animaotr_Controller PAC;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
        PAC._Move.Jumping();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PAC._Move.Jumping_Move();




    }
}
    

   
