using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_Slot : Base_Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    public Image Slot_Item_Image;//���ο��� ǥ�õǴ� �̹���
    public Vector3 Origin;//UI���� ��ġ
    public Transform Slot_Position;//��ġ�������� Ʈ������

    public RectTransform rect;
    public CanvasGroup Canvas_Group;

    [SerializeField]
    protected Sprite Base_Image;//��ĭ�̵Ǿ������� �����̹���

    //public Item_Data Slot_IN_Item;//���Ծ��� ������
    public int Slot_INDEX;
    public bool State_Window = false;

    public bool Activation;//�̹��������� UI���

    public bool Hide;

    public override void Setting_Slot(int i)
    {
        Slot_INDEX = i;
        Slot_Item_Image = GetComponent<Image>();

        #region//����������
        Color Temp_Color = Color.white;
        Temp_Color.a = 0;
        Slot_Item_Image.color = Temp_Color;
        #endregion

        Slot_Position = transform.parent;
        rect = GetComponentInParent<RectTransform>();
        Origin = rect.position;
        Canvas_Group = gameObject.AddComponent<CanvasGroup>();
        
    }
    public override void Insert_Slot_Item(int INDEX)//�ش� ���Կ� ������ ����
    {
        
        
        Slot_INDEX = INDEX;
        Image_Renewal();
    }

    
    protected override void Image_Renewal()
    {
        //Slot_IN_Item = New_Slot;//���ο� ������ �ޱ�
        if (Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item==null)
        {
            Image_Set(false);
        }
        else
        Image_Set(true);
        
    }
    

    public void Set_HIde(bool state)
    {
        Hide=state;
        
        Image_Set(!state);
        
    }
    public void Image_Set(bool active)//�̹��� true�� �������� false�� �����ϰ�
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


            Inventory_Slot temp = eventData.pointerDrag.transform.GetComponent<Inventory_Slot>();//����ִ���  �۵��ϴ°� ��������

            Debug.Log(temp.TYPE + "->"+ this.TYPE);

            if (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item == null)
            {
                return;
            }

            switch (this.TYPE)//��������
            {

                case Slot_Type.Weapon:
                    Equip_Slot_Type TW1 = this.GetComponent<Equip_Slot>().EST;
                    Equip_Item TW = (Equip_Item)Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item;
                    Equip_Slot_Type TW2 = TW.EST;
                    if (TW1 != TW2)//�������� �ٸ��迭 ��
                    {
                        //Debug.Log("�ٸ�ĭ�Դϴ�");
                        return;
                    }
                    Game_Master.instance.PM.Data.Swap_Items_ItoW(temp.Slot_INDEX);//Weapon-<ETC 
                    Game_Master.instance.PM._WeaponSlotManager.Change_Weapon_Prefab();

                    break;
                case Slot_Type.Armor:
                    Equip_Slot_Type TA1 = this.GetComponent<Equip_Slot>().EST;
                    Equip_Item TA = (Equip_Item)Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item;
                    Equip_Slot_Type TA2 = TA.EST;
                    if(TA1!=TA2)//�������� �ٸ��迭 ��
                    {
                        Debug.Log("�ٸ�ĭ�Դϴ�");
                        return;
                    }
                    
                    switch (temp.TYPE)//��߽���
                    {
                        case Slot_Type.Armor:
                            Game_Master.instance.PM.Data.Swap_Items_AtoA(this.Slot_INDEX, temp.Slot_INDEX);//Armor<-Armor
                            break;
                        default:
                            Game_Master.instance.PM.Data.Swap_Items_ItoA(this.Slot_INDEX, temp.Slot_INDEX);//Armor<-ETC
                            break;
                    }

                    break;
                default:

                    switch (temp.TYPE)//��߽���
                    {
                        case Slot_Type.Weapon:
                            Game_Master.instance.PM.Data.Swap_Items_WtoI(this.Slot_INDEX);//ETC<-Weapon
                            Game_Master.instance.PM._WeaponSlotManager.Change_Weapon_Prefab();
                            break;
                        case Slot_Type.Armor:

                            Game_Master.instance.PM.Data.Swap_Items_AtoI(this.Slot_INDEX, temp.Slot_INDEX);//ETC<-Armor
                            break;
                        default:

                            Game_Master.instance.PM.Data.Swap_Items_ItoI(this.Slot_INDEX, temp.Slot_INDEX);//ETC<-ETC

                            break;
                    }
                    break;
            }
            if (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item==null)
            {
                temp.Active_Slot(false);
            }
            Game_Master.instance.PM._Manager_Inventory.Load_on_Data(Game_Master.instance.PM.Data);

            Game_Master.instance.UI.Reseting_Status();
        }


    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if ((Activation && Game_Master.instance.UI.Windows == false) && Game_Master.instance.UI.Draging==false)
        {
            
            Game_Master.instance.UI.Active_Windows(true, Game_Master.instance.PM.Data.Items[Slot_INDEX]);

        }
        

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Game_Master.instance.UI.Windows == true)
        {
            Game_Master.instance.UI.Active_Windows(false);
        }


    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Game_Master.instance.PM.Data.Items[Slot_INDEX] == null)
        {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            switch (Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item.Type)
            {
                case Type.Use:
                    Use_Item temp = (Use_Item)Game_Master.instance.PM.Data.Items[Slot_INDEX].Base_item;
                    if (Game_Master.instance.UI.Use_Heal_item(temp))//�̹� �ִ�ü���̸�flase��ȯ �ƴ϶�� ȿ�������� true��ȯ
                    {
                        if (Game_Master.instance.PM.Data.Items[Slot_INDEX].UseItem())//������ �Ҹ��ϰ�  ������ 0�����ϰ��Ǹ� �������ε�������
                        {
                            Game_Master.instance.UI.Renewal_Text(Game_Master.instance.PM.Data.Items[Slot_INDEX]);
                            if(Game_Master.instance.PM.Data.Items[Slot_INDEX].count==0)
                            {
                                Empty_Slot();
                                Game_Master.instance.UI.Active_Windows(false);

                            }
                        }
                    }
                    break;
            }
        }


        
       
    }
    public void Empty_Slot()
    {
        Game_Master.instance.PM.Data.Items[Slot_INDEX].Delete_Data();
        
        Image_Set(false);
    }

}