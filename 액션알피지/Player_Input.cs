using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    Player_Manager manager;
    Player_Animator _Animator;
    public Player_Animaotr_Controller _Attacker;
    Player_Camera _Camera;
    public float Horizontal;
    public float Vertical;
    public float MoveAmount;
    public float MouseX;
    public float MouseY;

    public bool LeftClick;
    public bool RightClick;

    public bool CanJump = true;

    float Jump_Dodge;
    bool JCheck;
    public void Init()
    {
        manager = GetComponent<Player_Manager>();
        _Animator = GetComponent<Player_Animator>();
        _Attacker = GetComponentInChildren<Player_Animaotr_Controller>();
        _Camera = manager._Camera;


    }
    // Update is called once per frame

    public void Player_Key_Input()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        MoveAmount = Mathf.Abs(Horizontal) + Mathf.Abs(Vertical);


        if (manager._Connect_Object.IF)
        {
            MouseX = 0;
            MouseY = 0;
        }
        else
        {
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
        }
            if (Input.GetKeyDown(KeyCode.I))
        {
            manager.Open_Close_Inventory_Self();
            
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            manager.Connect_Object_Function();
        }
        // Input.GetMouseButtonDown(0)  Input.GetMouseButtonDown(1)
        // Input.GetKeyDown(KeyCode.E)
        if (!manager._Connect_Object.IF)
        {
            


            if (Input.GetMouseButtonDown(0))
            {
                if (manager.IsGround)
                {
                    if (!manager.IsInteracting)
                    {
                        _Attacker.LeftAttack();

                    }
                    else if (_Animator._animator.GetInteger("Combo_Stack") >= 1)
                    {
                        _Attacker.LeftAttack();
                    }
                }
                else if (!manager.IsGround)
                {

                    if (_Animator._animator.GetInteger("Combo_Stack") == 0)
                    {
                        _Animator._animator.SetInteger("Combo_Stack", 1);
                    }

                    if (_Animator._animator.GetInteger("Combo_Stack") < _Attacker.Air_Attack_Name.Count)
                        _Attacker.RightAttack();


                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                if (manager.IsGround)
                {
                    if (!manager.IsInteracting)
                    {
                        _Animator._animator.SetInteger("Combo_Stack", 0);
                        _Attacker.RightAttack();

                    }

                }

            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            manager.SprintState = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            manager.SprintState = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            manager.WalkState = !manager.WalkState;

        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            manager.LockOnMode = !manager.LockOnMode;

            if (manager.LockOnMode)
            {

                _Camera.HandleLockOn();

                if (_Camera.nearLockonTarget != null)
                {
                    _Camera.CurrentLockonTarget = _Camera.nearLockonTarget;
                }
                else
                {
                    Debug.Log("Å¸°Ù¾øÀ½");
                    manager.LockOnMode = false;
                    _Camera.ClearListTarget();
                }

            }
            else
            {

                _Camera.ClearListTarget();
                if (manager.WalkState == true)
                {
                    manager.WalkState = false;
                }
            }
        }


        if (manager.LockOnMode)
        {
            if (Input.GetKeyDown(KeyCode.Space) & manager.IsGround & CanJump & !manager.IsInteracting)
            {
                JCheck = true;
            }
            if (manager.IsInteracting)
            {
                JCheck = false;
            }

            if (JCheck)
            {
                Jump_Dodge += Time.deltaTime;
            }

            if (Jump_Dodge > 0.7f)
            {
                manager.Jump = true;
                Jump_Dodge = 0;
                JCheck = false;
            }

            if (Input.GetKeyUp(KeyCode.Space) & manager.IsGround & CanJump & !manager.IsInteracting)
            {



                if (JCheck && Jump_Dodge <= 0.7f)
                {
                    if (_Animator.PlayDodge(Horizontal, Vertical))
                    {

                        Jump_Dodge = 0;
                        JCheck = false;
                    }






                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) & manager.IsGround & CanJump & !manager.IsInteracting)
            {

                manager.Jump = true;
                //CanJump = false;
            }
        }
    }

    public void JumpCoolTime()
    {
        StartCoroutine(JCT_coroutine());
    }
    IEnumerator JCT_coroutine()
    {

        CanJump = false;
        yield return new WaitForSeconds(1);
        CanJump = true;

    }
}
