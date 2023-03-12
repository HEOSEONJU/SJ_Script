using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : Base_Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    public UI_Manager UI;//관리자
    public Image Slot_Item_Image;//슬로에서 표시되는 이미지
    public Vector3 Origin;//UI상의 위치
    public Transform Slot_Position;//위치를조정할 트랜스폼

    public RectTransform rect;
    public CanvasGroup Canvas_Group;

    [SerializeField]
    protected Sprite Base_Image;//빈칸이되었을때의 투명이미지

    public Item_Data Slot_IN_Item;//슬롯안의 아이템
    public int Slot_INDEX;
    public bool State_Window = false;

    public bool Activation;//이미지있을때 UI출력

    public bool Hide;

    public override void Setting_Slot(UI_Manager Manager,int i)
    {
        Slot_INDEX = i;
        Slot_Item_Image = GetComponent<Image>();

        #region//아이콘투명
        Color Temp_Color = Color.white;
        Temp_Color.a = 0;
        Slot_Item_Image.color = Temp_Color;
        #endregion

        Slot_Position = transform.parent;
        rect = GetComponentInParent<RectTransform>();
        Origin = rect.position;
        Canvas_Group = gameObject.AddComponent<CanvasGroup>();
        UI = Manager;
    }
    public override void Insert_Slot_Item(Item_Data New_Slot,int INDEX)//해당 슬롯에 아이템 들어가기
    {
        if (New_Slot.Base_item == null)//아이템칸이 빈칸인칸을 이동시키면 작동안함
        {
            return;
        }
        Image_Renewal(New_Slot);
        Slot_INDEX = INDEX;
    }

    /*
    public override void Insert_Slot_Item(Slot Current)// current 아이템의 이동하기전 슬롯
    //함수를 작동하는건 아이템을 받는위치의 슬롯
    {   
        if (Current.Slot_IN_Item.Base_item != null)
        {
            Image_Renewal(Current);

            if (UI._Manager._Manager_Inventory.Main_Weapon.Slot_IN_Item.Base_item == null)//메인무기칸이 빈칸이되면 빈손으로
            {
                UI._Manager._WeaponSlotManager.Change_Weapon_Prefab();
            }
        }

        
    }
    */
    protected override void Image_Renewal(Item_Data New_Slot)
    {
        Slot_IN_Item = New_Slot;//새로운 아이템 받기
        if(New_Slot.Base_item==null)
        {
            Image_Set(false);
        }
        else
        Image_Set(true);
        
    }
    /*
    protected override void Image_Renewal(Slot Current)
    {
        #region 중복소비아이템합치기
        
        if (Slot_IN_Item.Base_item != null && Current.Slot_IN_Item.Base_item != null) //중복소비아이템 합치기
        {
            if(Use_Item_Merge(Current))
            {
                return;
            }
        }
        #endregion
        Item_Data Temp_Data = Current.Slot_IN_Item;
        if (Slot_IN_Item.Base_item == null)//자신이 빈칸이라면 출발위치 슬롯은 투명하게끔
        {
            Current.Slot_IN_Item=Slot_IN_Item;
            Current.Image_Set(false);

        }
        else// 자신이 빈칸이 아니라면 출발위치슬롯에 자기데이터 덮여씌어두기
        {
            Current.Slot_IN_Item = Slot_IN_Item;
            Current.Image_Set(true);
        }
        Slot_IN_Item = Temp_Data;//새로운 아이템 받기
        if (Slot_IN_Item.Base_item != null)
        {
            Image_Set(true);
        }
        else
        {
            Image_Set(false);
        }

    }
    


    public bool Use_Item_Merge(Slot Current)
    {
        if (Slot_IN_Item.Base_item.Item_Index == Current.Slot_IN_Item.Base_item.Item_Index)//인덱스확인 같은아이템인지
        {
            switch(Current.Slot_IN_Item.Base_item.Type)
            {
                case Type.Use:   //소비아이템인지 아이템종류확인
                    Use_Item temp = (Use_Item)Slot_IN_Item.Base_item;
                    if (temp.Max_Count >= Slot_IN_Item.count + Current.Slot_IN_Item.count)//합쳤을때 최대갯수를넘지않는다면 합치기
                    {
                        Slot_IN_Item.count += Current.Slot_IN_Item.Merge_count();
                        Current.Image_Set(false);
                        Current.Clear_Slot();
                        return true;
                    }
                    break;
            }
        }

        return false;
    }
    */

    public void Set_HIde(bool state)
    {
        Hide=state;
        
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
                Slot_Item_Image.sprite = Slot_IN_Item.Base_item.Item_Icon;
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


    
    public void Delete_Slot_Item()
    {
        Slot_IN_Item.Base_item=null;
        Slot_IN_Item.Upgrade = 0;
        Slot_IN_Item.count = 0;

        Color temp = Color.white;
        temp.a = 0;
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
        transform.SetParent(UI.transform);
        transform.SetAsLastSibling();
        Canvas_Group.alpha = 0.6f;
        Canvas_Group.blocksRaycasts = false;
        UI.Draging = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Slot_IN_Item.Base_item == null)
        {
            return;
        }

        rect.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == UI.transform)
        {
            transform.SetParent(Slot_Position);

            rect.position = Origin;
            ///transform.position = Slot_Position.position;
        }

        Canvas_Group.alpha = 1.0f;
        Canvas_Group.blocksRaycasts = true;
        UI.Draging = false;
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {


            Slot temp = eventData.pointerDrag.transform.GetComponent<Slot>();//들고있던것  작동하는건 도착지점

            Debug.Log(temp.TYPE + "->"+ this.TYPE);

            if (temp.Slot_IN_Item.Base_item == null)
            {
                return;
            }

            switch (this.TYPE)//도착슬롯
            {

                case Slot_Type.Weapon:
                    
                    UI._Manager.Call_Data().Swap_Items_ItoW(temp.Slot_INDEX);//Weapon-<ETC 
                    UI._Manager._WeaponSlotManager.Change_Weapon_Prefab();

                    break;
                case Slot_Type.Armor:
                    switch (temp.TYPE)//출발슬롯
                    {

                        case Slot_Type.Armor:
                            
                            UI._Manager.Call_Data().Swap_Items_AtoA(this.Slot_INDEX, temp.Slot_INDEX);//Armor<-Armor
                            break;
                        default:
                            UI._Manager.Call_Data().Swap_Items_ItoA(this.Slot_INDEX, temp.Slot_INDEX);//Armor<-ETC
                            break;
                    }

                    break;
                default:

                    switch (temp.TYPE)//출발슬롯
                    {
                        case Slot_Type.Weapon:
                            UI._Manager.Call_Data().Swap_Items_WtoI(this.Slot_INDEX);//ETC<-Weapon
                            UI._Manager._WeaponSlotManager.Change_Weapon_Prefab();
                            break;
                        case Slot_Type.Armor:
                            
                            UI._Manager.Call_Data().Swap_Items_AtoI(this.Slot_INDEX, temp.Slot_INDEX);//ETC<-Armor
                            break;
                        default:

                            UI._Manager.Call_Data().Swap_Items_ItoI(this.Slot_INDEX, temp.Slot_INDEX);//ETC<-ETC

                            break;
                    }
                    break;
            }
            if (UI._Manager.Call_Data().Items[temp.Slot_INDEX].Base_item==null)
            {
                temp.Active_Slot(false);
            }
            UI._Manager._Manager_Inventory.Load_on_Data(UI._Manager.Call_Data());

            UI.Reseting_Status();
        }


    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if ((Activation && UI.Windows == false) && UI.Draging==false)
        {
            UI.Windows = true;
            UI.Active_Windows(UI.Windows, this);

        }
        

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (UI.Windows == true)
        {
            UI.Windows = false;
            UI.Active_Windows(UI.Windows);


        }


    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Slot_IN_Item.Base_item==null)
        {
            return;
        }
        switch(Slot_IN_Item.Base_item.Type)
        {
            case Type.Use:
                Use_Item temp = (Use_Item)Slot_IN_Item.Base_item;
                if (UI.Use_Heal_item(temp))
                {

                    if (Slot_IN_Item.UseItem())
                    {
                        UI.Renewal_Text(this);
                        if (Slot_IN_Item.count <= 0)
                        {
                            Delete_Slot_Item();
                            UI.Windows = false;
                            UI.Active_Windows(UI.Windows);
                        }
                    }


                }


                break;
        }
       
    }

}