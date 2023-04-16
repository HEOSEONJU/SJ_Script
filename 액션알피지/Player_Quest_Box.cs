using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Quests;

public class Player_Quest_Box : MonoBehaviour
{
    public List<Quest_Process> QBL;

    public void Init()
    {
        QBL = new List<Quest_Process>();
        for (int i = 0; i < Game_Master.instance.PM.Data.Accepted_Quest.Count; i++)
        {
            Load_Quest(Game_Master.instance.QLO.Search_Quest(Game_Master.instance.PM.Data.Accepted_Quest[i].INDEX), i);
        }


    }
    public void Load_Quest(Quest_Process QP, int i)
    {
        if (QP == null)
            return;
        QBL.Add(QP.Clone());
        QBL.Last().Progress = Game_Master.instance.PM.Data.Accepted_Quest[i].Progress;
        QP.Point = Game_Master.instance.PM.Data.Accepted_Quest[i].Point;
    }

    public void Accept_Quest(Quest_Process QP)
    {
        QBL.Add(QP.Clone());
        QBL.Last().Progress = 0;
    }

    public void Check_Monster_Kill(int ID)//죽은 몹이 고유 아이디
    {
        foreach (Quest_Process QP in QBL.FindAll(x => x.List_Process[x.Progress]._questType == EQuest_Type.Hunt))
        {
            QP.Do_It_Process(ID, QP.Point);


        }
    }

    public string Call_Talk_Script(int ID)//대화를 하는 NPC의 고유 아이디
    {
        foreach (Quest_Process QP in QBL.FindAll(x => x.List_Process[x.Progress]._questType == EQuest_Type.Talk && x.Progress != x.MaxProgress))
        {
            if (QP.List_Process[QP.Progress].Talk_Check(ID))
            {
                string Temp_String = QP.List_Process[QP.Progress++].Return_Value();
                Game_Master.instance.PM.Save_On_FireBase();
                return Temp_String;
            }
        }
        return null;
    }
    public string Check_Give_Item()
    {
        int c = 0;
        foreach (Quest_Process QP in QBL.FindAll(x => x.List_Process[x.Progress]._questType == EQuest_Type.Give && x.Progress != x.MaxProgress))
        {
            c++;
            if (QP.List_Process[QP.Progress].Give_Check((QP.List_Process[QP.Progress] as Quest_Give).Item_ID))
            {
                QP.List_Process[QP.Progress++].Return_Value();
                Game_Master.instance.PM.Save_On_FireBase();
                return "아이템 잘 건네 받았네";
            }
        }
        if (c == 0)
        {
            return "자네에게 받을 아이템은 없네만";
        }
        return "아이템이 모자란거만 같네만";
    }

}

