using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract  class Effect :MonoBehaviour
{
    [SerializeField]
    protected int Turn;//0�̸�ܹ� 5000�̸鿵�� �׿ܴ� ������
    public bool Require;//�ߵ��������� ��ȯ
    public abstract void RequireMent(Manager Manager);//�ߵ������� Ȯ���ϴ� �Լ�
    public abstract void Effect_Function(Manager _Manager);//�÷��̾�� �ߵ�
    public abstract void Effect_Solo_Function(Manager _Manager);//���� ĳ���Ϳ��� �ߵ�
    public abstract void Effect_Enemy_Function(Manager _Manager);//������ ȿ�����ִ� �ߵ�
    public abstract void Damage_Enemy_Function(EnemyManager Target);//������ �������� �ִ� ȿ�� �ߵ�
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
