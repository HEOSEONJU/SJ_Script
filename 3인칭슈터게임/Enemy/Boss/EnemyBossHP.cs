using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossHP : EnemyHp
{

    public List<GameObject> Shield;
    public ParticleSystem DieEff;
    public ParticleSystem ShieldDestroy;
    public Animation Ani;
    public Animator animator;
    public EnemyBossMove Move;
    public AudioSource DieSound;


    public int ShieldPoint;
    public int MAXShield;
    public bool ShieldCheck;
    public int Regentime = 15;
    Rigidbody R;
    public int MAXHP=20000;
    public void Awake()
    {
        R = GetComponent<Rigidbody>();
        Live = true;
        hp = 20000;
        ShieldPoint = MAXShield = 1000;
        //Ani = transform.GetChild(0).transform.GetChild(3).GetComponent<Animation>();
        //DieEff = transform.GetChild(0).transform.GetChild(4).GetComponent<ParticleSystem>();
        
        ShieldCheck = true;
    }



    private void Start()
    {
        InstID();
    }

    

    
    public override void Damged(int Da,bool special= false)
    {
        if (ShieldPoint > 0)
        {
            if (special)
            {
                Da *= 3;
                if(Da<=ShieldPoint)
                {
                    ShieldPoint -= Da;
                }
                else
                {
                    ShieldPoint = 0;
                    BrokeShield();
                }
            
            }
            else
            {
                     if (ShieldPoint <= Da)
                    {
                        Da -= ShieldPoint;
                        ShieldPoint -= ShieldPoint;
                        hp -= Da;
                        BrokeShield();
                    }
                    ShieldPoint -= Da;
            }
        }
        else
        {
            hp -= Da;
        }
        //Move.SetTargeting(GameManager.instance.Char_Player_Trace.transform);
        //Move.LockOnTarget();
        if (hp <= 0 & Live == true)
        {
            
            Live = false;
            StartCoroutine(DieEnemy());
        }

    }

    public void  BrokeShield()
    {
        
        ShieldDestroy.Play();
        for (int i = 0; i < Shield.Count; i++)
        {
            Shield[i].SetActive(false);
        }
        
        ShieldCheck = false;
        transform.GetChild(0).GetComponent<EnemyBossBulletAttack>().StopAttack();
        transform.GetChild(0).GetComponent<EnemyBossRocketAttack>().StopAttack();


        Move.OverHeat();
        
        
    }
    public void RegenShieldFunction()
    {
        if (Live == true)
        {
            ShieldCheck = true;
            for (int i = 0; i < Shield.Count; i++)
            {
                Shield[i].SetActive(true);
            }
            if (MAXShield <= 2500)
            {
                MAXShield += 500;
                ShieldPoint = MAXShield;
            }
            Move.CoolDownAction();

        }
    }
    IEnumerator DieEnemy()
    {
        

        
        R.isKinematic = true;
        
        R.velocity = Vector3.zero;
        

        animator.SetTrigger("Die");
        
        DelID();

        GameManager.instance.Score = Mathf.Floor(GameManager.instance.Score);
        if(GameManager.instance.Score<0)
        {
            GameManager.instance.Score = 0;
        }
        GameManager.instance.PlayerVictory();
        

        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }


}
