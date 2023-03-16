using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public class Upgrade_Slot_Vender : MonoBehaviour, IDropHandler
{
    [SerializeField]
    Upgrade_NPC _NPC;

    public int Current_INDEX;
    Slot Current;
    [SerializeField]
    Sprite base_Image;
    [SerializeField]
    Image Main_Image;
    [SerializeField]
    Item_Data Slot_IN_item;

    [SerializeField]
    TextMeshProUGUI Slot_Name;
    [SerializeField]
    TextMeshProUGUI Slot_Before_Upgrade_Point;
    [SerializeField]
    TextMeshProUGUI Slot_After_Upgrade_Point;
    [SerializeField]
    TextMeshProUGUI[] Before_Point;
    [SerializeField]
    TextMeshProUGUI[] After_Point;
    [SerializeField]
    TextMeshProUGUI Need_Gold;
    public int Need_Gold_Point;

    public int MAX_Upgrade = 10;
    private void OnEnable()
    {
        Current_INDEX = -1;
        Reset_Image();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Slot temp = eventData.pointerDrag.transform.GetComponent<Slot>();
            if (temp.Slot_IN_Item.Base_item == null)
            {
                return;
            }
            Slot_Into_item(temp);
            temp.UI.Reseting_Status();
        }

    }
    public void Slot_Into_item(Slot Current_Position)
    {
        if(Current_Position.Slot_IN_Item.Base_item.Type==Type.Use)
        {
            return;
        }
        if(Current!=null)
        {
            Current.Set_HIde(false);
        }
        _NPC._manager._Manager_Inventory.Load_on_Data(_NPC._manager.Call_Data());
        Current_INDEX = Current_Position.Slot_INDEX;
        Slot_IN_item = Current_Position.Slot_IN_Item;
        Current = Current_Position;
        Current_Position.Set_HIde(true);
        
        Current_Position.Image_Set(false);
        Renewal_Image();
    }
    public void Reset_Image()
    {
        Main_Image.sprite = base_Image;
        Slot_Name.text = "";
        Slot_Before_Upgrade_Point.text = "";
        Slot_After_Upgrade_Point.text = "";
        Need_Gold_Point = 0;
        Need_Gold.text = "X";
        for (int i = 0; i < Before_Point.Length; i++)
        {
            Before_Point[i].text = "";
            After_Point[i].text = "";
        }
    }

    public void Renewal_Image()
    {
        Main_Image.sprite = Slot_IN_item.Base_item.Item_Icon;
        Slot_Name.text = Slot_IN_item.Base_item.Item_Name;
        Slot_Before_Upgrade_Point.text = "+"+ Slot_IN_item.Upgrade;
        if (Slot_IN_item.Upgrade >= 10)
        {
            Slot_After_Upgrade_Point.text = "X";
        }
        else
        {
            Slot_After_Upgrade_Point.text = "+" + (Slot_IN_item.Upgrade + 1);
        }
        

        switch(Slot_IN_item.Base_item.Type)
        {
            case Type.Weapon:
                Weapon_Item temp_W = (Weapon_Item)Slot_IN_item.Base_item;
                Before_Point[0].text = "ATK: " + (temp_W.Attack_Point + (temp_W.UP_Attack_Point * Slot_IN_item.Upgrade));
                Before_Point[1].text = "CRP: " + (temp_W.CRP + (temp_W.UP_CRP * Slot_IN_item.Upgrade)) + "%";
                Before_Point[2].text = "CRT: " + (temp_W.CRT + (temp_W.UP_CRT * Slot_IN_item.Upgrade)) + "%";
                if (Slot_IN_item.Upgrade < MAX_Upgrade)
                {
                    After_Point[0].text = "ATK: " + (temp_W.Attack_Point + (temp_W.UP_Attack_Point * (Slot_IN_item.Upgrade + 1)));
                    After_Point[1].text = "CRP: " + (temp_W.CRP + (temp_W.UP_CRP * (Slot_IN_item.Upgrade + 1))) + "%";
                    After_Point[2].text = "CRT: " + (temp_W.CRT + (temp_W.UP_CRT * (Slot_IN_item.Upgrade + 1))) + "%";
                    Need_Gold_Point = (Slot_IN_item.Upgrade + 1) * 200;

                }
                else
                {
                    After_Point[0].text = "ATK: " + (temp_W.Attack_Point + (temp_W.UP_Attack_Point * Slot_IN_item.Upgrade));
                    After_Point[1].text = "CRP: " + (temp_W.CRP + (temp_W.UP_CRP * Slot_IN_item.Upgrade)) + "%";
                    After_Point[2].text = "CRT: " + (temp_W.CRT + (temp_W.UP_CRT * Slot_IN_item.Upgrade)) + "%";
                    Need_Gold_Point = 0;
                }


                break;

            case Type.Armor:
                Armor_Item temp_A = (Armor_Item)Slot_IN_item.Base_item;
                Before_Point[0].text = "ATK: " + (temp_A.Attack_Point + (temp_A.UP_Attack_Point * Slot_IN_item.Upgrade));
                Before_Point[1].text = "DEF: " + (temp_A.Armor_Point + (temp_A.UP_Armor_Point * Slot_IN_item.Upgrade));
                Before_Point[2].text = "HP : " + (temp_A.HP_Point + (temp_A.UP_HP_Point * Slot_IN_item.Upgrade));
                if (Slot_IN_item.Upgrade < MAX_Upgrade)
                {
                    After_Point[0].text = "ATK: " + (temp_A.Attack_Point + (temp_A.UP_Attack_Point * (Slot_IN_item.Upgrade + 1)));
                    After_Point[1].text = "DEF: " + (temp_A.Armor_Point + (temp_A.UP_Armor_Point * (Slot_IN_item.Upgrade + 1)));
                    After_Point[2].text = "HP : " + (temp_A.HP_Point + (temp_A.UP_HP_Point * (Slot_IN_item.Upgrade + 1)));
                    Need_Gold_Point = (Slot_IN_item.Upgrade + 1) * 100;

                }
                else
                {
                    After_Point[0].text = "ATK: " + (temp_A.Attack_Point + (temp_A.UP_Attack_Point * Slot_IN_item.Upgrade));
                    After_Point[1].text = "DEF: " + (temp_A.Armor_Point + (temp_A.UP_Armor_Point * Slot_IN_item.Upgrade));
                    After_Point[2].text = "HP : " + (temp_A.HP_Point + (temp_A.UP_HP_Point * Slot_IN_item.Upgrade));
                    Need_Gold_Point = 0;
                }
                break;
        }
        if(Need_Gold_Point==0)
        {
            Need_Gold.text = "X";
        }
        else
        {
            Need_Gold.text = Need_Gold_Point.ToString();
        }
    }

    public void Remove_Slot_Item()
    {
        if(Current!=null)
        Current.Set_HIde(false);
        Current = null;
        Current_INDEX = -1;
        Slot_IN_item = null;
        Reset_Image();
    }

    
}

