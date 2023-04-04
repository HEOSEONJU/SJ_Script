using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player_Quest_Box : MonoBehaviour
{
    public List<Quest_Process> QBL;

    public void Init()
    {
        QBL=new List<Quest_Process>();
        for (int i = 0; i < Game_Master.instance.PM.Data.Accepted_Quest.Count; i++)
        {
            Quest_Process QP = Game_Master.instance.QLO.Search_Quest(Game_Master.instance.PM.Data.Accepted_Quest[i].INDEX);
            
            if (QP != null) 
            {
                QBL.Add(QP.Clone());
                QBL.Last().Progress = Game_Master.instance.PM.Data.Accepted_Quest[i].Progress;
                QP.Point = Game_Master.instance.PM.Data.Accepted_Quest[i].Point;
                

            }

        }


    }
    public void Accept_Quest(Quest_Process QP)
    {
        QBL.Add(QP.Clone());
        QBL.Last().Progress = 0;
    }

    public void Check_Monster_Kill(int ID)//���� ���� ���� ���̵�
    {

        List<Quest_Process> QPS = QBL.FindAll(x => x.List_Process[x.Progress].QUEST_TYPE == Quest_Type.Hunt);


        

        foreach (Quest_Process QP in QPS)
        {
            QP.Do_It_Process(ID, QP.Point);

            //QP.List_Process[QP.Progress].Hunt_Check(ID,QP.Point,out int RP);
            
        }
    }

    public string Call_Talk_Script(int ID)//��ȭ�� �ϴ� NPC�� ���� ���̵�
    {
        List<Quest_Process> QTS = QBL.FindAll(x=>x.List_Process[x.Progress].QUEST_TYPE == Quest_Type.Talk);
        foreach (Quest_Process QP in QTS)
        {
            if (QP.List_Process[QP.Progress].Talk_Check(ID))
                {
                    
                    return QP.List_Process[QP.Progress++].Return_Value();
                }
        }
        return null;
    }

}

