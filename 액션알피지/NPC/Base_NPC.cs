using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_NPC : MonoBehaviour
{
    public int Object_ID;
    protected Transform MY_Position;
    protected Transform Connect_Player;
    [SerializeField]
    protected GameObject UI;
    
    [SerializeField]
    public Player_Manager _manager;

    public float distance;

    public abstract void Get_ID();

    public abstract void Connecting_NPC(Player_Manager MANAGER);
    public abstract void DisConnecting_NPC();
    public abstract void Open_Window();
    public abstract void Close_Window();


}
