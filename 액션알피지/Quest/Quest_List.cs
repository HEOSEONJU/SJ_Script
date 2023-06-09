using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ScriptableObject/Quest/Data")]
public class Quest_List : ScriptableObject
{
    public List<Quest_Process> Quests;
    public List<GameObject> Prefabs;
    public Quest_Process Search_Quest(int INDEX)//이진탐색
    {

        if (INDEX == 0)
        {
            return null;
        }
        int Low = 0;
        int High = Quests.Count - 1;
        int Mid;
        while (Low <= High)
        {
            Mid = (Low + High) / 2;
            if (Quests[Mid].Quest_ID == INDEX)
            {
                return Quests[Mid];
            }
            else if (Quests[Mid].Quest_ID > INDEX)
            {
                High = Mid - 1;
            }
            else
            {
                Low = Mid + 1;
            }

        }
        return null;
    }

    public void Sort_List()//리스트를 이용한 아이템순서 정렬
    {
        Quests = Quests.OrderBy(x => x.Quest_ID).ToList();
        Prefabs = Prefabs.OrderBy(x => x.name).ToList();
        for(int i=0;i<Quests.Count;i++)
        {
            Quests[i].List_Process = Prefabs[i].GetComponents<Quest_Basic>().ToList();
            Quests[i].MaxProgress = Quests[i].List_Process.Count;
        }
        
    }


    
    


}
