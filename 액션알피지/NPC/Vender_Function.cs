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
    [SerializeField]
    GameObject Sell_Window;
    [SerializeField]
    Image Sell_item_Icon;
    [SerializeField]
    TextMeshProUGUI Sell_TEXT;
    [SerializeField]
    Slot Current;
    



    bool Selling= false;
    int Sell_Value;

    public void Init(Vender_NPC _NPC)
    {
        Vender = _NPC;
        for (int i=0;i<Sell_Item.Count;i++)
        {
            var e=Instantiate(Sell_Icon, this.transform.GetChild(0));
            e.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Sell_Item[i].Item_Name;
            e.transform.GetChild(3).GetComponent<Image>().sprite = Sell_Item[i].Item_Icon;
            e.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = Sell_Item[i].Value.ToString();
        }


    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("드랍");
        if (eventData.pointerDrag != null)
        {
            
            Slot temp = eventData.pointerDrag.transform.GetComponent<Slot>();
            if (temp.Slot_IN_Item.Base_item == null)
            {
                return;
            }
            Current = temp;
            
            Open_Close_Sell_Window(true);
            Sell_item_Icon.sprite = temp.Slot_IN_Item.Base_item.Item_Icon;
            Sell_Value= (temp.Slot_IN_Item.Base_item.Value * 10) / 7;
            Sell_TEXT.text= Sell_Value.ToString();



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
        
        Current.Delete_Slot_Item();
        Current = null;

        Vender._manager._Manager_Inventory.Get_Money(Sell_Value);
        Sell_Value = 0;
        Vender._manager._UI.Renewal_Gold_Text();


        Open_Close_Sell_Window(false);
    }
    public void Cancle_Sell()
    {

        Open_Close_Sell_Window(false);
    }

}


