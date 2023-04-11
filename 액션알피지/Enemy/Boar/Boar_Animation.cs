using Hit_Type_NameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar_Animation : MonoBehaviour
{
    [SerializeField]
    public Animator _Animator;

    //[SerializeField]
    Boar_Weapon Weapon_Collider;
    [SerializeField]
    Boar_Enemy Manager;

    public void Init()
    {
        Weapon_Collider = GetComponentInChildren<Boar_Weapon>();
    }
    public void WeaponColliderEnable()
    {
        Debug.Log("공격");
        Manager.Clear_ID_List();
        //Debug.Log((Hit_AnimationNumber)_Animator.GetInteger("Attack"));
        Manager.AttackType = (Hit_AnimationNumber)_Animator.GetInteger("Attack");
        Manager.KnockPower = _Animator.GetFloat("KnockPower");

        Weapon_Collider.WeaponColliderEnable();

    }
    public void WeaponColliderDisable()
    {
        Debug.Log("공격끝");
        Weapon_Collider.WeaponColliderDisable();
    }

    public void PlayerTargetAnimation(string targetAnim, bool IsInteracting)
    {
        _Animator.SetBool("IsInteracting", IsInteracting);


        _Animator.CrossFade(targetAnim, 0.2f);

    }
}
