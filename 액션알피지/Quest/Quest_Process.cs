using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Quest_Process
{

    public int Quest_ID;
    
    public string Quest_Title;
    public string Quest_Description;//����Ʈ��ü����

    [SerializeField]
     int Value_Gold;

    [SerializeField]
    List<Item_Data> Value_item;

    public int Progress=0;
    public int MaxProgress=1;

    public List<Quest_Basic> List_Process;
    public int Point;

    
    public int[] Before_List;//������;

    public bool Can//����Ʈ�� ���� �� ���� = �������� ����
    {
        get
        {
            return Check();
        }
    }
    public string View_Reward_List()
    {
        string s="";
        s += Value_Gold + "G";
        foreach(var item in Value_item) 
        {
            switch(item.Base_item.Type)
            {
                case Type.Equip:
                    s += ","+item.Base_item.name;
                    break;
                case Type.Use:
                    s += "," + item.Base_item.name+item.count + "��";
                    break;

            }

            
        }

        return s;
    }

    bool Check()//�������� Ž��
    {
        if (Before_List.Length == 0)//�������� �����Ƿ� �ٷ� ��
        {
            return true;
        }
        foreach (int item in Before_List)
        {
            if (Game_Master.instance.PM.Data.Complted_Quest.Find(x => x == item) == 0)//�������ǿ� �����ϴ� ����Ʈ�� Ŭ�������� �ʾ��� ���
            {
                return false;
            }
        }
        //�ݺ������� ������� �ʾҴٸ� ��� ���� ����
        return true;
    }

    public bool Accept()//�̹̹ްų� �Ϸ��� ����Ʈ�� ���� �� ����
    {
        Debug.Log("��������Ʈ" + Quest_ID);
        if (Game_Master.instance.PM.Data.Complted_Quest.FindIndex(x => x == Quest_ID) != -1)
        {

            Debug.Log("�̹̿Ϸ���");
            return false;
        }
        if (Game_Master.instance.PM.PQB.QBL.FindIndex(x => x.Quest_ID == Quest_ID) != -1)
        {
            Debug.Log("�̹̹�������Ʈ");
            return false;
        }


        return true;
    }
    public bool Reward()
    {
        if (Progress!=MaxProgress)
        {
            Debug.Log("���ǹ̸���");
            return false;
        }
        else if ((Game_Master.instance.PM._Manager_Inventory.Inventroy_Count + Value_item.Count) >= 20)
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

    public Quest_Process Clone()
    {



        return (Quest_Process)this.MemberwiseClone();
    }



    public void Do_It_Process(int ID, int Current_point)
    {
        
        if (Progress == MaxProgress)
        {
            
            return;
        }

        switch (List_Process[Progress].QUEST_TYPE) 
        {
            case Quest_Type.Talk:
                if (List_Process[Progress].Talk_Check(ID))
                {
                    Point = 1;
                }
                else { Point = 0; }
                break;
            case Quest_Type.Hunt:
                List_Process[Progress].Hunt_Check(ID,Point,out int RP);
                Point = RP;
                
                break;

        }
        if(List_Process[Progress].Complete_Check(Point))
        {
            Progress++;
            Point = 0;
            
            return;
        }
        
    }

}
