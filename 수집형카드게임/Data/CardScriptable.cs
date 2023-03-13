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
    public int CardType;//0������ / 1������ // 2��ü
    //public int SpellType;//0 0�̸鵥���� 1�̸���ٿ� 2�̸��ٿ� 3CC?
                         //1 0�̸�ȸ�� 1�̸���� 2�̸��� 3�̸��ִ�ü��ȸ�� 
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
    public SpellCardData Find_Spell_Card(int INDEX)//����Ž��
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
    public List<int>Ban_List=new List<int>();//����ȿ���� �����ϸ� �ȵǴ� ī��
    public List<SpellCardData> Special_cards = new List<SpellCardData>();//�������θ� �����Ǵ� ī���

}

