using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Item_Enum;
public class Inventory_Slot : Base_Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Image Slot_Item_Image;//슬로에서 표시되는 이미지
    public Vector3 Origin;//UI상의 위치
    public Transform Slot_Position;//위치를조정할 트랜스폼

    public RectTransform rect;
    public CanvasGroup Canvas_Group;

    [SerializeField]
    protected Sprite Base_Image;//빈칸이되었을때의 투명이미지

    //public Item_Data Slot_IN_Item;//슬롯안의 아이템
    public int Slot_INDEX;
    public bool State_Window = false;

    public bool Activation;//이미지있을때 UI출력

    public bool Hide;



    public override void Setting_Slot(int i)
    {
        Slot_INDEX = i;

        if (TryGetComponent(out Image Get))
        {
            Slot_Item_Image = Get;
        }
        #region//아이콘투명
        Color Temp_Color = Color.white;
        Temp_Color.a = 0;
        Slot_Item_Image.color = Temp_Color;
        #endregion

        Slot_Position = transform.parent;
        rect = GetComponentInParent<RectTransform>();


        Origin = rect.position;
        Canvas_Group = gameObject.AddComponent<CanvasGroup>();

    }
    public override void Insert_Slot_Item(int INDEX)//해당 슬롯에 아이템 들어가기
    {


        Slot_INDEX = INDEX;
        Image_Renewal();
    }


    protected override void Image_Renewal()
    {
        //Slot_IN_Item = New_Slot;//새로운 아이템 받기
        if (Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item == null)
        {
            Image_Set(false);
        }
        else
            Image_Set(true);

    }


    public void Set_HIde(bool state)
    {
        Hide = state;

        Image_Set(!state);

    }
    public void Image_Set(bool active)//이미지 true면 투명도해제 false면 투명하게
    {
        Color temp = Color.white;


        if (Hide)
        {
            temp.a = 0;
            Activation = !Hide;
            Slot_Item_Image.color = temp;
            return;
        }
        Activation = active;



        if (active)
        {
            temp.a = 1;
            Slot_Item_Image.sprite = Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item.Item_Icon;
        }
        else
        {
            if (Base_Image != null)
            {
                Slot_Item_Image.sprite = Base_Image;
                temp.a = 1;
            }
            else
                temp.a = 0;
        }

        Slot_Item_Image.color = temp;



    }





    public bool IsEmpty()
    {
        return !Activation;
    }
    public void Active_Slot(bool check)
    {
        Activation = check;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(Game_Master.instance.UI.transform);
        transform.SetAsLastSibling();
        Canvas_Group.alpha = 0.6f;
        Canvas_Group.blocksRaycasts = false;
        Game_Master.instance.UI.Draging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item == null)
        {
            return;
        }

        rect.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == Game_Master.instance.UI.transform)
        {
            transform.SetParent(Slot_Position);

            rect.position = Origin;
            ///transform.position = Slot_Position.position;
        }

        Canvas_Group.alpha = 1.0f;
        Canvas_Group.blocksRaycasts = true;
        Game_Master.instance.UI.Draging = false;
    }


    public void OnDrop(PointerEventData eventData)
    {

        if (eventData.pointerDrag != null)
        {


            eventData.pointerDrag.transform.TryGetComponent<Inventory_Slot>(out Inventory_Slot temp);//들고있던것  작동하는건 도착지점


            if (temp is Equip_Weapon)//출발칸이 무기
            {
                Game_Master.instance.PM.Data.Swap_Items_WtoI(this.Slot_INDEX);//Weapon->ETC

            }
            else if (temp is Equip_Armor)//출발칸이 방어구
            {
                Game_Master.instance.PM.Data.Swap_Items_AtoI(this.Slot_INDEX, temp.Slot_INDEX);//Armor->ETC    
            }
            else//출반칸이 가방
            {
                Game_Master.instance.PM.Data.Swap_Items_ItoI(this.Slot_INDEX, temp.Slot_INDEX);//ETC->ETC
            }
            Game_Master.instance.PM._manager_Inventory.Load_on_Data(Game_Master.instance.PM.Data);
            Game_Master.instance.UI.Reseting_Status();
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if ((Activation && Game_Master.instance.UI.Windows == false) && Game_Master.instance.UI.Draging == false)
        {

            Game_Master.instance.UI.Open_Small_Windows(Game_Master.instance.PM.Data.Items[Slot_INDEX]);

        }


    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Game_Master.instance.UI.Windows == true)
        {
            Game_Master.instance.UI.Close_Small_Windows();
        }


    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Game_Master.instance.PM.Data.Items[Slot_INDEX] == null)
        {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Right)//우클릭
        {
            if (Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item.Type == EItem_Slot_Type.Use)//우클릭 대상이 소비
            {
                if (Game_Master.instance.UI.Use_Heal_item(Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item as Use_Item))//이미 최대체력이면false반환 아니라면 효과적용후 true반환
                {
                    if (Game_Master.instance.PM.Data.Items[Slot_INDEX].UseItem() && Game_Master.instance.PM.Data.Items[Slot_INDEX].count==0)//갯수를 소모하고  갯수가 0개이하가되면 아이템인덱스삭제
                    {
                        Empty_Slot();
                        Game_Master.instance.UI.Close_Small_Windows();

                    }
                }

            }
        }




    }
    public void Empty_Slot()
    {
        Game_Master.instance.PM.Data.Items[Slot_INDEX].Delete_Data();

        Image_Set(false);
    }

}