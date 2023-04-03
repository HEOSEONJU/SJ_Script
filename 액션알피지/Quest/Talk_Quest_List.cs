using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/Quest/Talk")]
public class Talk_Quest_List : ScriptableObject
{
    public List<Quest_Talk> Talk_Quests;

    public Quest_Talk Search_Quest(int INDEX)//이진탐색
    {

        if (INDEX == 0)
        {
            return null;
        }
        int Low = 0;
        int High = Talk_Quests.Count - 1;
        int Mid;
        while (Low <= High)
        {
            Mid = (Low + High) / 2;
            if (Talk_Quests[Mid].Quest_ID == INDEX)
            {
                return Talk_Quests[Mid];
            }
            else if (Talk_Quests[Mid].Quest_ID > INDEX)
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
        Talk_Quests = Talk_Quests.OrderBy(x => x.Quest_ID).ToList();
    }


    
    


}
