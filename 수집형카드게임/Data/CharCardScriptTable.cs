using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[System.Serializable]
public class CharCardData
{
    public int id;
    public string CardName;
    public int Level;
    public int ATK;
    public int DEF;
    public int HP;
    public int Rank;
    public Sprite Image;
    public Sprite BGImage;
    public Sprite RankImage;
    public Sprite Small_Image;

    public int CritiaclPercent = 10;
    public int CritiaclDamage = 50;
    public int AttackType;//0 ���� 1����
    public List<int>AttackEffectNum;
    public int UpAtk;
    public int UpDef;
    public int UpHp;

    public List<int> Skill_ID;


    //public Material material;
    public Material Title_material;


}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharDataObject", order = 1)]
public class CharCardScriptTable : ScriptableObject
{
    public List<CharCardData> Monster = new List<CharCardData>();

    public CharCardData Find_Char(int INDEX)//����Ž��
    {

        if (Monster.Count == 0)
        {
            return null;
        }
        int Low = 0;
        int High = Monster.Count - 1;
        int Mid;
        while (Low <= High)
        {
            Mid = (Low + High) / 2;
            if (Monster[Mid].id == INDEX)
            {
                return Monster[Mid];
            }
            else if (Monster[Mid].id > INDEX)
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


//��ƮŸ�Թ���
//0,1������
//3,4ũ�ν�������
//5,6�ָ�
//7,8��ũ��ġ
//8,9,10������
//11���̾�
//12���ν�Ʈ
//13����
//14����Ʈ��
//15������
//16������
//17����


//��ƮŸ�Ը���

//������
//0���̾�
//1� 
//2���ν�Ʈ
//3����
//4����Ʈ��
//5������
//6������
//7����
//8������
//9����Ʈ
//���Ǿ����Ʈ
//10���̾�
//11� 
//12���ν�Ʈ
//13����
//14����Ʈ��
//15������
//16������
//17����
//18������
//19����Ʈ
//����������Ÿ��
//20���̾�
//21� 
//22���ν�Ʈ
//23����
//24����Ʈ��
//25������
//26������
//27����
//28������
//29����Ʈ