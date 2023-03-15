using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Manager : MonoBehaviour
{

    public Player_Manager _Manager;


    Vector3 Mouse_Position;
    public bool Windows;

    public bool Draging;
    [SerializeField]
    Transform Windows_Object;
    [SerializeField]
    UI_Information INFO;
    
    public TextMeshProUGUI Status_Text_Level;
    public TextMeshProUGUI Status_Text_ATK;
    public TextMeshProUGUI Status_Text_DEF;
    public TextMeshProUGUI Status_Text_HP;
    public TextMeshProUGUI Status_Text_CRP;
    public TextMeshProUGUI Status_Text_CRT;
    public TextMeshProUGUI Gold_Text;



    CanvasGroup Canvas_Group;


    public void Init(Player_Manager Manager)
    {
        _Manager=Manager;
        Canvas_Group=GetComponent<CanvasGroup>();
        Slot[] temp= GetComponentsInChildren<Slot>();
        int INDEX = 0;
        foreach(Slot slot in temp)
        {
            slot.Setting_Slot(this, INDEX++);
        }
        Windows_Object.gameObject.SetActive(false);
    }

    public bool Use_Heal_item(Use_Item temp)
    {
        if(_Manager._Status.Current_HP==_Manager._Status.MAX_HP)
        {
            return false;
        }
        
        _Manager._Status.Heal(temp.Point);
        Status_Text_HP.text = _Manager._Status.Current_HP.ToString() + "/" + _Manager._Status.MAX_HP.ToString();
        return true;
    }

    public void Reseting_Status()
    {
        _Manager._Status.Cal_All();
        Status_Text_Level.text = _Manager._Status.Level.ToString();
        Status_Text_ATK.text = _Manager._Status.ATK_Point.ToString();
        Status_Text_DEF.text = _Manager._Status.DEF_Point.ToString();
        Status_Text_HP.text = _Manager._Status.Current_HP.ToString() + "/"+_Manager._Status.MAX_HP.ToString();
        Status_Text_CRP.text = _Manager._Status.CRP_Point.ToString()+"%";
        Status_Text_CRT.text = _Manager._Status.CRT_Point.ToString()+"%";

    }


    public void Active_Windows(bool state,Slot Item_temp=null)
    {
        
        Windows_Object.gameObject.SetActive(state);
        if(Item_temp == null)
        {
            return;
        }
        if(Item_temp.Slot_IN_Item.Base_item!=null)
        Renewal_Text(Item_temp);
        




    }
    public void Renewal_Text(Slot Item_temp)
    {
        
        INFO.Name.text = Item_temp.Slot_IN_Item.Base_item.Item_Name;

        switch(Item_temp.Slot_IN_Item.Base_item.Type)
        {
            case Type.Use:
                INFO.Upgarde.text = "";
                INFO.Type.text = "Use";
                Use_Item temp_U = (Use_Item)Item_temp.Slot_IN_Item.Base_item;
                INFO.Header[0].text = "Heal :";
                INFO.Point[0].text = temp_U.Point.ToString();
                INFO.Header[1].text = "Count:";
                INFO.Point[1].text = Item_temp.Slot_IN_Item.count.ToString();
                for (int i = 2; i < INFO.Header.Count; i++)
                {
                    INFO.Header[i].text = "";
                    INFO.Point[i].text = "";
                }
                break;
            case Type.Weapon:
                INFO.Type.text = "Weapon";
                Weapon_Item temp_W = (Weapon_Item)Item_temp.Slot_IN_Item.Base_item;
                INFO.Upgarde.text = "+" + Item_temp.Slot_IN_Item.Upgrade;
                INFO.Header[0].text = "ATK :";
                INFO.Point[0].text = (temp_W.Attack_Point + (temp_W.UP_Attack_Point * Item_temp.Slot_IN_Item.Upgrade)).ToString();
                INFO.Header[1].text = "CRP :";
                INFO.Point[1].text = (temp_W.CRP + (temp_W.CRP * Item_temp.Slot_IN_Item.Upgrade)).ToString() + "%";
                INFO.Header[2].text = "CRT :";
                INFO.Point[2].text = (temp_W.CRT + (temp_W.CRT * Item_temp.Slot_IN_Item.Upgrade)).ToString() + "%";
                for (int i = 3; i < INFO.Header.Count; i++)
                {
                    INFO.Header[i].text = "";
                    INFO.Point[i].text = "";
                }

                break;
            case Type.Armor:
                INFO.Type.text = "Armor";
                Armor_Item temp_A = (Armor_Item)Item_temp.Slot_IN_Item.Base_item;
                INFO.Upgarde.text = "+" + Item_temp.Slot_IN_Item.Upgrade;
                INFO.Header[0].text = "ATK :";
                INFO.Point[0].text = (temp_A.Attack_Point + (temp_A.UP_Attack_Point * Item_temp.Slot_IN_Item.Upgrade)).ToString();
                INFO.Header[1].text = "DEF :";
                INFO.Point[1].text = (temp_A.Armor_Point + (temp_A.UP_Armor_Point * Item_temp.Slot_IN_Item.Upgrade)).ToString();
                INFO.Header[2].text = "HP  :";
                INFO.Point[2].text = (temp_A.HP_Point + (temp_A.UP_HP_Point * Item_temp.Slot_IN_Item.Upgrade)).ToString();
                for (int i = 3; i < INFO.Header.Count; i++)
                {
                    INFO.Header[i].text = "";
                    INFO.Point[i].text = "";
                }

                break;
        }
       
    }

    public void Renewal_Gold_Text()
    {
        Gold_Text.text = _Manager._Manager_Inventory.Current_Gold().ToString();
    }



}
