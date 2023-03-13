using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class SpellCardData
{
    

    public int id;
    public string CardName;
    public string exp;
    public int cost;
    public int Rank;
    public Sprite Image;
    public Sprite BG_Image;    
    public int CardType;//0적에게 / 1나에게 // 2전체
    //public int SpellType;//0 0이면데미지 1이면공다운 2이면방다운 3CC?
                         //1 0이면회복 1이면공업 2이면방업 3이면최대체력회복 
    public GameObject Effects;
    public int Value_Enemy_Effect_Num;
    public int Value_Char_Effect_Num;
    public int Value_Enemy_Damage_Effect;
}
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellCardDataObject", order = 1)]
public class CardScriptable : ScriptableObject
{
    public List<SpellCardData> cards = new List<SpellCardData>();
    public SpellCardData Find_Spell_Card(int INDEX)//이진탐색
    {

        if (cards.Count == 0)
        {
            return null;
        }
        int Low = 0;
        int High = cards.Count - 1;
        int Mid;
        while (Low <= High)
        {
            Mid = (Low + High) / 2;
            if (cards[Mid].id == INDEX)
            {
                return cards[Mid];
            }
            else if (cards[Mid].id > INDEX)
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
    public List<int>Ban_List=new List<int>();//생성효과로 생성하면 안되는 카드
    public List<SpellCardData> Special_cards = new List<SpellCardData>();//생성으로만 생성되는 카드들

}

