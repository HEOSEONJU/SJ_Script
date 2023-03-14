using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Patrol : StateMachineBehaviour
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
        if (_Manager.Search_Target())
        {
            animator.SetBool("Follow", true);
            animator.SetBool("Patrol", false);
            return;
        }
        _Manager._Move.Move_Target();
    }
}
