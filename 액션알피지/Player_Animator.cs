using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator : MonoBehaviour
{
    
    
    
    public Animator _animator;
    [SerializeField]
    Player_Input _Input;
   
    int vertical;
    int horizontal;

    
    public void Init(Player_Input IP)
    {
        _Input = IP;
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    public void UpdataAnimation(float verticalMove, float horizontalMove, bool isSprinting)
    {

        if(Game_Master.instance.PM.LockOnMode)
        {
            _animator.SetFloat(vertical, verticalMove);
            _animator.SetFloat(horizontal, horizontalMove);
            return;
        }
        
        float v = Mathf.Abs(verticalMove);

        float h = Mathf.Abs(horizontalMove);


        if (_Input._moveAmount > 0.1f)
        {
            if (isSprinting & _Input._moveAmount > 0.1f)
            {
                v = Mathf.Clamp(v, 0, 2.0f)*2f;
                h = Mathf.Clamp(h, 0, 2.0f) * 2f;
            }
            else if (Game_Master.instance.PM._isWalkState & _Input._moveAmount > 0.1f)
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
