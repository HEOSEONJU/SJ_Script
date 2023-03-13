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
    
    public int Value_Enemy_Damage;

    public int Value_Enemy_Effect_Num;
    public int Value_Char_Effect_Num;

    public int Turn;//������
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharSkillDataObject", order = 1)]
public class Char_Skill_ScriptTable : ScriptableObject
{
    public List<CharSkillData> Skill = new List<CharSkillData>();

    public CharSkillData Find_Skill(int INDEX)//����Ž��
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
