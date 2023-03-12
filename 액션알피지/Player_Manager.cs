using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hit_Type_NameSpace;
using UnityEditor;

public class Player_Manager : MonoBehaviour
{
    [SerializeField]
    Player_Data Data;
    public FireBase_Manager FireBase_M;
    public int ObjectID = 0;


    Player_Input _Input;
    Player_Move _Move;
    Player_Animator _Animator;
    public Player_Camera _Camera;
    public Player_Weapon_Slot_Manager _WeaponSlotManager;
    Player_Animaotr_Controller _Attacker;
    public Player_Inventory _Manager_Inventory;
    public Player_Status _Status;
    public Player_Connect_NPC _Connect_NPC;
    public bool Infinity = false;
    public bool IsGround = false;
    public bool Jump = false;
    public bool Rotate = true;
    public bool WalkState = false;
    public bool SprintState = false;
    public bool IsInteracting;
    public bool LockOnMode;
    public bool Attacking;


    public float Magnification;
    bool Live = true;
    [SerializeField]
    bool Open_UI;

    public Transform Char_Center_Posi;

    public UI_Manager _UI;

    [SerializeField]
    bool State = false;
    // Start is called before the first frame update
    public void Init(Player_Data Load_Data)
    {
        Data=Load_Data;
        Game_Master.instance.Load_Player(this);

        _Camera = Player_Camera.instance;
        _Camera.Init(transform);
        _Input = GetComponent<Player_Input>();
        _Input.Init();
        _Move = GetComponent<Player_Move>();
        _Move.Init();
        _Animator = GetComponent<Player_Animator>();
        _Animator.Init();
        _Attacker = GetComponentInChildren<Player_Animaotr_Controller>();
        _WeaponSlotManager = GetComponent<Player_Weapon_Slot_Manager>();

        _Manager_Inventory = gameObject.GetComponent<Player_Inventory>();
        _UI.Init(this);
        Game_Master.instance.Load_UI(_UI);
        _Manager_Inventory.Init(this, Data);
        _WeaponSlotManager.Init(_Manager_Inventory);
        _Status = GetComponent<Player_Status>();
        _Status.init();
        _Connect_NPC = GetComponentInChildren<Player_Connect_NPC>();
        _Connect_NPC.Init(this);
        State = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch(State)
        {
            case true:

                if (!Live)
                {
                    return;
                }
                float delta = Time.deltaTime;
                _Input.Player_Key_Input();

                if (Input.GetKeyDown(KeyCode.V))
                {
                    //Data_Save();
                }


                _Move.SpeedSetting();
                _Animator.CanRotate();
                _Attacker.Update_Attack_Timer();


                break;

        }


    }
    private void FixedUpdate()
    {
        if (!Live)
        {
            return;
        }
        float delta = Time.deltaTime;



        switch(State)
        {
            case true:
                _Move.CalDir();





                _Move.GroundMove();
                _Move.Falling();


                _Move.Speed_Control();

                _Move.CharRotate(delta);
                _Move.Jump_Function();

                _Animator.UpdataAnimation(_Input.Vertical, _Input.Horizontal, SprintState);
                if (_Camera != null)
                {

                    _Camera.FollowTarget(delta);
                    if (!Open_UI)//���߿� UI�������� �ٲܰ�
                    {

                        _Camera.HandleCameraRotation(delta, _Input.MouseX, _Input.MouseY);
                    }
                }
                break;
        }




    }
    private void LateUpdate()
    {
        switch(State)
        {
            case true:

                if (!Live)
                {
                    return;
                }
                float delta = Time.deltaTime;
                _Animator._animator.SetBool("IsGround", IsGround);
                IsInteracting = _Animator._animator.GetBool("IsInteracting");
                Infinity = _Animator._animator.GetBool("Infinity");
                Attacking = _Animator._animator.GetBool("Attacking");
                Magnification = _Animator._animator.GetFloat("Magnification");
                _Input.LeftClick = false;
                _Input.RightClick = false;

                bool d = Mathf.Abs(_Input.Vertical) + Mathf.Abs(_Input.Horizontal) == 0 ? d = true : d = false;

                _Animator._animator.SetBool("MoveAmount", d);

                _Animator._animator.SetBool("Lock_On_Camera", LockOnMode);
                if (LockOnMode)
                {
                    WalkState = true;
                    SprintState = false;
                }
                _Animator._animator.SetBool("WalkState", WalkState);
                _Animator._animator.SetBool("SprintState", SprintState);


                break;
        }

    }

    public Player_Data Call_Data()
    {
        return Data;
    }

    public List<int> Enemy_IDList = new List<int>();

    public void Reset_IDList()
    {
        Enemy_IDList.Clear();
    }

    public void Check_UI()
    {

        if (_Manager_Inventory.Inventory_Open || _Manager_Inventory.Equip_Open)
        {
            Open_UI = true;
            return;
        }

        Open_UI = false;

    }
    public bool Return_Open_UI()
    {
        return Open_UI;
    }

    public void Setting_UI_Open(bool state)
    {
        Open_UI = state;
    }

    public void Open_Close_Inventory_Self()
    {
        if (_Connect_NPC.Connecting_Check())
        {
            return;
        }
        Check_UI();
        if (Return_Open_UI())
        {
            _Manager_Inventory.Open_Close_Equip_Window(false);
            _Manager_Inventory.Open_Close_Inventory_Window(false);
        }
        else
        {
            _Manager_Inventory.Open_Close_Equip_Window(true);
            _Manager_Inventory.Open_Close_Inventory_Window(true);
        }
        Check_UI();
    }

    public void Connect_NPC_Function()
    {
        if (!_Connect_NPC.Connect())
        {
            return;

        }
        if (_Connect_NPC.Connecting_Check())
        {
            _Connect_NPC.DisConnect_NPC();
            _Manager_Inventory.Open_Close_Inventory_Window(false);
            Check_UI();
            return;
        }


        _Connect_NPC.Connect_NPC();
        _Manager_Inventory.Open_Close_Inventory_Window(true);
        if (_Manager_Inventory.Equip_Open)
        {
            _Manager_Inventory.Open_Close_Equip_Window(false);
        }

        Check_UI();
    }

    public void Enemy_Damaged(Enemy_Base_Status temp)
    {
        if (temp.Live)
        {
            Vector3 Weapon_Position = _WeaponSlotManager.Main_Weapon.Current_Weapon.transform.position;
            _Attacker.temp.transform.position = Vector3.Lerp(_Attacker.temp.transform.position, Weapon_Position, 0.9f);
            _Attacker.temp.transform.rotation = _WeaponSlotManager.Main_Weapon.Current_Weapon.transform.rotation;
            _Attacker.temp.Play();
            int CR = Random.Range(1, 101);
            float Damage_Point = _Status.ATK_Point;

            if (CR >= 100 - _Status.CRP_Point)
            {
                Damage_Point = 1f * (Damage_Point * (100 + _Status.CRT_Point)) / 100f;
            }
            Damage_Point *= Magnification;
            int INT_DAMGE = (int)Damage_Point;
            Enemy_IDList.Add(temp.ID);
            if (temp.Damaged(INT_DAMGE, this.transform))
            {
                LockOnMode = false;
                _Camera.ClearListTarget();
                if (WalkState == true)
                {
                    WalkState = false;
                }
            }

        }
    }



    public void PlayerDamaged(int Damage, Vector3 Dir, Hit_AnimationNumber AttackType, float KnockPower)
    {

        if (Infinity)
        {
            Debug.Log("����ȸ��");
            return;
        }

        transform.LookAt(Dir);



        Quaternion temp = transform.rotation;
        temp.Set(0, temp.y, 0, temp.w);
        transform.rotation = temp;


        if (IsGround)
        {
            switch (AttackType)
            {
                case Hit_AnimationNumber.Weak:
                    _Animator.PlayerTargetAnimation("Damage_Front_Big_ver_A", true);
                    _Move.rb.AddForce(KnockPower * -transform.forward, ForceMode.VelocityChange);
                    break;
                case Hit_AnimationNumber.Nomal:
                    _Animator.PlayerTargetAnimation("Damage_Front_Big_ver_C", true);
                    _Move.rb.AddForce(KnockPower * -transform.forward, ForceMode.VelocityChange);
                    break;
                case Hit_AnimationNumber.Hard:
                    _Animator.PlayerTargetAnimation("Damage_Front_High_KnockDown", true);
                    _Move.rb.AddForce(KnockPower / 2f * -transform.forward, ForceMode.VelocityChange);
                    _Move.rb.AddForce(KnockPower / 4f * transform.up, ForceMode.VelocityChange);
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (AttackType)
            {
                case Hit_AnimationNumber.Weak:
                    _Animator.PlayerTargetAnimation("Damage_Front_Big_ver_A", true);
                    _Move.rb.AddForce(KnockPower / 10f * -transform.forward, ForceMode.Impulse);
                    break;
                case Hit_AnimationNumber.Nomal:
                    _Animator.PlayerTargetAnimation("Damage_Front_Big_ver_C", true);
                    _Move.rb.AddForce(KnockPower / 10f * -transform.forward, ForceMode.Impulse);
                    break;
                case Hit_AnimationNumber.Hard:
                    _Animator.PlayerTargetAnimation("Damage_Front_High_KnockDown", true);
                    _Move.rb.AddForce(KnockPower / 20f * -transform.forward, ForceMode.Impulse);
                    _Move.rb.AddForce(KnockPower / 40f * transform.up, ForceMode.Impulse);
                    break;
                default:
                    break;
            }
        }
        if (_Status.Damaged_HP(Damage))
        {
            Live = false;
            _Animator.PlayerTargetAnimation("Damage_Die", true);
        }


    }


    public void Load_On_FireBase()
    {
        FireBase_M.Load();
    }

    public void Save_On_FireBase()
    {
        FireBase_M.save();
    }
    public void Data_Load()//�÷��̾���Ϳ��ִ³����� �κ��丮�� �ű�°���//�κ��丮�ݾƼ� �ٲｽ����ġ ����Ѿ����� ����
    {
        _Manager_Inventory.Main_Weapon.Slot_IN_Item = Data.Equip_Weapon_Item;
        for (int i = 0; i < Data.Equip_Armor_Item.Count; i++)
        {
            _Manager_Inventory.Equip_Armors[i].Slot_IN_Item = Data.Equip_Armor_Item[i];
        }

        for (int i = 0; i < Data.Items.Count; i++)
        {
            _Manager_Inventory.Item_List[i].Slot_IN_Item = Data.Items[i];
        }
    }

    public void Data_Save(bool T)//�κ��丮������ �÷��̾���ͷοű�°��� �κ��丮���� �۵�
    {
        if (_Manager_Inventory.Main_Weapon != null)
        {
            Data.Equip_Weapon_Item = _Manager_Inventory.Main_Weapon.Slot_IN_Item;
            
        }
        else
        {
            Data.Equip_Weapon_Item = null;
            
        }

        for (int i = 0; i < Data.Equip_Armor_Item.Count; i++)
        {
            if (_Manager_Inventory.Equip_Armors[i].Slot_IN_Item != null)
            {
                Data.Equip_Armor_Item[i] = _Manager_Inventory.Equip_Armors[i].Slot_IN_Item;
            }
            else
            {
                Data.Equip_Armor_Item[i] = null;
            }


        }

        for (int i = 0; i < Data.Items.Count; i++)
        {
            if (_Manager_Inventory.Item_List[i].Slot_IN_Item != null)
            {

                        Data.Items[i] = _Manager_Inventory.Item_List[i].Slot_IN_Item;
                
                
                
            }
            else
            {
                Data.Items[i] = null;
            }


        }
        if(T)
        Save_On_FireBase();
    }

    


}

namespace Hit_Type_NameSpace
{
    public enum Hit_AnimationNumber
    {
        Weak, Nomal, Hard
    }
}