using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class Vender_Slot : Base_Slot, IPointerEnterHandler, IPointerExitHandler
{
    Item_Data ITEM;
    public void Init(item ID)
    {
        ITEM=new Item_Data();
        ITEM.Base_item = ID;
        ITEM.Upgrade = 0;
        ITEM.count = 0;
    }
    protected override void Image_Renewal()
    {
        return;

    }
    public override void Insert_Slot_Item(int INDEX)
    {
        return;

    }
    public override void Setting_Slot(int i = 0)
    {
        return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if ((Game_Master.Instance.Call_UI().Windows == false) && Game_Master.Instance.Call_UI().Draging == false)
        {
            Game_Master.Instance.Call_UI().Active_Windows(true, ITEM);

        }


    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Game_Master.Instance.Call_UI().Windows == true)
        {
            Game_Master.Instance.Call_UI().Active_Windows(false);
        }


    }
}
