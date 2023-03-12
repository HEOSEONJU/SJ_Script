using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator : MonoBehaviour
{
    Player_Manager manager;
    [SerializeField]
    public Animator _animator;
    Player_Input _Input;
   
    int vertical;
    int horizontal;

    
    public void Init()
    {
        manager = GetComponent<Player_Manager>();
        _Input = GetComponent<Player_Input>();
       _animator = GetComponentInChildren<Animator>();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdataAnimation(float verticalMove, float horizontalMove, bool isSprinting)
    {

        if(manager.LockOnMode)
        {
            _animator.SetFloat(vertical, verticalMove);
            _animator.SetFloat(horizontal, horizontalMove);
            return;
        }
        
        float v = Mathf.Abs(verticalMove);

        float h = Mathf.Abs(horizontalMove);


        if (_Input.MoveAmount > 0.1f)
        {
            if (isSprinting & _Input.MoveAmount > 0.1f)
            {
                v = Mathf.Clamp(v, 0, 2.0f)*2f;
                h = Mathf.Clamp(h, 0, 2.0f) * 2f;
            }
            else if (manager.WalkState & _Input.MoveAmount > 0.1f)
            {
                v = Mathf.Clamp(v, 0, 0.5f);
                h = Mathf.Clamp(h, 0, 0.5f);
            }
            else
            {
                v = Mathf.Clamp(v, 0, 1.0f);
                h= Mathf.Clamp(h, 0, 1.0f);
            }
        }

        
        if(v>h)
        { 
            _animator.SetFloat(vertical, v);
        }
        else
        {
            _animator.SetFloat(vertical, h);
        }
        //_animator.SetFloat(vertical, v);
        //_animator.SetFloat(horizontal, h);

        

    }
    public void PlayerTargetAnimation(string targetAnim,bool IsInteracting)
    {
        //Manager.Active_Animation(targetAnim, isInteracting);
        _animator.SetBool("IsInteracting", IsInteracting);
        
        _animator.CrossFade(targetAnim, 0.2f);

    }

    public bool CanRotate()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump"))
        {
            return false;
        }
        


        return true;

    }


    public bool PlayDodge(float Horizontal,float Vertical)
    {
        if(Mathf.Abs(Vertical)>=Mathf.Abs(Horizontal ))
        {
            if (Vertical > 0.1f)
            {
                PlayerTargetAnimation("Dodge_Front", true);
                return true;
            }
            else if (Vertical < -0.1f)
            {
                PlayerTargetAnimation("Dodge_Back", true);
                return true;
            }
        }
        else if (Mathf.Abs(Vertical) < Mathf.Abs(Horizontal))
        {
            if (Horizontal >= 0.1)
            {
                PlayerTargetAnimation("Dodge_Right", true);
                return true;
            }
            else if (Horizontal <= -0.1)
            {
                PlayerTargetAnimation("Dodge_Left", true);
                return true;
            }
        }

        PlayerTargetAnimation("Dodge_Back", true);
        return true;
        


    }

}
