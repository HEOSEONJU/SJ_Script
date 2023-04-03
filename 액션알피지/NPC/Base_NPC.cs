using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_NPC : MonoBehaviour
{

    [SerializeField]
    protected int NPC_ID;
    public abstract int ID
    {
         get;
    }

    protected Transform MY_Position;
    protected Transform Connect_Player;
    [SerializeField]
    protected GameObject UI;
    
    [SerializeField]
    public Player_Manager _manager;

    public float distance;
    public abstract void Init();
    public abstract bool Check_UI();



    public abstract void Connecting_NPC(Player_Manager MANAGER);
    public abstract void DisConnecting_NPC();
    public abstract void Open_Window();
    public abstract void Close_Window();
    public abstract void Close_ALL_Window();


}
