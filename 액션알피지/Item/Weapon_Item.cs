using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item/Weapon_Item")]
public class Weapon_Item : item
{
    public List<string> Attack_List;
    public List<string> Air_Attack_List;
    public GameObject WeaponPrefab;

    
    public int Attack_Point;
    public int CRP;
    public int CRT;
    
    public int UP_Attack_Point;
    
    public int UP_CRP;

    public int UP_CRT;

}
