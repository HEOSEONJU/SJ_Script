using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Effect : Base_Effect
{
    [SerializeField]
    int Damage_Value;
    [SerializeField]
    Attack_Effect_Type TYPE;
    

    public override void Damage_Enemy_Function(EnemyManager Target)
    {
        Target.Magic_Attack(Damage_Value, TYPE);
    }
}
public enum Attack_Effect_Type
{
    Fire=0,
    Earth=1,
    Frost=2,
    Storm=3,
    Lighting=4,
    Arcane=5,
    Water=6,
    Life=7,
    Light=8,
    FireSphere=9,
    EarthSphere=10,
    FrosotSphere=11,
    StormSphere=12,
    LightingSphere=13,
    ArcaneSphere=14,
    WaterSphere=15,
    LifeSphere=16,
    LightSphere=17,
    FireOver=18,
    EarthOver=19,
    FrostOver=20,
    StormOver=21,
    LightingOver=22,
    ArcaneOver=23,
    WaterOver=24,
    LifeOver=25,
    LightOver=26


}
//마법 0 ~10
//0~2 블루
//3~5 레드
//6~9 화이트
//10 옐로우

//물리 11~21
//11 랜덤방향베기
//12십자베기
//13~14 아웃터??? 이해안됨

//15~16 펀치//주먹캐릭터
//17~18 스크래치//짐승형
//19~21 베기