using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Interaction_Function : MonoBehaviour
{
    float Dis;
    public float Distance
    {
        get
        {
            return Dis;
        }
        set
        {
            Dis = value;
        }
    }
    public int Object_ID
    {
        get
        {
            return Get_ID();
        }
    }
    [SerializeField]
    bool State_IF=false;
    public bool Current_State
    {
        get { return State_IF; } 
        set { State_IF = Setting_IF(value); }
    }
    public virtual bool Setting_IF(bool v)
    {
        return v;
    }
    public virtual int Get_ID()
    {
        return gameObject.GetInstanceID();
    }
    
    public virtual void Connect_Object(Player_Manager _Manager)//접촉
    {
        return;
    }
    public virtual void DisConnect_Object()//접촉해제
    {
        return;
    }
    public virtual void Function(bool State)
    {

        
        if (State)
        {
            Current_State = !Current_State;
        }
        else
        {
            return;

        }


        if (Current_State)
        {
            Open_Window();
        }
        else
        {
            Close_Window();
            
        }

    }
    public virtual void Open_Window()//기능 활성화
    {
        return;
    }
    public virtual void Close_Window()// 기능종료
    {
       
        return;
    }

}
