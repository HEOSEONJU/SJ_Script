using Item_Enum;

using UnityEngine;


public  class Equip_Slot : Inventory_Slot
{
    protected virtual void Image_Set()//�̹��� true�� �������� false�� �����ϰ�
    {
        return;
    }
    public void Init_Slot_item(int i)
    {
        Slot_INDEX = i;
        

        Image_Set();
    }

}
