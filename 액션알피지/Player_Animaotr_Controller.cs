using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animaotr_Controller : MonoBehaviour
{

    public Player_Manager manager;
    public Player_Move _Move;
    [SerializeField]
    Player_Animator _Animator;
    [SerializeField]
    Player_Input _Input;



    //string _Last_Name;
    Transform Player_Camera_Transform;

    Player_Weapon_Slot_Manager _Weapon_Slot_Manager;


    [SerializeField]
    public List<string> Attack_Name;
    [SerializeField]
    public List<string> Air_Attack_Name;
    //public int Combo_Stack=0;
    [SerializeField]
    bool Can_Attack = true;


    [SerializeField]
    float DragPower;
    [SerializeField]
    float PushPower;

    [SerializeField]
    public ParticleSystem temp;

    

    private void Awake()
    {

        _Weapon_Slot_Manager = GetComponentInParent<Player_Weapon_Slot_Manager>();
        Player_Camera_Transform = Camera.main.transform;
    }
    public Vector3 deltaPosition_ATK;
    public Vector3 deltaPosition;
    private void OnAnimatorMove()
    {
        deltaPosition=_Animator._animator.deltaPosition;
    }

    public void Change_Weapon_Type(List<string> New_Weapon_Animation_List = null, List<string> New_Air_Weapon_Animation_List = null)
    {
        Attack_Name.Clear();
        if (New_Weapon_Animation_List != null)
        {
            for (int i = 0; New_Weapon_Animation_List.Count > i; i++)
            {
                Attack_Name.Add(New_Weapon_Animation_List[i]);
            }
        }
        Air_Attack_Name.Clear();
        if (New_Air_Weapon_Animation_List != null)
        {

            for (int i = 0; New_Air_Weapon_Animation_List.Count > i; i++)
            {
                Air_Attack_Name.Add(New_Air_Weapon_Animation_List[i]);
            }
        }
        _Animator._animator.SetInteger("Combo_Stack", 0);
    }
    void EnableAttack()
    {
        Can_Attack = true;
        OffWeaponCollider();
    }
    void DisableAttack()
    {
        Can_Attack = false;

    }
    void ComboReset()
    {

        Debug.Log("시간초과로 콤보리셋");
        _Animator._animator.SetInteger("Combo_Stack", 0);
    }

    void OnWeaponCollider()
    {
        //Debug.Log("콜라이더활성화 콤보번호:" + Combo_Stack);
        manager.Reset_IDList();

        _Weapon_Slot_Manager.AbleWeaponCollider(true);

    }
    void OffWeaponCollider()
    {

        //Debug.Log("콜라이더비활성화 콤보번호:" + Combo_Stack);
        _Weapon_Slot_Manager.AbleWeaponCollider(false);
    }
    void Interacting_True()
    {

        _Animator._animator.SetBool("IsInteracting", true);
    }
    void Interacting_False()
    {

        _Animator._animator.SetBool("IsInteracting", false);

    }

    void Falling_Start()
    {
        //Debug.Log("초기화");
        //Combo_Stack = 0;
        //DisableAttack();
    }

    void End_Air_Combo()
    {

        //OffWeaponCollider();
        //_Animator.PlayerTargetAnimation("Empty", false);
        //_Animator.PlayerTargetAnimation("Falling", false);
        //Combo_Stack = 0;

    }

    public void LeftAttack()
    {
        if (!Can_Attack)
            return;
        if (Attack_Name.Count == 0)
        {
            return;
        }


        if (!manager.LockOnMode)
            ChangeDir();

        _Animator.PlayerTargetAnimation(Attack_Name[_Animator._animator.GetInteger("Combo_Stack")], true);
        _Animator._animator.SetInteger("Combo_Stack", _Animator._animator.GetInteger("Combo_Stack") + 1);
        DisableAttack();
        if (_Animator._animator.GetInteger("Combo_Stack") >= Attack_Name.Count)
        {
            _Animator._animator.SetInteger("Combo_Stack", 0);
        }

    }


    public void RightAttack()
    {
        if (!Can_Attack)
            return;
        if (Air_Attack_Name.Count == 0)
        {
            return;
        }

        if (!manager.LockOnMode)
            ChangeDir();
        Debug.Log("우클릭콤보"+ _Animator._animator.GetInteger("Combo_Stack"));
        _Animator.PlayerTargetAnimation(Air_Attack_Name[_Animator._animator.GetInteger("Combo_Stack")], true);
        _Animator._animator.SetInteger("Combo_Stack", _Animator._animator.GetInteger("Combo_Stack") + 1);
        DisableAttack();
        if (_Animator._animator.GetInteger("Combo_Stack") >= Air_Attack_Name.Count)
        {
            _Animator._animator.SetInteger("Combo_Stack", 0);
        }

    }

    float Timer = 0;
    public void Update_Attack_Timer()
    {
        if (!Can_Attack)
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Timer = 0;
        }

        if (Timer > 1.5)
        {
            Can_Attack = true;
        }

    }

    public void ChangeDir()
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverrride = _Input._moveAmount;

        targetDir = Player_Camera_Transform.transform.forward * _Input._vertical;
        targetDir += Player_Camera_Transform.transform.right * _Input._horizontal;


        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        float rotationSpeed = 100;
        float rs;
        if (manager._isGround)
        {
            rs = rotationSpeed;
        }
        else
        {
            rs = rotationSpeed / 15f;
        }

        Quaternion tr = Quaternion.LookRotation(targetDir);


        transform.parent.rotation = tr;
    }



}
