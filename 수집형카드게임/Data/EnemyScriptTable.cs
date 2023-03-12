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
    public List<int> SkillNumber;//리스트로바꿀까고민중 여러스킬발동하도록
    public List<int> SkillDamage;
    public List<int> SkillPercentage;
    public int AttackCount;

    public Sprite Boss_Image;
    public Material Boss_material;
    public Sprite Boss_BGImage;
    //Type
    //0/8파이어
    //1/9어스 
    //2/10프로스트
    //3/11스톰
    //4/12라이트닝
    //5/13섀도우
    //6/14아케인
    //7/15워터


    //스킬
    //0 실드
    //1 메테오






}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyDataObject", order = 1)]
public class EnemyScriptTable : ScriptableObject
{
    public List<EnemyData> Enemy = new List<EnemyData>();

}
