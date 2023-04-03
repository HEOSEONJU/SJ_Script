using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player_Quest_Box : MonoBehaviour
{
    public List<Quest_Basic> QBL;

    public void Init()
    {
        QBL=new List<Quest_Basic>();
        for (int i = 0; i < Game_Master.instance.PM.Data.Accepted_Quest.Count; i++)
        {
            Quest_Basic QB = Game_Master.instance.QLO.Search_Quest(Game_Master.instance.PM.Data.Accepted_Quest[i].INDEX);
            
            if (QB != null) 
            {
                QBL.Add(QB.Clone());
                QBL.Last().Progress = Game_Master.instance.PM.Data.Accepted_Quest[i].Progress;
                
            }

        }


    }
    public void Accept_Quest(Quest_Basic QB)
    {
        QBL.Add(QB.Clone());
        QBL.Last().Progress = 0;
    }

    public void Check_Monster_Kill(int ID)//죽은 몹이 고유 아이디
    {

        List<Quest_Basic> QHS = QBL.FindAll(x => x.QUEST_TYPE == Quest_Type.Hunt);
        foreach (Quest_Basic QB in QHS)
        {
            Quest_Hunt QH = (Quest_Hunt)QB;
            QH.Hunt_Check(ID);
        }
    }

    public string Call_Talk_Script(int ID)//대화를 하는 NPC의 고유 아이디
    {
        List< Quest_Basic> QTS = QBL.FindAll(x=>x.QUEST_TYPE==Quest_Type.Talk);
        foreach (Quest_Basic QB in QTS)
        {
                Quest_Talk QT = (Quest_Talk)QB;
                if(QT.Talk_Check(ID))
                {
                    return QT.Return_Value();
                }
        }
        return null;
    }

}

