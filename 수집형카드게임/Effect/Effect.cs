using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract  class Effect :MonoBehaviour
{
    [SerializeField]
    protected int Turn;//0이면단발 5000이면영구 그외는 지속턴
    public bool Require;//발동가능한지 반환
    public abstract void RequireMent(Manager Manager);//발동조건을 확인하는 함수
    public abstract void Effect_Function(Manager _Manager);//플레이어에게 발동
    public abstract void Effect_Solo_Function(Manager _Manager);//단일 캐릭터에게 발동
    public abstract void Effect_Enemy_Function(Manager _Manager);//적에게 효과를주는 발동
    public abstract void Damage_Enemy_Function(EnemyManager Target);//적에게 데미지를 주는 효과 발동
}
public enum BUFF_TYPE
{
    Heal,
    ATK,
    DEF,
    MAXHP,
    PATK,
    PDEF,
    PHP,
    MG,
    Regen,
    AtC,
    CP,
    CD,
    DC
}
[System.Serializable]
public class Effect_Value
{ 
    public int Value;
    public BUFF_TYPE TYPE;
}
