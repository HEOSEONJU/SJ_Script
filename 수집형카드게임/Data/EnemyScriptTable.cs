using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class EnemyData
{
    public int Enemy_id;
    public string Name;
    public int Base_ATK;
    public int Base_DEF;
    public int Base_HP;
    public int Magic_Regi;
    public int TYPE;
    public List<int> SkillNumber;//����Ʈ�ιٲܱ����� ������ų�ߵ��ϵ���
    public List<int> SkillDamage;
    public List<int> SkillPercentage;
    public int AttackCount;

    public Sprite Boss_Image;
    public Material Boss_material;
    public Sprite Boss_BGImage;
    //Type
    //0/8���̾�
    //1/9� 
    //2/10���ν�Ʈ
    //3/11����
    //4/12����Ʈ��
    //5/13������
    //6/14������
    //7/15����


    //��ų
    //0 �ǵ�
    //1 ���׿�






}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataObject", order = 1)]
public class EnemyScriptTable : ScriptableObject
{
    public List<EnemyData> Enemy = new List<EnemyData>();

}
