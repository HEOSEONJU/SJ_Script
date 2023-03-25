using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Vender_Function : MonoBehaviour, IDropHandler
{
    Vender_NPC Vender;
    [SerializeField]
    List<item> Sell_Item;
    [SerializeField]
    GameObject Sell_Icon;//판매할아이템
    
    public GameObject Sell_Window;
    [SerializeField]
    Image Sell_item_Icon;
    [SerializeField]
    TextMeshProUGUI Sell_TEXT;
    [SerializeField]
    Inventory_Slot Current;


    
    public GameObject Buy_Window;
    [SerializeField]
    Image Buy_item_Icon;
    [SerializeField]
    TextMeshProUGUI Buy_TEXT;

    bool Selling = false;
    int Sell_Value;
    int Buy_Value;
    int Selected_INDEX;

    
    public void Init(Vender_NPC _NPC)
    {
        Vender = _NPC;
        
        for (int i = 0; i < Sell_Item.Count; i++)
        {
            var e = Instantiate(Sell_Icon, this.transform.GetChild(0));
            e.GetComponent<Vender_Slot>().Init(Sell_Item[i]);
            e.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Sell_Item[i].Item_Name;
            e.transform.GetChild(3).GetComponent<Image>().sprite = Sell_Item[i].Item_Icon;
            e.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Sell_Item[i].Value.ToString();
            int temp = i;
            e.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { Buy_Item(temp); }) ;

        }
    } 


    public void Open_Vender_Item_Info(Item_Data ID)
    {
        Vender._manager._Manager_Inventory.UI.Active_Windows(true,ID);
    }


    public void Buy_Item(int n)
    {
        Buy_Window.SetActive(true);
        Selected_INDEX = n;
        Buy_item_Icon.sprite = Sell_Item[n].Item_Icon;
        Buy_Value = Sell_Item[n].Value;
        Buy_TEXT.text = Buy_Value.ToString();

    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("드랍");
        if (eventData.pointerDrag != null)
        {
            
            Inventory_Slot temp = eventData.pointerDrag.transform.GetComponent<Inventory_Slot>();
            if (temp == null)
            {
                return;
            }
            
            if (Game_Master.instance.Call_Player().Call_Data().Items[temp.Slot_INDEX].Base_item == null)
            {
                return;
            }
            Current = temp;
            
            Open_Close_Sell_Window(true);
            Sell_item_Icon.sprite = Game_Master.instance.Call_Player().Call_Data().Items[temp.Slot_INDEX].Base_item.Item_Icon;
            switch(Game_Master.instance.Call_Player().Call_Data().Items[temp.Slot_INDEX].Base_item.Type)
            {
                case Type.Use:
                    Sell_Value = (Game_Master.instance.Call_Player().Call_Data().Items[temp.Slot_INDEX].Base_item.Value * 7) / 10 * Game_Master.instance.Call_Player().Call_Data().Items[temp.Slot_INDEX].count;
                    break;
                default:
                    Sell_Value = (Game_Master.instance.Call_Player().Call_Data().Items[temp.Slot_INDEX].Base_item.Value * 7) / 10;
                    
                    break;
            }
            Sell_TEXT.text = Sell_Value.ToString();



        }

    }

    
    public void Open_Close_Sell_Window(bool state)
    {

        Sell_Window.SetActive(state);
        if (state == false)
        {
            Current = null;
            
            Sell_Value = 0;
        }
    }
    public void Agree_Sell()
    {

        Current.Empty_Slot();
        Current = null;

        Vender._manager._Manager_Inventory.Get_Money(Sell_Value);
        Vender._manager.Data_Save(true);
        Sell_Value = 0;
        Vender._manager._UI.Renewal_Gold_Text();


        Open_Close_Sell_Window(false);
    }
    public void Cancle_Sell()
    {

        Open_Close_Sell_Window(false);
    }
    public void Cancle_Buy()
    {

        Buy_Window.SetActive(false);
    }
    public void Agree_Buy()
    {

        if(Vender._manager._Manager_Inventory.Current_Gold()<Buy_Value)
        {

            Debug.Log("돈부족");
            return;
        }
        Item_Data Temp = new Item_Data();
        Temp.Base_item = Sell_Item[Selected_INDEX];
        Temp.Upgrade = 0;
        if (Sell_Item[Selected_INDEX].Type==Type.Use)
        {
            Temp.count = 1;
            if (!Vender._manager._Manager_Inventory.Get_Item(Temp))
            {
                return;
            }
        }
        else
        {
            Temp.count = 0;
            if (!Vender._manager._Manager_Inventory.Get_Item(Temp))
            {
                return;
            }

        }

        Vender._manager._Manager_Inventory.Use_Money(Sell_Item[Selected_INDEX].Value);
        Vender._manager._UI.Renewal_Gold_Text();
        Vender._manager.Data_Save(true);
        Buy_Window.SetActive(false);
    }

}


