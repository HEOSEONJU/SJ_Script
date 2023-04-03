using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Weapon_Slot_Manager : MonoBehaviour
{
    
    
    public Weapon_Slot Main_Weapon;
    public Weapon_Slot Sub_Weapon;
    [SerializeField] Player_Animaotr_Controller _Attack_Animator;
    public void Init()
    {
        
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
                
                Main_Weapon.Init((Weapon_Item)Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item);




                AbleWeaponCollider(false);
            }
        }


        if(Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item != null)
        {
            Change_Weapon_Prefab();
        }

    }

    public void Change_Weapon_Prefab()
    {

        if (Game_Master.instance.PM.Data.Equip_Weapon_Item == null || Main_Weapon==null)
        {
            return;
        }

        
        if(Game_Master.instance.PM.Data.Equip_Weapon_Item!=null)
        {
            Weapon_Item temp = (Weapon_Item)Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item;
            
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
