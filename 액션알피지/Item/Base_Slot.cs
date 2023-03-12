using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Slot : MonoBehaviour
{
    public Slot_Type TYPE;
    public abstract void Insert_Slot_Item(Item_Data New_Slot,int INDEX);//빈슬롯에 아이템집어넣기
    //public abstract void Insert_Slot_Item(Slot Current);//Current에서 함수를 부르는 Slot으로 이동하기
    protected abstract void Image_Renewal(Item_Data New_Slot);//빈슬롯 아이템 이미지 갱신

    //protected abstract void Image_Renewal(Slot Current);

    public abstract void Setting_Slot(UI_Manager Manager,int i=0);
}

public enum Slot_Type
{
    Weapon,
    Armor,
    ETC
}