using Item_Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Equip_Armor : Equip_Slot, IPointerEnterHandler, IDragHandler, IDropHandler
{
    
    public int Armor_Position;
    public new void OnPointerEnter(PointerEventData eventData)
    {

        if ((Activation && Game_Master.instance.UI.Windows == false) && Game_Master.instance.UI.Draging == false)
        {

            Game_Master.instance.UI.Open_Small_Windows(Game_Master.instance.PM.Data.Equip_Armor_Item[Slot_INDEX]);

        }


    }
    protected override void Image_Set()//�̹��� true�� �������� false�� �����ϰ�
    {
        
        Color temp = Color.white;
        temp.a = 1;

            if (Game_Master.instance.PM.Data.Equip_Armor_Item[Slot_INDEX].Base_item == null)
            {
                Slot_Item_Image.sprite = Base_Image;
                Activation = false;
            }
            else
            {
                Slot_Item_Image.sprite = Game_Master.instance.PM.Data.Equip_Armor_Item[Slot_INDEX].Base_item.Item_Icon;
                Activation = true;
            }



        




        Slot_Item_Image.color = temp;


    }
    public new void OnDrag(PointerEventData eventData)
    {
        if (Game_Master.instance.PM.Data.Equip_Armor_Item[Slot_INDEX].Base_item == null)
        {
            return;
        }

        rect.position = eventData.position;
    }



    public new void OnDrop(PointerEventData eventData)//�κ��丮���� ���ĭ�����̵��� �۵�
    {

        if (eventData.pointerDrag != null)
        {


            eventData.pointerDrag.transform.TryGetComponent<Inventory_Slot>(out Inventory_Slot temp);//����ִ���  �۵��ϴ°� ��������

            

            if (!temp.TYPE.HasFlag(this.TYPE))
            {
                //Debug.Log("�ٸ�ĭ�Դϴ�");
                return;
            }

            if (Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item == null)
            {
                return;
            }
            

            if (!Game_Master.instance.PM.Data.Items[temp.Slot_INDEX].Base_item.Type.HasFlag(this.TYPE))
            {
                //Debug.Log("�ٸ�Ÿ���� ����Դϴ�");
                return;
            }

            //Debug.Log("�� �� �ִ� ĭ�Դϴ�");

            Game_Master.instance.PM.Data.Swap_Items_ItoA(this.Slot_INDEX, temp.Slot_INDEX);//ETC->Armor
            Game_Master.instance.PM._manager_Inventory.Load_on_Data(Game_Master.instance.PM.Data);
            Game_Master.instance.UI.Reseting_Status();
        }


    }

}

