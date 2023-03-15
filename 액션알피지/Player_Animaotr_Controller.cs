using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animaotr_Controller : MonoBehaviour
{
    Player_Move _Move;
    Player_Animator _Animator;
    Player_Manager manager;
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

        _Move = GetComponentInParent<Player_Move>();
        _Animator = GetComponentInParent<Player_Animator>();
        manager = GetComponentInParent<Player_Manager>();
        _Input = GetComponentInParent<Player_Input>();
        _Weapon_Slot_Manager = GetComponentInParent<Player_Weapon_Slot_Manager>();
        Player_Camera_Transform = Camera.main.transform;
    }
    public Vector3 deltaPosition_ATK;
    private void OnAnimatorMove()
    {


        if (Check_Animation())//��Ÿ�����ǻ�������ʴ� �ִϸ��̼��ǰ�� ��ȯ
        {
            return;
        }
        float delta = Time.deltaTime;
        if (_Animator._animator.GetCurrentAnimatorStateInfo(1).IsTag("Land_Air_Attack")) //���󿡼� ���������� ����
        {
            _Move.rb.drag = DragPower;
            Vector3 deltaPosition_air = _Animator._animator.deltaPosition / delta;
            
            deltaPosition_air.y += _Move.rb.velocity.y * delta;
            deltaPosition_air.x *= PushPower*delta;
            deltaPosition_air.z *= PushPower*delta;

            _Move.rb.velocity = deltaPosition_air;

            if (manager.LockOnMode)
            {
                transform.LookAt(manager._Camera.CurrentLockonTarget);
                Quaternion temp = transform.rotation;
                temp.Set(0, temp.y, 0, temp.w);
                transform.rotation = temp;

            }

            return;
        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(1).IsTag("Air_Attack"))//���߰���
        {
            _Move.rb.drag = DragPower;
            Vector3 deltaPosition_air = _Animator._animator.deltaPosition / delta;
            //deltaPosition_air.y *= 1.5f;
            deltaPosition_air.y += _Move.rb.velocity.y * delta;
            //deltaPosition_air.x *= PushPower/delta;
            //deltaPosition_air.z *= PushPower/delta;

            _Move.rb.velocity = deltaPosition_air;
            return;
        }

        //ĳ���� ������ȯ �и����ϸ� �巡�׿ø��� ���ν�Ƽ�� ����





        // ��������� ������� ������
        _Move.rb.drag = DragPower;
        Vector3 deltaPosition = _Animator._animator.deltaPosition;
        deltaPosition.y += _Move.rb.velocity.y * delta;
        deltaPosition.x *= PushPower / delta;
        deltaPosition.z *= PushPower / delta;

        _Move.rb.velocity = deltaPosition;

        //�����޺����
        //float delta = Time.deltaTime;
        //_Move.rb.drag = 0;
        //Vector3 deltaPosition = _Animator.deltaPosition;
        //deltaPosition.y = 0;
        //Vector3 velocity = deltaPosition / delta;
        //_Move.rb.velocity = velocity;
    }


    bool Check_Animation()//�ش��±׿� �´� �ִϸ��̼��� ��Ʈ��ǿ������̵��ϱ� ���� ����
    {

        if (_Animator._animator.GetCurrentAnimatorStateInfo(2).IsTag("Hit_Player"))
        {

            return true;
        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
        {

            return false;
        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(1).IsTag("Air_Attack"))
        {
            return false;
        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(1).IsTag("Land_Air_Attack"))
        {
            return false;
        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge"))
        {
            return false;
        }
        return true;
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
        _Animator._animator.SetInteger("Combo_Stack", 0);
    }

    void OnWeaponCollider()
    {
        //Debug.Log("�ݶ��̴�Ȱ��ȭ �޺���ȣ:" + Combo_Stack);
        manager.Reset_IDList();

        _Weapon_Slot_Manager.AbleWeaponCollider(true);

    }
    void OffWeaponCollider()
    {

        //Debug.Log("�ݶ��̴���Ȱ��ȭ �޺���ȣ:" + Combo_Stack);
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
        //Debug.Log("�ʱ�ȭ");
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
        float moveOverrride = _Input.MoveAmount;

        targetDir = Player_Camera_Transform.transform.forward * _Input.Vertical;
        targetDir += Player_Camera_Transform.transform.right * _Input.Horizontal;


        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        float rotationSpeed = 100;
        float rs;
        if (manager.IsGround)
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
