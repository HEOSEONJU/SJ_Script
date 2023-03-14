using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Attack_IDLE : StateMachineBehaviour
{
    
    Enemy_Manager _Manager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _Manager = animator.GetComponent<Enemy_Manager>();
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!_Manager._HP.Live)
        {
            return;
        }
        if (_Manager._Move.Distance() > _Manager.MAX_Dis || !_Manager.Search_Target())//타겟 사정거리 밖으로이동
        {
            animator.SetBool("Attack_IDle", false);
            animator.SetBool("Follow", true);
            return;
        }


        if (_Manager.Check_Attack())
        {
            Debug.Log(_Manager.Current_Delay + "쏨");
            animator.SetTrigger("Attack");
            _Manager.Current_Delay = 0;
            
        }
        _Manager._Move.Stop_Enemy();
    }
}
