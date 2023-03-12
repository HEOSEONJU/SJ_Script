using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharSkillData
{

    public Sprite Image;
    public int id;
    public string SkillName;
    public string exp;
    
    
    
    public int CardType;//0������ / 1������ // 2��ü
    public int SpellType;//0 0�̸鵥���� 1�̸���ٿ� 2�̸��ٿ� 3CC?
                         //1 0�̸�ȸ�� 1�̸���� 2�̸��� 3�̸��ִ�ü��ȸ�� 

    public GameObject Effects;
    public int HEAL;
    public int Value_Enemy_Damage;

    public int Value_Enemy_Effect_Num;
    public int Value_Char_Effect_Num;
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
    public int Turn;//������
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharSkillDataObject", order = 1)]
public class Char_Skill_ScriptTable : ScriptableObject
{
    public List<CharSkillData> Skill = new List<CharSkillData>();

    public CharSkillData Find_Skill(int INDEX)
    {

        if (Skill.Count== 0)
        {
            return null;
        }
        int Low = 0;
        int High = Skill.Count - 1;
        int Mid;
        while (Low <= High)
        {
            Mid = (Low + High) / 2;
            if (Skill[Mid].id == INDEX)
            {
                return Skill[Mid];
            }
            else if (Skill[Mid].id > INDEX)
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

}
