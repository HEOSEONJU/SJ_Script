using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item/Item")]
public class item:ScriptableObject
{
    public string Item_Name;//�������̸�
    public int Item_Index;//������ ID
    public Sprite Item_Icon;//�����۾�����
    public string exp;//����

    public int Value;
    public Type Type;
    
    

    
}


public enum Type
{
    Weapon, Armor, Use
}