using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy_Base_Status : MonoBehaviour
{
    [SerializeField]
    protected Monset_Data Data;
    [SerializeField]
    protected float HP;
    protected float Speed;
    protected float ATK;

    public int ID;
    public Transform Object_Postion;

    public ParticleSystem baseParticle;

    public bool Live = true;
    public void init()
    {
        
        HP = Data.MAXHP;
        Speed = Data.SPeed;
        ATK = Data.Attack;
        Live = true;
        Object_Postion = transform;
        ID = Random.Range(1, 1000);
    }
    public abstract void Drop(int Per);
    public abstract bool Damaged(float Damaged_Point,Transform Player, int Type = 0);//데미지량,플레이어위치,이펙트발생지점,공격타입
    public abstract void Died();//사망

}
