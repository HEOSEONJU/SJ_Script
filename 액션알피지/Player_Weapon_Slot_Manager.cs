using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Weapon_Slot_Manager : MonoBehaviour
{
    public Player_Inventory Inventory;
    
    public Weapon_Slot Main_Weapon;
    public Weapon_Slot Sub_Weapon;
    [SerializeField] Player_Animaotr_Controller _Attack_Animator;
    public void Init(Player_Inventory Manager_Inventory)
    {
        Inventory= Manager_Inventory;
        Weapon_Slot[] weapon_Slot = GetComponentsInChildren<Weapon_Slot>();

        foreach (Weapon_Slot weaponHolderSlot in weapon_Slot)
        {
            if (weaponHolderSlot.Left)
            {
                Sub_Weapon = weaponHolderSlot;
                //Sub_Weapon.Init(Inventory.Main_Weapon.weapon);
                AbleWeaponCollider(false);
            }
            else if (weaponHolderSlot.Right)
            {
                Main_Weapon = weaponHolderSlot;
                Main_Weapon.Init((Weapon_Item)Inventory.Main_Weapon.Slot_IN_Item.Base_item);




                AbleWeaponCollider(false);
            }
        }


        if(Manager_Inventory.Main_Weapon != null)
        {
            Change_Weapon_Prefab();
        }

    }

    public void Change_Weapon_Prefab()
    {

        if (Inventory == null)
        {
            return;
        }

        
        if(Inventory.Main_Weapon.Slot_IN_Item.Base_item!=null)
        {
            Weapon_Item temp = (Weapon_Item)Inventory.Main_Weapon.Slot_IN_Item.Base_item;
            Main_Weapon.Change_Weapon(temp);
            _Attack_Animator.Change_Weapon_Type(temp.Attack_List, temp.Air_Attack_List);
        }
        else
        {
            Main_Weapon.Destroy_Current_Weapon();
            _Attack_Animator.Change_Weapon_Type();
        }
        
        

           

        
    }



    public void AbleWeaponCollider(bool able,bool Twin_weapon=false)
    {
        if(Twin_weapon)
        {
            
            Main_Weapon.ableWeaponCollider(able);
            Sub_Weapon.ableWeaponCollider(able);
        }
        else
        {
            Main_Weapon.ableWeaponCollider(able);

        }


    }

}
