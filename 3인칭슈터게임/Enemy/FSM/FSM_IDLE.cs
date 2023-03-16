using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_IDLE: StateMachineBehaviour
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

        float Dis = _Manager._Move.Distance();
        if (_Manager.Check_Attack() && Dis <= _Manager.Attack_Range)
        {
            animator.SetTrigger("Attack");
            _Manager.Current_Delay = 0;
            return;
        }
        


        if (_Manager.Search_Target())
        {
            animator.SetBool("Follow",true);
            animator.SetBool("Patrol", false);
        }
        else
        {
            animator.SetBool("Follow", false);
            animator.SetBool("Patrol", true);
        }
    }
}
