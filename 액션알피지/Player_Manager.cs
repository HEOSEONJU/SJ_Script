using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hit_Type_NameSpace;
using UnityEditor;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Unity.Mathematics;

public class Player_Manager : MonoBehaviour
{
    [SerializeField]
    public Player_Data Data
    {
        get 
        {
            return FireBase_M.CPD();
        }
        
    }
    public FireBase_Manager FireBase_M;
    public int ObjectID = 0;

    [SerializeField]
    Player_Input _Input;
    [SerializeField]
    Player_Move _Move;
    public Player_Animator _Animator;
    public Player_Camera _Camera;
    public Player_Weapon_Slot_Manager _WeaponSlotManager;
    Player_Animaotr_Controller _Attacker;
    public Player_Inventory _Manager_Inventory;
    public Player_Status _Status;
    public Player_Connect_Object _Connect_Object;
    public Player_Quest_Box PQB;
    public bool Infinity = false;
    public bool IsGround = false;
    public bool Jump = false;
    public bool Rotate = true;
    public bool WalkState = false;
    public bool SprintState = false;
    public bool IsInteracting;

    bool Lock = false;
    public bool LockOnMode
    {
        get { return Lock; }
        set{
            Lock = value;
        }
    }
    public bool Attacking;
    
    public float Combo_Timer;

    public float Magnification;
    bool Live = true;
    [SerializeField]
    public bool Open_UI
    {
        get { return Check_UI(); }
        
    }


    public Transform Char_Center_Posi;

    public UI_Manager _UI;

    public Vector3 CC_VELOCITY;



    public List<int> Enemy_IDList = new List<int>();//적 중복공격방지
    [SerializeField]
    bool State = false;
    
    [SerializeField]
    List<GameObject> ColliderList;//땅에 접촉한 오브젝트갯수
    [SerializeField]
    LayerMask GroundLayer;
    // Start is called before the first frame update
    public void Init()
    {
        Debug.Log("init시작");
        
        //Game_Master.instance.Load_Player(this);
        //Game_Master.instance.Load_UI(_UI);
        ColliderList =new List<GameObject>();
        _Camera = Player_Camera.instance;
        _Camera.Init(transform);
        
        _Input.Init();
        
        _Move.Init();
        
        _Animator.Init();
        _Attacker = GetComponentInChildren<Player_Animaotr_Controller>();
        

        
        _UI.Init();
        
        _Manager_Inventory.Init();
        _WeaponSlotManager.Init();
        _Status = GetComponent<Player_Status>();
        _Status.init();
        _Connect_Object = GetComponentInChildren<Player_Connect_Object>();
        _Connect_Object.Init();
        PQB.Init();
        State = true;
        Debug.Log("initi종료");
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
                
                _Input.Player_Key_Input();

                if (Input.GetKeyDown(KeyCode.V))
                {
                    //Data_Save();
                }


                _Move.SpeedSetting();
                
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
        if(Combo_Timer>0)
        {
            Combo_Timer -= Time.fixedDeltaTime;
            if (Combo_Timer <= 0)
            {
                _Animator._animator.SetInteger("Combo_Stack", 0);
            }
        }
        
        Ground_Check();
        CC_VELOCITY = _Move.rb.velocity;
        _Move.CalDir();
        //Ground_Check();
        _Move.Jump_Function();
        _Animator.UpdataAnimation(_Input.Vertical, _Input.Horizontal, SprintState);
        switch (State)
        {
            case true:

                break;
        }
        if (_Camera != null)
        {

            _Camera.FollowTarget(Time.deltaTime);
            if (!Open_UI)
            {

                _Camera.HandleCameraRotation(Time.deltaTime, _Input.MouseX, _Input.MouseY);
            }
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



                
                //IsGround = _Move.CC.isGrounded;
                _Animator._animator.SetBool("IsGround", IsGround);
                IsInteracting = _Animator._animator.GetBool("IsInteracting");
                Infinity = _Animator._animator.GetBool("Infinity");
                Attacking = _Animator._animator.GetBool("Attacking");
                Magnification = _Animator._animator.GetFloat("Magnification");
                Rotate = _Animator._animator.GetBool("Rotate");
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






    #region UI관련
    public bool Check_UI()
    {

        if (Game_Master.instance.UI.Count_Open_UI>0)
        {
            return true;
        }
        return false;
        

    }
    

    public void Open_Close_Inventory_Self()
    {
        if (_Connect_Object.IF)
        {
            return;
        }
        
        if (Open_UI)
        {
            _Manager_Inventory.Open_Close_Equip_Window(false);
            _Manager_Inventory.Open_Close_Inventory_Window(false);
        }
        else
        {
            _Manager_Inventory.Open_Close_Equip_Window(true);
            _Manager_Inventory.Open_Close_Inventory_Window(true);
        }
        
    }
    #endregion



    #region 데미지관련

    public void Enemy_Damaged(Enemy_Base_Status temp)
    {
        if (temp.Live)
        {
            Vector3 Weapon_Position = _WeaponSlotManager.Main_Weapon.Current_Weapon.transform.position;
            _Attacker.temp.transform.position = Vector3.Lerp(_Attacker.temp.transform.position, Weapon_Position, 0.9f);
            _Attacker.temp.transform.rotation = _WeaponSlotManager.Main_Weapon.Current_Weapon.transform.rotation;
            _Attacker.temp.Play();
            int CR = UnityEngine.Random.Range(1, 101);
            float Damage_Point = _Status.ATK_Point;

            if (CR >= 100 - _Status.CRP_Point)
            {
                Damage_Point = 1f * (Damage_Point * (100 + _Status.CRT_Point)) / 100f;
            }
            Damage_Point *= Magnification;
            int INT_DAMGE = (int)Damage_Point;
            Enemy_IDList.Add(temp.ID);
            if (temp.Damaged(INT_DAMGE, this.transform,1))
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

    public void Reset_IDList()
    {
        Enemy_IDList.Clear();
    }

    public void PlayerDamaged(int Damage, Vector3 Dir, Hit_AnimationNumber AttackType, float KnockPower)
    {

        if (Infinity)
        {
            Debug.Log("무적회피");
            return;
        }

        
        Debug.Log(Vector3.Angle(transform.forward, Dir));

        Damage = (int)(Damage + (KnockPower / 3));

        
        Quaternion Q = transform.rotation;

        Vector3 temp = Q.eulerAngles;
        //Debug.Log("Before" + temp.y);
        temp.y -= Vector3.Angle(transform.forward, Dir);
        temp.x = 0;
        temp.z = 0;
        Q = Quaternion.Euler(temp);
        //Debug.Log("Before" + Q.eulerAngles);
        //Debug.Log("After" + transform.rotation.eulerAngles);
        transform.rotation = Q;
        
        _Animator._animator.SetFloat("Knock", KnockPower);
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
                    //_Move.rb.AddForce(KnockPower / 4f * transform.up, ForceMode.VelocityChange);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _Animator.PlayerTargetAnimation("Damage_Front_Flying_ver_B", true);
        }
        if (_Status.Damaged_HP(Damage))
        {
            Live = false;
            _Animator.PlayerTargetAnimation("Damage_Die", true);
        }


    }
    #endregion

    #region 데이터관련

    public void Save_On_FireBase()
    {
        FireBase_M.Save_On_FireBase();
    }
    

    public void Data_Save(bool T)//인벤토리내용을 플레이어데이터로옮기는과정 인벤토리열때 작동
    {
        
        if(T)
        Save_On_FireBase();
    }

    #endregion

    #region 지상접촉 탐지
    void Ground()
    {
        
        if (ColliderList.Count > 0)
        {
            IsGround = true;
            return;
        }
        

        IsGround = false;
    }

    public void Ground_Check()//땅에 닿을 위치면 땅위치로 이동시킴
    {
        
        if(_Animator._animator.GetBool("JUMP"))
        {
            IsGround = false;
            return;
        }
        Vector3 RaycastOrigin = transform.position;
        RaycastOrigin.y += 1f;
        Vector3 Last_Posi= transform.position;
        
        if (Physics.SphereCast(RaycastOrigin,0.2f, Vector3.down,out RaycastHit hit,1.1f, GroundLayer))
        {
            
            Vector3 Temp = hit.point;
            Last_Posi.y = Temp.y;
            transform.position = Last_Posi;
            
            IsGround = true;
        }
        else
        {
            IsGround =false;
        }
        
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            //Ground();
        }
    }
    public float Dis;
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Ground();
        }
    }
    #endregion





}

namespace Hit_Type_NameSpace
{
    public enum Hit_AnimationNumber
    {
        Weak, Nomal, Hard
    }
}