using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Item_Enum;
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

            if(e.transform.GetChild(1).GetChild(0).TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI Sell_TEXT))
            Sell_TEXT.text= Sell_Item[i].Item_Name;
            if(e.transform.GetChild(3).TryGetComponent<Image>(out Image Temp_Image))
            Temp_Image.sprite=    Sell_Item[i].Item_Icon;
            if(e.transform.GetChild(4).GetChild(1).TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI Value_TEXT))
            Value_TEXT.text = Sell_Item[i].Value.ToString();
            int temp = i;
            e.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate { Buy_Item(temp); }) ;

        }
    } 


    public void Open_Vender_Item_Info(Item_Data ID)
    {
        Game_Master.instance.UI.Open_Small_Windows(ID);
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
            if(!eventData.pointerDrag.transform.TryGetComponent<Inventory_Slot>(out Inventory_Slot temp))
            {
                return;
            }

            if (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item == null)
            {
                return;
            }
            Current = temp;
            
            Open_Close_Sell_Window(true);
            Sell_item_Icon.sprite = Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Item_Icon;
            if(Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Type==EItem_Slot_Type.Use||
                Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Type == EItem_Slot_Type.ETC)
            {
                Sell_Value = (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Value * 7) / 10 * Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].count;

            }
            else
            {
                Sell_Value = (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Value * 7) / 10;
            }
            
            Sell_TEXT.text = Sell_Value.ToString();



        }

    }

    
    public void Open_Close_Sell_Window(bool state)
    {

        
        if (state == false)
        {
            Game_Master.instance.UI.Close_UI(Sell_Window);
            Current = null;
            Sell_Value = 0;
            return;
        }
        Game_Master.instance.UI.Open_UI(Sell_Window);
    }
    public void Agree_Sell()
    {
        Current.Empty_Slot();
        Current = null;

        Game_Master.instance.PM.Data.Current_Gold+=Sell_Value;
        Game_Master.instance.PM.Data_Save(true);
        Sell_Value = 0;
        Game_Master.instance.UI.Renewal_Gold_Text();



        Open_Close_Sell_Window(false);
        
    }
    public void Cancle_Sell()//버툰으로구현
    {
        Open_Close_Sell_Window(false);
    }
    public void Cancle_Buy()//버툰으로구현
    {
        Game_Master.instance.UI.Close_UI(Buy_Window);   
    }
    public void Agree_Buy()
    {
        if(Game_Master.instance.PM.Data.Current_Gold<Buy_Value)
        {
            Debug.Log("돈부족");
            return;
        }
        Item_Data Temp = new Item_Data();
        Temp.Base_item = Sell_Item[Selected_INDEX];
        Temp.Upgrade = 0;
        if (Sell_Item[Selected_INDEX].Type==EItem_Slot_Type.Use)
        {
            Temp.count = 1;
        }
        else
        {
            Temp.count = 0;
        }
        if (!Game_Master.instance.PM._manager_Inventory.Get_Item(Temp))
        {
            return;
        }

        Game_Master.instance.PM._manager_Inventory.Use_Money(Sell_Item[Selected_INDEX].Value);
        Game_Master.instance.UI.Renewal_Gold_Text();
        Game_Master.instance.PM.Data_Save(true);
        
        Game_Master.instance.UI.Close_UI(Buy_Window);
    }

}


