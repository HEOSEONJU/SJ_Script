using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hit_Type_NameSpace;
public class Knight_Animation : MonoBehaviour
{
    [SerializeField]
    public Animator _Animator;

    [SerializeField]
    Knight_Weapon Weapon_Collider;

    Knight_Enemy Manager;

    private void Start()
    {
        _Animator = GetComponent<Animator>();
        Weapon_Collider = GetComponentInChildren<Knight_Weapon>();
        Manager = GetComponent<Knight_Enemy>();
        
    }











    public void WeaponColliderEnable()
    {
        Manager.Clear_ID_List();
        //Debug.Log((Hit_AnimationNumber)_Animator.GetInteger("Attack"));
        Manager.AttackType = (Hit_AnimationNumber)_Animator.GetInteger("Attack");
        Manager.KnockPower = _Animator.GetFloat("KnockPower");

        Weapon_Collider.WeaponColliderEnable();

    }
    public void WeaponColliderDisable()
    {

        Weapon_Collider.WeaponColliderDisable();
    }

    public void PlayerTargetAnimation(string targetAnim,bool IsInteracting)
    {
        _Animator.SetBool("IsInteracting", IsInteracting);


        _Animator.CrossFade(targetAnim, 0.2f);

    }
}
