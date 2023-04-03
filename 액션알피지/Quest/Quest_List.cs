using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/Quest/Data")]
public class Quest_List : ScriptableObject
{
    public List<Quest_Basic> Quests;

    public Quest_Basic Search_Quest(int INDEX)//����Ž��
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

    public void Sort_List()//����Ʈ�� �̿��� �����ۼ��� ����
    {
        Quests = Quests.OrderBy(x => x.Quest_ID).ToList();
    }


    
    


}
