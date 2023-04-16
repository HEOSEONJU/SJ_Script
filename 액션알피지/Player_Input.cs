using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    
    [SerializeField]
    Player_Animator _animator;
    [SerializeField]
    Player_Animaotr_Controller _attackerController;
    Player_Camera _camera;
    public float _horizontal;
    public float _vertical;
    public float _moveAmount;
    public float _mouseX;
    public float _mouseY;

    public bool _isLeftClick;
    public bool _isRightClick;

    public bool _canJump = true;

    float _jump_Dodge;
    bool _jCheck;
    public void Init(Player_Animator PA, Player_Animaotr_Controller PAC)
    {
        _animator = PA;
        _attackerController = PAC;
        
        
        _camera =  Game_Master.instance.PM._camera;


    }
    // Update is called once per frame

    public void Player_Key_Input()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _moveAmount = Mathf.Abs(_horizontal) + Mathf.Abs(_vertical);


        if (Game_Master.instance.PM._connect_Object.IF)
        {
            _mouseX = 0;
            _mouseY = 0;
        }
        else
        {
            _mouseX = Input.GetAxis("Mouse X");
            _mouseY = Input.GetAxis("Mouse Y");
        }
            if (Input.GetKeyDown(KeyCode.I))
        {
            Game_Master.instance.PM.Open_Close_Inventory_Self();
            
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            Game_Master.instance.PM._connect_Object.Connect_IF_Function();
        }
        // Input.GetMouseButtonDown(0)  Input.GetMouseButtonDown(1)
        // Input.GetKeyDown(KeyCode.E)
        if (!Game_Master.instance.PM.Check_UI())
        {
            


            if (Input.GetMouseButtonDown(0))
            {
                if (Game_Master.instance.PM._isGround)
                {
                    if (!Game_Master.instance.PM._isInteracting)
                    {
                        _attackerController.LeftAttack();

                    }
                    else if (_animator._animator.GetInteger("Combo_Stack") >= 1)
                    {
                        _attackerController.LeftAttack();
                    }
                }
                else if (!Game_Master.instance.PM._isGround)
                {

                    if (_animator._animator.GetInteger("Combo_Stack") == 0)
                    {
                        _animator._animator.SetInteger("Combo_Stack", 1);
                    }

                    if (_animator._animator.GetInteger("Combo_Stack") < _attackerController.Air_Attack_Name.Count)
                        _attackerController.RightAttack();


                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                if (Game_Master.instance.PM._isGround)
                {
                    if (!Game_Master.instance.PM._isInteracting)
                    {
                        _animator._animator.SetInteger("Combo_Stack", 0);
                        _attackerController.RightAttack();

                    }

                }

            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Game_Master.instance.PM._isSprintState = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Game_Master.instance.PM._isSprintState = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Game_Master.instance.PM._isWalkState = !Game_Master.instance.PM._isWalkState;

        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Game_Master.instance.PM.LockOnMode = !Game_Master.instance.PM.LockOnMode;

            if (Game_Master.instance.PM.LockOnMode)
            {

                _camera.HandleLockOn();

                if (_camera.nearLockonTarget != null)
                {
                    _camera.CurrentLockonTarget = _camera.nearLockonTarget;
                }
                else
                {
                    Debug.Log("Å¸°Ù¾øÀ½");
                    Game_Master.instance.PM.LockOnMode = false;
                    _camera.ClearListTarget();
                }

            }
            else
            {

                _camera.ClearListTarget();
                if (Game_Master.instance.PM._isWalkState == true)
                {
                    Game_Master.instance.PM._isWalkState = false;
                }
            }
        }


        if (Game_Master.instance.PM.LockOnMode)
        {
            if (Input.GetKeyDown(KeyCode.Space) & Game_Master.instance.PM._isGround & _canJump & !Game_Master.instance.PM._isInteracting)
            {
                _jCheck = true;
            }
            if (Game_Master.instance.PM._isInteracting)
            {
                _jCheck = false;
            }

            if (_jCheck)
            {
                _jump_Dodge += Time.deltaTime;
            }

            if (_jump_Dodge > 0.7f)
            {
                Game_Master.instance.PM._isJump = true;
                _jump_Dodge = 0;
                _jCheck = false;
            }

            if (Input.GetKeyUp(KeyCode.Space) & Game_Master.instance.PM._isGround & _canJump & !Game_Master.instance.PM._isInteracting)
            {



                if (_jCheck && _jump_Dodge <= 0.7f)
                {
                    if (_animator.PlayDodge(_horizontal, _vertical))
                    {

                        _jump_Dodge = 0;
                        _jCheck = false;
                    }






                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) & Game_Master.instance.PM._isGround & _canJump & !Game_Master.instance.PM._isInteracting)
            {

                Game_Master.instance.PM._isJump = true;
                //CanJump = false;
            }
        }
    }

    public void JumpCoolTime()
    {
        StartCoroutine(JCT_cCroutine());
    }
    IEnumerator JCT_cCroutine()
    {

        _canJump = false;
        yield return new WaitForSeconds(1);
        _canJump = true;

    }
}
