using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Bullet : MonoBehaviour
{

    public bool Armor;

    public abstract void Damaged(float a);    
}
