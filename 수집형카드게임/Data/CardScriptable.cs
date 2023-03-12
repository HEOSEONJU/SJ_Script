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




    //public int HEAL;
    //public int Value_Enemy_Damage;
    public int Value_Enemy_Damage_Effect;
    /*
    public int Value_A;
    public int Value_D;
    public int Value_H;
    public int Value_PA;
    public int Value_PD;
    public int Value_PH;
    public int Value_R;

    public int Value_AC;
    public int Value_TY;
    public int Value_MR;
    public int Value_CP;
    public int Value_CD;
    public int Value_DC;
    public int Value_Cost;
    public int Value_MAXCost;
    public int Value_Char_Damage;

    public int Value_Drew;
    public int Value_Hand_Less;
    public int Value_Create_Deck;
    public int Value_Create_Deck1;
    public int Value_Create_Deck2;
    public int Value_Create_Deck3;
    public int Value_Create_Deck4;
    public int Turn;//지속턴
    public bool Special = false;
    */
}
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellCardDataObject", order = 1)]
public class CardScriptable : ScriptableObject
{
    

    public List<SpellCardData> cards = new List<SpellCardData>();


    public SpellCardData Find_Spell_Card(int INDEX)
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

    public List<int>Ban_List=new List<int>();
    public List<SpellCardData> Special_cards = new List<SpellCardData>();

}

