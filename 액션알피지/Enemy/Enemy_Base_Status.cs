
using UnityEngine;
using Hit_Type_NameSpace;
using Enemy;
public abstract class Enemy_Base_Status : MonoBehaviour
{

    [SerializeField]
    protected Monset_Data Data;
    [SerializeField]
    protected float HP;
    protected float Speed;
    protected int ATK;

    public EMonster_Size MS;
    public int ID;
    public Transform Object_Postion;

   // public ParticleSystem baseParticle;



    public bool Live = true;


    [SerializeField]
    protected Drop_Table_Script DTS;


    public float Aggro_Time;
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
    public abstract bool Damaged(float Damaged_Point,Transform Player, EAttack_Special AC);//데미지량,플레이어위치,이펙트발생지점,공격타입
    public abstract void Died();//사망

}
