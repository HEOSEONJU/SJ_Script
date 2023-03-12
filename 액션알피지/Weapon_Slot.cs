using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Slot : MonoBehaviour
{
    public bool Right;
    public bool Left;

    //public Weapon_Item Weapon_Item;
    public Transform HandPosition;

    public Player_Weapon_Collider Weapon_Colldier;

    public GameObject Current_Weapon;


    public void Init(Weapon_Item Main_Weapon)
    {
        HandPosition = this.transform;
        Change_Weapon(Main_Weapon);
    }
    

    public void Setting(Weapon_Item Info)
    {

        //Weapon_Item = Info;



    }

    public void Destroy_Current_Weapon()
    {
        Destroy(Current_Weapon);
    }
    public void Change_Weapon(Weapon_Item temp)
    {
        if(Current_Weapon !=null)
        {
            Destroy(Current_Weapon);
        }

        if (temp != null)
        {
            Current_Weapon = Instantiate(temp.WeaponPrefab, HandPosition);
            Current_Weapon.transform.GetChild(0).GetChild(0).tag = "Weapon";
            Weapon_Colldier = Current_Weapon.GetComponentInChildren<Player_Weapon_Collider>();
        }
        

    }

    public void ableWeaponCollider(bool able)
    {
        if (Weapon_Colldier != null)
        {
            Weapon_Colldier.AbleCollider(able);
        }
    }



}
