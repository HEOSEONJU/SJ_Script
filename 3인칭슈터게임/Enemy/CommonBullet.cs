using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonBullet : Base_Bullet
{
    public int BulletNumber;
    public float  HP;//총알의 체력
    public float diff;//2번째총 데미지배율
    public float speed; //총알의속도 클수록 느림
    public GameObject Effect;


    public void Awake()
    {
        HP = 10;
        diff = 1;
        speed = 1;
    }

    protected void MoveBullet(float BulletSpeed)
    {
        transform.position += transform.forward / BulletSpeed;
    }
    public void DestroyFisrtGun(float a)
    {
        
        HP -= a;
        if (HP < 0)
        {
            SettingBullet();
            Debug.Log("총알격파");
        }
    }
    public void DestroySecondGun(float a)
    {
        HP -= a*diff;
        if (HP < 0)
        {
            SettingBullet();
        }
    }

    public void SettingBullet()
    {
        Effect.transform.position = transform.position;
        Effect.transform.rotation = Quaternion.identity;
        Effect.SetActive(true);
        switch(BulletNumber)
        {
            case 2:
                Effect.GetComponent<OldTanKEXP>().StartAction();
                break;
            case 3:
                Effect.GetComponent<EnemySoldierExp>().DoSetoff();
                break;
            default:
                break;
        }
        
        gameObject.SetActive(false);
        
    }
    public override void Damaged(float a)
    {
        HP -= a;
        if (HP < 0)
        {
            SettingBullet();
            Debug.Log("총알격파");
        }
    }
}
