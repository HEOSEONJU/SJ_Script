using System.Collections;
using System.Collections.Generic;
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
    public int AttackType;//0 물리 1마법
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

}


//히트타입물리
//0,1슬래시
//3,4크로스슬래시
//5,6주먹
//7,8스크래치
//8,9,10슬래시
//11파이어
//12프로스트
//13스톰
//14라이트닝
//15섀도우
//16아케인
//17워터


//히트타입마법

//슬래시
//0파이어
//1어스 
//2프로스트
//3스톰
//4라이트닝
//5섀도우
//6아케인
//7워터
//8라이프
//9라이트
//스피어블래스트
//10파이어
//11어스 
//12프로스트
//13스톰
//14라이트닝
//15섀도우
//16아케인
//17워터
//18라이프
//19라이트
//데미지오버타임
//20파이어
//21어스 
//22프로스트
//23스톰
//24라이트닝
//25섀도우
//26아케인
//27워터
//28라이프
//29라이트