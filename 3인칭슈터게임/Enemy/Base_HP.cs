using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_HP : MonoBehaviour
{
    public bool Live;
    public int hp;
    public int ID;
    public bool Armor;

    public abstract void Damged(int a,bool special=false);
    
}
