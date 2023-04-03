using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest/Hunt")]
public class Hunt_Quest_List : ScriptableObject
{
    public List<Quest_Hunt> Hunt_Quests;

    public Quest_Hunt Search_Quest(int INDEX)//이진탐색
    {

        if (INDEX == 0)
        {
            return null;
        }
        int Low = 0;
        int High = Hunt_Quests.Count - 1;
        int Mid;
        while (Low <= High)
        {
            Mid = (Low + High) / 2;
            if (Hunt_Quests[Mid].Quest_ID == INDEX)
            {
                return Hunt_Quests[Mid];
            }
            else if (Hunt_Quests[Mid].Quest_ID > INDEX)
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
        Hunt_Quests = Hunt_Quests.OrderBy(x => x.Quest_ID).ToList();
    }


    
    


}
