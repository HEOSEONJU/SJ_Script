using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip_Weapon : Equip_Slot
{

    [SerializeField]
    Player_Weapon_Slot_Manager _Manager;
    //public Weapon_Item weapon;


    public override void  Setting_Slot(UI_Manager Manager,int i=0)
    {


        Slot_Item_Image = GetComponent<Image>();
        if (Slot_IN_Item.Base_item == null)
        {
            Color Temp_Color = Color.white;
            Temp_Color.a = 0;
            Slot_Item_Image.color = Temp_Color;

        }
        else
        {
            Slot_Item_Image.color = Color.white;
            Slot_Item_Image.sprite = Slot_IN_Item.Base_item.Item_Icon;
        }
        Slot_Position = transform.parent;
        rect = GetComponentInParent<RectTransform>();
        Origin = rect.position;


        Canvas_Group = gameObject.AddComponent<CanvasGroup>();

        UI = Manager;
        _Inventory = Manager._Manager._Manager_Inventory;
        _Manager = Manager._Manager._WeaponSlotManager;

    }

    /*
    public override void Insert_Slot_Item(Slot Current)
    {

        if (Current.Slot_IN_Item.Base_item != null)
        {
            if (Current.Slot_IN_Item.Base_item.Type != Type.Weapon)
            {
                Debug.Log("잘못된위치입니다");
                return;
            }
            Image_Renewal(Current);
            
            _Manager.Change_Weapon_Prefab();
        }
        else
        {
            _Manager.Change_Weapon_Prefab();
            Slot_Item_Image.sprite = Base_Image;
            Image_Set(true);
        }
    }
    

    protected override void Image_Renewal(Slot Current)
    {
        Item_Data Temp_Data = Current.Slot_IN_Item;
        if (Slot_IN_Item.Base_item == null)//자신이 빈칸이라면 출발위치 슬롯은 투명하게끔
        {
            Current.Slot_IN_Item = Slot_IN_Item;
            Current.Image_Set(false);
        }
        else// 자신이 빈칸이 아니라면 출발위치슬롯에 자기데이터 덮여씌어두기
        {
            Current.Slot_IN_Item = Slot_IN_Item;
            Current.Image_Set(true);
        }

        Slot_IN_Item = Temp_Data;//새로운 아이템 받기
        Image_Set();

    }

    */
    public void Init_Slot_item(Item_Data Slot)
    {
        Slot_IN_Item = Slot;
        Slot_Item_Image.sprite = Base_Image;

        Image_Set();
        _Manager.Change_Weapon_Prefab();
    }

}
