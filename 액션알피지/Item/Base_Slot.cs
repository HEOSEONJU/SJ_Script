using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Slot : MonoBehaviour
{
    public Slot_Type TYPE;
    public abstract void Insert_Slot_Item(Item_Data New_Slot,int INDEX);//�󽽷Կ� ����������ֱ�
    //public abstract void Insert_Slot_Item(Slot Current);//Current���� �Լ��� �θ��� Slot���� �̵��ϱ�
    protected abstract void Image_Renewal(Item_Data New_Slot);//�󽽷� ������ �̹��� ����

    //protected abstract void Image_Renewal(Slot Current);

    public abstract void Setting_Slot(UI_Manager Manager,int i=0);
}

public enum Slot_Type
{
    Weapon,
    Armor,
    ETC
}