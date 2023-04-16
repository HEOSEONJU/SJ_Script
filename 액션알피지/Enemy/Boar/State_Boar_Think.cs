using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Boar_Think : StateMachineBehaviour
{
    Boar_Enemy KN;
    [SerializeField]
    float Max_Time = 2.0f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent<Boar_Enemy>(out Boar_Enemy E))
            KN = E;
        KN._boar_AI.Move_OFF();
        KN._RD.velocity = Vector3.zero;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("None") || !animator.GetCurrentAnimatorStateInfo(2).IsTag("None"))
        {
            return;
        }
        animator.SetFloat("ThinkTime", (animator.GetFloat("ThinkTime") + Time.deltaTime));
        Vector3 Dir = KN._boar_AI.Current_Player.position - KN.transform.position;
        KN._boar_AI.Distance_Currennt_Player = Vector3.Distance(KN._boar_AI.Current_Player.position, KN.transform.position);
        animator.SetFloat("Attack_Range", KN._boar_AI.Distance_Currennt_Player);
        KN._boar_AI.Rotate_Target(false);
        if (KN._boar_AI.Attack_Think() || animator.GetFloat("ThinkTime") > Max_Time)
        {
            animator.SetTrigger("Attack_Function");
            animator.SetFloat("ThinkTime", 0);
        }

    }
}