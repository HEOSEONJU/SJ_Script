using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Attack : StateMachineBehaviour
{
    
    Enemy_Manager _Manager;

    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _Manager = animator.GetComponent<Enemy_Manager>();

    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_Manager._HP.Live)
        {
            return;
        }
        _Manager._Move.Stop_Enemy();


    }

}
