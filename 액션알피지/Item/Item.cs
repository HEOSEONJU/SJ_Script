using Item_Enum;

using UnityEngine;


[CreateAssetMenu(menuName = "Item/Item")]
public class item:ScriptableObject
{
    public string Item_Name;//아이템이름
    public int Item_Index;//아이템 ID
    public Sprite Item_Icon;//아이템아이콘
    public string exp;//설명

    public int Value;
    
    public EItem_Slot_Type Type;
    
    

    
}


