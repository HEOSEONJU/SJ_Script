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
            if (weaponHolderSlot._isLeft)
            {
                Sub_Weapon = weaponHolderSlot;
                //Sub_Weapon.Init(Inventory.Main_Weapon.weapon);
                AbleWeaponCollider(false);
            }
            else if (weaponHolderSlot._isRight)
            {
                Main_Weapon = weaponHolderSlot;
                
                Main_Weapon.Init(Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item as Weapon_Item);




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
        
        
        if(Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item!=null)
        {
            Weapon_Item temp = Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item as Weapon_Item;
            
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
