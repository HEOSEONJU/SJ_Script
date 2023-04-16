using Item_Enum;

using UnityEngine;


public  class Equip_Slot : Inventory_Slot
{
    protected virtual void Image_Set()//이미지 true면 투명도해제 false면 투명하게
    {
        return;
    }
    public void Init_Slot_item(int i)
    {
        Slot_INDEX = i;
        

        Image_Set();
    }

}
