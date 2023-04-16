
using System.Collections.Generic;
using UnityEngine;
using Hit_Type_NameSpace;


public class Player_Manager : MonoBehaviour
{
    [SerializeField]
    public Player_Data Data
    {
        get
        {
            return _fireBaseManger.CPD();
        }

    }
    public FireBase_Manager _fireBaseManger;
    public int _objectID = 0;

    [SerializeField]
    Player_Input _input;
    [SerializeField]
    Player_Move _move;

    public Player_Animator _animator;
    public Player_Camera _camera;
    public Player_Weapon_Slot_Manager _weaponSlotManager;
    [SerializeField]
    Player_Animaotr_Controller _attackerController;
    public Player_Inventory _manager_Inventory;
    public Player_Status _status;
    public Player_Connect_Object _connect_Object;
    public Player_Quest_Box _playerQuestBox;

    public bool _isInfinity = false;
    public bool _isGround = false;
    public bool _isJump = false;
    public bool _isRotate = true;
    public bool _isWalkState = false;
    public bool _isSprintState = false;
    public bool _isInteracting;

    bool _isLock = false;
    public bool LockOnMode
    {
        get { return _isLock; }
        set
        {
            _isLock = value;
        }
    }
    public bool _isAttacking;
    public float _comboTimer;
    public float _magnification;
    bool _isLive = true;
    [SerializeField]
    public bool Open_UI
    {
        get { return Check_UI(); }

    }
    public Transform _charCentePosi;
    public Vector3 _move_VELOCITY;
    public List<int> _enemyIDList = new List<int>();//적 중복공격방지
    [SerializeField]
    bool _isState = false;
    
    [SerializeField]
    LayerMask _groundLayer;
    public EAttack_Special _Attack_Effect;
    public void Init()
    {
        
        _camera = Player_Camera.instance;

        _camera.Init(transform);
        _move.Init(_input, _animator);
        _animator.Init(_input);
        _input.Init(_animator, _attackerController);
        _manager_Inventory.Init();
        _weaponSlotManager.Init();
        _status.init();
        _connect_Object.Init();
        _playerQuestBox.Init();
        _isState = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isState)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.V)) 
        {
            Save_On_FireBase();
        }
        _input.Player_Key_Input();
        _move.SpeedSetting();
        _attackerController.Update_Attack_Timer();
    }
    private void FixedUpdate()
    {
        if (!_isLive)
        {
            return;
        }

        if (_comboTimer > 0)
        {
            _comboTimer -= Time.fixedDeltaTime;
            if (_comboTimer <= 0)
            {
                _animator._animator.SetInteger("Combo_Stack", 0);
            }
        }

        Ground_Check();
        _move_VELOCITY = _move._rigidBody.velocity;
        _move.CalDir();
        _move.Jump_Function();
        _animator.UpdataAnimation(_input._vertical, _input._horizontal, _isSprintState);
        
        if (_camera != null)
        {
            _camera.FollowTarget(Time.deltaTime);
            if (!Open_UI)
            {
                _camera.HandleCameraRotation(Time.deltaTime, _input._mouseX, _input._mouseY);
            }
        }
    }
    private void LateUpdate()
    {
        if (!_isLive)
        {
            return;
        }


        _isInteracting = _animator._animator.GetBool("IsInteracting");
        _isInfinity = _animator._animator.GetBool("Infinity");
        _isAttacking = _animator._animator.GetBool("Attacking");
        _magnification = _animator._animator.GetFloat("Magnification");
        _isRotate = _animator._animator.GetBool("Rotate");
        _input._isLeftClick = false;
        _input._isRightClick = false;

        
        _animator._animator.SetBool("IsGround", _isGround);
        bool d = Mathf.Abs(_input._vertical) + Mathf.Abs(_input._horizontal) == 0 ? d = true : d = false;
        _animator._animator.SetBool("MoveAmount", d);
        _animator._animator.SetBool("Lock_On_Camera", LockOnMode);
        if (LockOnMode)
        {
            _isWalkState = true;
            _isSprintState = false;
        }
        _animator._animator.SetBool("WalkState", _isWalkState);
        _animator._animator.SetBool("SprintState", _isSprintState);
    }






    #region UI관련
    public bool Check_UI()
    {
        if (Game_Master.instance.UI.Count_Open_UI > 0)
        {
            return true;
        }
        return false;
    }


    public void Open_Close_Inventory_Self()
    {
        if (_connect_Object.IF)
        {
            return;
        }

        if (Open_UI)
        {
            _manager_Inventory.Open_Close_Equip_Window(false);
            _manager_Inventory.Open_Close_Inventory_Window(false);
        }
        else
        {
            _manager_Inventory.Open_Close_Equip_Window(true);
            _manager_Inventory.Open_Close_Inventory_Window(true);
        }

    }
    #endregion



    #region 데미지관련

    public void Enemy_Damaged(Enemy_Base_Status temp)
    {
        if (temp.Live)
        {
            Vector3 Weapon_Position = _weaponSlotManager.Main_Weapon._currentWeapon.transform.position;
            _attackerController.temp.transform.position = Vector3.Lerp(_attackerController.temp.transform.position, Weapon_Position, 0.9f);
            _attackerController.temp.transform.rotation = _weaponSlotManager.Main_Weapon._currentWeapon.transform.rotation;
            _attackerController.temp.Play();

            int CR = Random.Range(1, 101);

            float Damage_Point = _status.ATK_Point;
            if (CR >= 100 - _status.CRP_Point)
            {
                Damage_Point = 1f * (Damage_Point * (100 + _status.CRT_Point)) / 100f;
            }
            Damage_Point *= _magnification;
            int INT_DAMGE = (int)Damage_Point;

            _enemyIDList.Add(temp.ID);
            
            
            if (temp.Damaged(INT_DAMGE, this.transform, _Attack_Effect))
            {
                LockOnMode = false;
                _camera.ClearListTarget();
                if (_isWalkState == true)
                {
                    _isWalkState = false;
                }
            }

        }
    }

    public void Reset_IDList()
    {
        _enemyIDList.Clear();
    }

    public void PlayerDamaged(int Damage, Vector3 Dir, EHit_AnimationNumber AttackType, float KnockPower)
    {

        if (_isInfinity)
        {
            Debug.Log("무적회피");
            return;
        }

        Debug.Log(Vector3.Angle(transform.forward, Dir));
        Damage = (int)(Damage + (KnockPower / 3));
        Quaternion Q = transform.rotation;
        Vector3 temp = Q.eulerAngles;
        
        temp.y -= Vector3.Angle(transform.forward, Dir);
        temp.x = 0;
        temp.z = 0;
        Q = Quaternion.Euler(temp);
        transform.rotation = Q;

        _animator._animator.SetFloat("Knock", KnockPower);
        if (_isGround)
        {
            switch (AttackType)
            {
                case EHit_AnimationNumber.Weak:
                    _animator.PlayerTargetAnimation("Damage_Front_Big_ver_A", true);
                    //_move._rigidBody.AddForce(KnockPower * -transform.forward, ForceMode.VelocityChange);
                    break;
                case EHit_AnimationNumber.Nomal:
                    _animator.PlayerTargetAnimation("Damage_Front_Big_ver_C", true);
                    //_move._rigidBody.AddForce(KnockPower * -transform.forward, ForceMode.VelocityChange);
                    break;
                case EHit_AnimationNumber.Hard:
                    _animator.PlayerTargetAnimation("Damage_Front_High_KnockDown", true);
                    //_move._rigidBody.AddForce(KnockPower / 2f * -transform.forward, ForceMode.VelocityChange);
                    
                    break;
                default:
                    break;
            }
        }
        else
        {
            _animator.PlayerTargetAnimation("Damage_Front_Flying_ver_B", true);
        }
        if (_status.Damaged_HP(Damage))
        {
            _isLive = false;
            _animator.PlayerTargetAnimation("Damage_Die", true);
        }
    }
    #endregion

    #region 데이터관련

    public void Save_On_FireBase()
    {
        _fireBaseManger.Save_On_FireBase();
    }


    public void Data_Save(bool T)//인벤토리내용을 플레이어데이터로옮기는과정 인벤토리열때 작동
    {

        if (T)
            Save_On_FireBase();
    }

    #endregion

    #region 지상접촉 탐지
    public void Ground_Check()//땅에 닿을 위치면 땅위치로 이동시킴
    {

        if (_animator._animator.GetBool("JUMP"))
        {
            _isGround = false;
            return;
        }
        Vector3 RaycastOrigin = transform.position;
        RaycastOrigin.y += 1f;
        Vector3 Last_Posi = transform.position;

        if (Physics.SphereCast(RaycastOrigin, 0.2f, Vector3.down, out RaycastHit hit, 1.1f, _groundLayer))
        {

            Vector3 Temp = hit.point;
            Last_Posi.y = Temp.y;
            transform.position = Last_Posi;

            _isGround = true;
        }
        else
        {
            _isGround = false;
        }



    }

    #endregion





}

