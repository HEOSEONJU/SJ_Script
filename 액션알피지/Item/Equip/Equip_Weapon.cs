using Item_Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Equip_Weapon : Equip_Slot, IPointerEnterHandler, IDragHandler, IDropHandler
{
    public override void  Setting_Slot(int i=0)
    {
        base.Setting_Slot(i);
        
        if (Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item == null)
        {
            Color Temp_Color = Color.white;
            Temp_Color.a = 0;
            Slot_Item_Image.color = Temp_Color;
        }
        else
        {
            Slot_Item_Image.color = Color.white;
            Slot_Item_Image.sprite = Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item.Item_Icon;
        }
        
        
    }

    protected override  void Image_Set()//이미지 true면 투명도해제 false면 투명하게
    {
        Color temp = Color.white;
        temp.a = 1;
        if (Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item == null)
        {
            Slot_Item_Image.sprite = Base_Image;
            Activation = false;
        }
        else
        {
            Slot_Item_Image.sprite = Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item.Item_Icon;
            Activation = true;
        }


        Slot_Item_Image.color = temp;


    }

    public void Init_Slot_item()
    {
        Slot_Item_Image.sprite = Base_Image;
        Image_Set();
        Game_Master.instance.PM._weaponSlotManager.Change_Weapon_Prefab();
    }
    public new  void OnPointerEnter(PointerEventData eventData)
    {

        
        if ((Activation && Game_Master.instance.UI.Windows == false) && Game_Master.instance.UI.Draging == false)
        {

            Game_Master.instance.UI.Open_Small_Windows(Game_Master.instance.PM.Data.Equip_Weapon_Item);

        }


    }
    public new void OnDrag(PointerEventData eventData)
    {
        if (Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item == null)
        {
            return;
        }

        rect.position = eventData.position;
    }



    public new void OnDrop(PointerEventData eventData)//인벤토리에서 장비칸으로이동시 작동
    {

        if (eventData.pointerDrag != null)
        {


            eventData.pointerDrag.transform.TryGetComponent<Inventory_Slot>(out Inventory_Slot temp);//들고있던것  작동하는건 도착지점

            
            Debug.Log(Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Type + "/" + this.TYPE);
            if (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item == null)
            {
                
                return;
            }

            if (!Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Type.HasFlag(this.TYPE))
            {
                
                return;
            }

            // Debug.Log("들어갈 수 있는 칸입니다");
            
            Game_Master.instance.PM.Data.Swap_Items_ItoW(temp.Slot_INDEX);//ETC->Weapon
            
            Game_Master.instance.PM._weaponSlotManager.Change_Weapon_Prefab();
            
            Game_Master.instance.PM._manager_Inventory.Load_on_Data(Game_Master.instance.PM.Data);
            
            Game_Master.instance.UI.Reseting_Status();
        }


    }



}
