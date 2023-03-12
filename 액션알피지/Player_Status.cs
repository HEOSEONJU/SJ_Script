using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Status : MonoBehaviour
{
    public int Level=1;


    public int MAX_HP=1000;
    public int Current_HP=10;

    public int ATK_Point;
    public int DEF_Point;
    public int CRP_Point;
    public int CRT_Point;



    Player_Inventory _inventroy;

    public void init()
    {
        _inventroy=GetComponent<Player_Inventory>();
        Cal_All();
        Current_HP = MAX_HP;
    }


    public void Heal(int T)
    {
        Current_HP += T;
        if(Current_HP>MAX_HP)
        {
            Current_HP = MAX_HP;
        }
    }

    public bool Damaged_HP(int T)//죽으면 트루반환
    {
        float Block_Point = DEF_Point / (DEF_Point + 100f);
        float Damage_Point = T * (1-Block_Point);
        int INT_DAMGE = (int)Damage_Point;

        





        Current_HP -= INT_DAMGE;
        if(Current_HP<=0)
        {
            return true;
        }
        return false;
        
    }

    public void Cal_All()
    {
        Cal_HP_MAX();
        
        Cal_ATK();
        Cal_DEF();
        Cal_CRP();
        Cal_CRT();

    }



    public void Cal_HP_MAX()
    {
        int Base = 1000;
        Armor_Item Armor_temp;
        for (int i = 0; i < 3; i++)
        {
            if (_inventroy.Equip_Armors[i].Slot_IN_Item.Base_item == null)
            {
                continue;
            }
            Armor_temp = (Armor_Item)_inventroy.Equip_Armors[i].Slot_IN_Item.Base_item;
            Base += Armor_temp.HP_Point+(_inventroy.Equip_Armors[i].Slot_IN_Item.Upgrade*Armor_temp.UP_HP_Point);
        }
        MAX_HP = Base;
    }




    public void Cal_ATK()
    {
        int Base = 0;
        if(_inventroy.Main_Weapon.Slot_IN_Item.Base_item!=null)
        {
            Weapon_Item Weapon_temp = (Weapon_Item)_inventroy.Main_Weapon.Slot_IN_Item.Base_item;

            Base+=Weapon_temp.Attack_Point+(_inventroy.Main_Weapon.Slot_IN_Item.Upgrade* Weapon_temp.UP_Attack_Point);
        }
        Armor_Item Armor_temp;
        for (int i=0;i<3;i++)
        {
            if(_inventroy.Equip_Armors[i].Slot_IN_Item.Base_item == null)
            {
                continue;
            }
            Armor_temp = (Armor_Item)_inventroy.Equip_Armors[i].Slot_IN_Item.Base_item;
            Base += Armor_temp.Attack_Point+(_inventroy.Equip_Armors[i].Slot_IN_Item.Upgrade * Armor_temp.UP_Attack_Point);
        }
        ATK_Point = Base;

    }
    public void Cal_DEF()
    {
        int Base = 0;
        Armor_Item Armor_temp;
        for (int i = 0; i < 3; i++)
        {
            if (_inventroy.Equip_Armors[i].Slot_IN_Item.Base_item == null)
            {
                continue;
            }
            Armor_temp = (Armor_Item)_inventroy.Equip_Armors[i].Slot_IN_Item.Base_item;
            Base += Armor_temp.Armor_Point + (_inventroy.Equip_Armors[i].Slot_IN_Item.Upgrade * Armor_temp.Armor_Point);
        }
        DEF_Point = Base;
    }
    public void Cal_CRP()
    {
        int Base = 5;
        if (_inventroy.Main_Weapon.Slot_IN_Item.Base_item != null)
        {
            Weapon_Item Weapon_temp = (Weapon_Item)_inventroy.Main_Weapon.Slot_IN_Item.Base_item;

            Base += Weapon_temp.CRP + (_inventroy.Main_Weapon.Slot_IN_Item.Upgrade * Weapon_temp.UP_CRP);
        }
        CRP_Point = Base;
    }
    public void Cal_CRT()
    {
        int Base = 50;
        if (_inventroy.Main_Weapon.Slot_IN_Item.Base_item != null)
        {
            Weapon_Item Weapon_temp = (Weapon_Item)_inventroy.Main_Weapon.Slot_IN_Item.Base_item;

            Base += Weapon_temp.CRT + (_inventroy.Main_Weapon.Slot_IN_Item.Upgrade * Weapon_temp.UP_CRT);
        }
        CRT_Point = Base;
    }
    
}
