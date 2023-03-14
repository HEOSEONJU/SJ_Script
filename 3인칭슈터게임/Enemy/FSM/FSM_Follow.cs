using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Follow : StateMachineBehaviour
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

        if (!_Manager.Search_Target())//타겟실종시
        {
            animator.SetBool("Follow", false);
            animator.SetBool("Patrol", true);
            return;
        }


        float Dis = _Manager._Move.Distance();
        
        if (_Manager.Check_Attack() && Dis <= _Manager.Attack_Range)
        {
            animator.SetTrigger("Attack");
            _Manager.Current_Delay = 0;
            return;
        }
        if (Dis <= _Manager.Attack_Range)//공격사거리 범위 내 적있을시 정지
        {
            
            animator.SetBool("Attack_IDle", true);
            animator.SetBool("Follow", false);
            animator.SetBool("Patrol", false);
            return;
        }
        _Manager._Move.Follow_Target();
    }

}
