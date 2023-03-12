using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Monster_Attack_Setting : StateMachineBehaviour
{
    [SerializeField]
    int AttackType;
    [SerializeField]
    float KnockPower;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        animator.SetInteger("Attack", AttackType);
        animator.SetFloat("KnockPower", KnockPower);
    }

}
