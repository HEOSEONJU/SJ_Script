using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gravity_Control : StateMachineBehaviour
{

    [SerializeField]
    bool state;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<Player_Animaotr_Controller>()._Move.Control_Gravity(state);
        





    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.GetComponent<Player_Animaotr_Controller>()._Move.Control_Gravity(true);
    }
}