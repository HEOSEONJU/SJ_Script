using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item_Enum;
using System.Text;
using Quests;

[System.Serializable]
public class Quest_Process
{

    public int Quest_ID;
    
    public string Quest_Title;
    public string Quest_Description;//퀘스트전체설명

    [SerializeField]
     int Value_Gold;

    [SerializeField]
    List<Item_Data> Value_item;

    public int Progress=0;
    public int MaxProgress=1;

    public List<Quest_Basic> List_Process;
    public int Point;

    
    public int[] Before_List;//선행목록;

    public bool Can//퀘스트를 받을 수 있음 = 선행조건 만족
    {
        get
        {
            return Check();
        }
    }
    public StringBuilder View_Reward_List()
    {
        StringBuilder s=new StringBuilder();
        s .Append(Value_Gold + "G");

        foreach(var item in Value_item) 
        {
            if(item.Base_item.Type==EItem_Slot_Type.ETC||
                item.Base_item.Type==EItem_Slot_Type.Use)
            {
                s.AppendLine( "," + item.Base_item.name + item.count + "개");
            }
            else
            {
                s.AppendLine("," + item.Base_item.name);
            }


            
        }

        return s;
    }

    bool Check()//선행조건 탐지
    {
        if (Before_List.Length == 0)//선행조건 없으므로 바로 참
        {
            return true;
        }
        foreach (int item in Before_List)
        {
            if (Game_Master.instance.PM.Data.Complted_Quest.Find(x => x == item) == 0)//선행조건에 만족하는 퀘스트를 클리어하지 않았을 경우
            {
                return false;
            }
        }
        //반복문에서 검출되지 않았다면 모든 조건 만족
        return true;
    }

    public bool Accept()//이미받거나 완료한 퀘스트는 받을 수 없음
    {
        Debug.Log("받은퀘스트" + Quest_ID);
        if (Game_Master.instance.PM.Data.Complted_Quest.FindIndex(x => x == Quest_ID) != -1)
        {

            Debug.Log("이미완료기록");
            return false;
        }
        if (Game_Master.instance.PM._playerQuestBox.QBL.FindIndex(x => x.Quest_ID == Quest_ID) != -1)
        {
            Debug.Log("이미받은퀘스트");
            return false;
        }


        return true;
    }
    public bool Reward()
    {
        if (Progress!=MaxProgress)
        {
            Debug.Log("조건미만족");
            return false;
        }
        else if ((Game_Master.instance.PM._manager_Inventory.Inventroy_Count + Value_item.Count) >= 20)
        {
            Debug.Log("공간부족");
            return false;
        }
        foreach (var item in Value_item)
        {
            Game_Master.instance.PM._manager_Inventory.Get_Item(item);
        }
        Game_Master.instance.PM.Data.Current_Gold += Value_Gold;

        return true;

    }

    public Quest_Process Clone()
    {

        return this.MemberwiseClone()as Quest_Process;
    }



    public void Do_It_Process(int ID, int Current_point)
    {
        
        if (Progress == MaxProgress)
        {
            
            return;
        }

        switch (List_Process[Progress]._questType) 
        {
            case EQuest_Type.Talk:
                if (List_Process[Progress].Talk_Check(ID))
                {
                    Point = 1;
                }
                else { Point = 0; }
                break;
            case EQuest_Type.Hunt:
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
