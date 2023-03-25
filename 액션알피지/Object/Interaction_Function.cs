using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction_Function : MonoBehaviour
{
    public float Distance
    {
        get
        {
            return Distance;
        }
        set
        {
            Distance = value;
        }
    }
    public int Object_ID
    {
        get
        {
            return Get_ID();
        }
    }
    public abstract int Get_ID();
    public abstract void Connect_Object(Player_Manager _Manager);
    public abstract void DisConnect_Object();

    public abstract void Open_Window();
    public abstract void Close_Window();

}
