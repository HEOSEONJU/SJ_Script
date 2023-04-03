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
    protected int ATK;

    public Monster_Size MS;
    public int ID;
    public Transform Object_Postion;

    public ParticleSystem baseParticle;



    public bool Live = true;


    [SerializeField]
    Drop_Table_Script DTS;

    public void init()
    {
        
        HP = Data.MAXHP;
        Speed = Data.SPeed;
        ATK = Data.Attack;
        Live = true;
        Object_Postion = transform;
        ID = gameObject.GetInstanceID();
    }
    public abstract void Drop(int Per);
    public abstract bool Damaged(float Damaged_Point,Transform Player, int Type = 0);//��������,�÷��̾���ġ,����Ʈ�߻�����,����Ÿ��
    public abstract void Died();//���

}
public enum Monster_Size
{
    S,
    M,
    B
}