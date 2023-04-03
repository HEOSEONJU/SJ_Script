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
    public bool Can//퀘스트를 받을 수 있음 = 선행조건 만족
    {
        get
        {
            return Check();
        }
    }
    public int[] Before_List;//선행목록;

    public bool Complete = false;

    protected virtual void Complete_Check()
    {
        Complete = false;
    }
    public virtual float Progress_float()
    {
        return 0;
    }
    bool Check()//선행조건 탐지
    {
        if(Before_List.Length==0)//선행조건 없으므로 바로 참
        {
            return true;
        }
        foreach (int item in Before_List)
        {
            if(Game_Master.instance.PM.Data.Complted_Quest.Find(x=>x ==item)==0)//선행조건에 만족하는 퀘스트를 클리어하지 않았을 경우
            {
                return false;
            }
        }
        //반복문에서 검출되지 않았다면 모든 조건 만족
        return true;
    }

    public bool Accept()//이미받거나 완료한 퀘스트는 받을 수 없음
    {
        Debug.Log("받은퀘스트"+Quest_ID);
        if (Game_Master.instance.PM.Data.Complted_Quest.FindIndex(x=>x==Quest_ID)!=-1)
        {

            Debug.Log("이미완료기록");
            return false;
        }
        if (Game_Master.instance.PM.PQB.QBL.FindIndex(x => x.Quest_ID == Quest_ID) !=-1)
        {
            Debug.Log("이미받은퀘스트");
            return false;
        }
        

        return true;
    }
    public bool Reward()
    {
        if(!Complete)
        {
            Debug.Log("조건미만족");
            return false;
        }
        else if((Game_Master.instance.PM._Manager_Inventory.Inventroy_Count + Value_item.Count) >= 20)
        {
            Debug.Log("공간부족");
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

public enum Quest_Type//퀘스트 타입 
{
    Hunt,//사냥퀘스트
    Talk//대화형퀘스트
}
