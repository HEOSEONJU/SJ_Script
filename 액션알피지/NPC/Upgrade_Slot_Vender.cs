using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Item_Enum;

public class Upgrade_Slot_Vender : MonoBehaviour, IDropHandler
{
    [SerializeField]
    Upgrade_NPC _NPC;

    public int Current_INDEX;
    Inventory_Slot Current;
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

    public object Equip_Slot_Type { get; private set; }

    private void OnEnable()
    {
        Current_INDEX = -1;
        Reset_Image();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (!eventData.pointerDrag.transform.TryGetComponent<Inventory_Slot>(out Inventory_Slot temp))
            {
                return;
            }

            if (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item == null)
            {
                return;
            }

            Slot_Into_item(temp);
            Game_Master.instance.UI.Reseting_Status();
        }

    }
    public void Slot_Into_item(Inventory_Slot Current_Position)
    {
        if(Game_Master.instance.PM.Data.Items[Current_Position.Slot_INDEX].Base_item.Type==EItem_Slot_Type.Use||
            Game_Master.instance.PM.Data.Items[Current_Position.Slot_INDEX].Base_item.Type == EItem_Slot_Type.ETC)
        {
            return;
        }
        if(Current!=null)
        {
            Current.Set_HIde(false);
        }
        _NPC._manager._manager_Inventory.Load_on_Data(Game_Master.instance.PM.Data);
        Current_INDEX = Current_Position.Slot_INDEX;
        Slot_IN_item = Game_Master.instance.PM.Data.Items[Current_Position.Slot_INDEX];
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
        Slot_Before_Upgrade_Point.text = "+" + Slot_IN_item.Upgrade;
        if (Slot_IN_item.Upgrade >= 10)
        {
            Slot_After_Upgrade_Point.text = "X";
        }
        else
        {
            Slot_After_Upgrade_Point.text = "+" + (Slot_IN_item.Upgrade + 1);
        }

        

        if (Slot_IN_item.Base_item.Type == EItem_Slot_Type.Weapon)
        {
            Weapon_Item temp_W = Slot_IN_item.Base_item as Weapon_Item;
            Before_Point[0].text = "공격력: " + (temp_W.Attack_Point + (temp_W.UP_Attack_Point * Slot_IN_item.Upgrade));
            Before_Point[1].text = "치명타확률: " + (temp_W.CRP + (temp_W.UP_CRP * Slot_IN_item.Upgrade)) + "%";
            Before_Point[2].text = "치명타데미지: " + (temp_W.CRT + (temp_W.UP_CRT * Slot_IN_item.Upgrade)) + "%";
            if (Slot_IN_item.Upgrade < MAX_Upgrade)
            {
                After_Point[0].text = "공격력: " + (temp_W.Attack_Point + (temp_W.UP_Attack_Point * (Slot_IN_item.Upgrade + 1)));
                After_Point[1].text = "치명타확률: " + (temp_W.CRP + (temp_W.UP_CRP * (Slot_IN_item.Upgrade + 1))) + "%";
                After_Point[2].text = "치명타데미지: " + (temp_W.CRT + (temp_W.UP_CRT * (Slot_IN_item.Upgrade + 1))) + "%";
                Need_Gold_Point = (Slot_IN_item.Upgrade + 1) * 200;

            }
            else
            {
                After_Point[0].text = "공격력: " + (temp_W.Attack_Point + (temp_W.UP_Attack_Point * Slot_IN_item.Upgrade));
                After_Point[1].text = "치명타확률: " + (temp_W.CRP + (temp_W.UP_CRP * Slot_IN_item.Upgrade)) + "%";
                After_Point[2].text = "치명타데미지: " + (temp_W.CRT + (temp_W.UP_CRT * Slot_IN_item.Upgrade)) + "%";
                Need_Gold_Point = 0;
            }


        }
        else if (Slot_IN_item.Base_item.Type == EItem_Slot_Type.Helmet ||
            Slot_IN_item.Base_item.Type== EItem_Slot_Type.Glove ||
            Slot_IN_item.Base_item.Type== EItem_Slot_Type.Breastplate)
        {

            Armor_Item temp_A = Slot_IN_item.Base_item as Armor_Item;
            Before_Point[0].text = "공격력: " + (temp_A.Attack_Point + (temp_A.UP_Attack_Point * Slot_IN_item.Upgrade));
            Before_Point[1].text = "방어력: " + (temp_A.Armor_Point + (temp_A.UP_Armor_Point * Slot_IN_item.Upgrade));
            Before_Point[2].text = "체력 : " + (temp_A.HP_Point + (temp_A.UP_HP_Point * Slot_IN_item.Upgrade));
            if (Slot_IN_item.Upgrade < MAX_Upgrade)
            {
                After_Point[0].text = "공격력: " + (temp_A.Attack_Point + (temp_A.UP_Attack_Point * (Slot_IN_item.Upgrade + 1)));
                After_Point[1].text = "방어력: " + (temp_A.Armor_Point + (temp_A.UP_Armor_Point * (Slot_IN_item.Upgrade + 1)));
                After_Point[2].text = "체력 : " + (temp_A.HP_Point + (temp_A.UP_HP_Point * (Slot_IN_item.Upgrade + 1)));
                Need_Gold_Point = (Slot_IN_item.Upgrade + 1) * 100;

            }
            else
            {
                After_Point[0].text = "공격력: " + (temp_A.Attack_Point + (temp_A.UP_Attack_Point * Slot_IN_item.Upgrade));
                After_Point[1].text = "방어력: " + (temp_A.Armor_Point + (temp_A.UP_Armor_Point * Slot_IN_item.Upgrade));
                After_Point[2].text = "체력 : " + (temp_A.HP_Point + (temp_A.UP_HP_Point * Slot_IN_item.Upgrade));
                Need_Gold_Point = 0;
            }

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

