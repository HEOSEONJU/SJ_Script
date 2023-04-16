using Item_Enum;
using UnityEngine;

public abstract class Base_Slot : MonoBehaviour
{
    [EnumFlags]
    public EItem_Slot_Type TYPE;
    public abstract void Insert_Slot_Item(int INDEX);//빈슬롯에 아이템집어넣기
    //public abstract void Insert_Slot_Item(Slot Current);//Current에서 함수를 부르는 Slot으로 이동하기
    protected abstract void Image_Renewal();//빈슬롯 아이템 이미지 갱신

    //protected abstract void Image_Renewal(Slot Current);

    public abstract void Setting_Slot(int i=0);
}

