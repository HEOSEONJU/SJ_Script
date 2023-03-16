using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossHP : EnemyHp
{
    [SerializeField]
    Boss_Manager _Manager;

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
    [SerializeField]
    Rigidbody R;
    public int MAXHP=20000;
    



    private void Start()
    {
        InstID();
        Live = true;
        hp = 20000;
        ShieldPoint = MAXShield = 1000;
        ShieldCheck = true;
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
        
        if (hp <= 0 & Live == true)
        {
            
            Live = false;
            StartCoroutine(DieEnemy());
        }

    }

    public void  BrokeShield()
    {
        _Manager.SHB();
        

        foreach(GameObject GO in Shield)
        {
            GO.SetActive(false);
        }
        
        
        ShieldCheck = false;
        transform.GetChild(0).GetComponent<EnemyBossBulletAttack>().StopAttack();
        transform.GetChild(0).GetComponent<EnemyBossRocketAttack>().StopAttack();


        
        
        
    }
    public void RegenShieldFunction()
    {
        if (Live == true)
        {
            ShieldCheck = true;
            foreach (GameObject GO in Shield)
            {
                GO.SetActive(true);
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
