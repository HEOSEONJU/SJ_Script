using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Quest_Basic
{
    public int Quest_ID;
    public Quest_Type QUEST_TYPE;
    public string Quest_Title;
    public string Quest_Description;

    [SerializeField]
    int Value_Gold;

    [SerializeField]
    List<Item_Data> Value_item;



    [SerializeField]
    public int Progress = 0;
    public bool Can//����Ʈ�� ���� �� ���� = �������� ����
    {
        get
        {
            return Check();
        }
    }
    public int[] Before_List;//������;

    public bool Complete = false;

    protected virtual void Complete_Check()
    {
        Complete = false;
    }
    public virtual float Progress_float()
    {
        return 0;
    }
    bool Check()//�������� Ž��
    {
        if(Before_List.Length==0)//�������� �����Ƿ� �ٷ� ��
        {
            return true;
        }
        foreach (int item in Before_List)
        {
            if(Game_Master.instance.PM.Data.Complted_Quest.Find(x=>x ==item)==0)//�������ǿ� �����ϴ� ����Ʈ�� Ŭ�������� �ʾ��� ���
            {
                return false;
            }
        }
        //�ݺ������� ������� �ʾҴٸ� ��� ���� ����
        return true;
    }

    public bool Accept()//�̹̹ްų� �Ϸ��� ����Ʈ�� ���� �� ����
    {
        Debug.Log("��������Ʈ"+Quest_ID);
        if (Game_Master.instance.PM.Data.Complted_Quest.FindIndex(x=>x==Quest_ID)!=-1)
        {

            Debug.Log("�̹̿Ϸ���");
            return false;
        }
        if (Game_Master.instance.PM.PQB.QBL.FindIndex(x => x.Quest_ID == Quest_ID) !=-1)
        {
            Debug.Log("�̹̹�������Ʈ");
            return false;
        }
        

        return true;
    }
    public bool Reward()
    {
        if(!Complete)
        {
            Debug.Log("���ǹ̸���");
            return false;
        }
        else if((Game_Master.instance.PM._Manager_Inventory.Inventroy_Count + Value_item.Count) >= 20)
        {
            Debug.Log("��������");
            return false;
        }
        foreach (var item in Value_item)
        {
            Game_Master.instance.PM._Manager_Inventory.Get_Item(item);
        }
        Game_Master.instance.PM.Data.Current_Gold += Value_Gold;
        
        
        


        return true;

    }

    public Quest_Basic Clone()
    {



        return (Quest_Basic)this.MemberwiseClone();
    }
}

public enum Quest_Type//����Ʈ Ÿ�� 
{
    Hunt,//�������Ʈ
    Talk//��ȭ������Ʈ
}
