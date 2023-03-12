using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossBulletAttack : MonoBehaviour
{
    
    public ParticleSystem LParticle, RPartlcle;
    public Transform Targeting;
    public GameObject BulletParent;
    List<GameObject> list = new List<GameObject>();
    
    public int count;
    bool Stop;
    public bool StartAttack;
    private void Awake()
    {
        for(int i=0; i <BulletParent.transform.childCount; i++)
        {
            list.Add(BulletParent.transform.GetChild(i).gameObject);
        }
        for(int i=0;i<list.Count;i++)
        {
            list[i].transform.parent= null;
        }
        Stop = false;
        StartAttack = false;
    }

    public void FindPooling(Transform posi)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.parent = posi;
            list[i].GetComponent<EnemyBossBullet>().FindPooling(posi);
        }
    }

    public  void StopAttack()
    {
        Stop = true;
    }

    public void AttackBullet(Transform CenterPoint)
    {
        count = 0;
        Stop = false;
        Targeting = CenterPoint;
        StartAttack = true;
        StartCoroutine(AttackFunction());
        
    }

    IEnumerator AttackFunction( )
    {

        if (count >= list.Count || Stop==true || StartAttack == false) 
        {
           
           


            float Delay = 5f;

            yield return new WaitForSeconds(Delay);
            count = 0;
            StartAttack = false;



        }
        else
        {
            

            LParticle.Play();

            list[count].GetComponent<EnemyBossBullet>().FireBullet(Targeting,LParticle.transform.position);
            RPartlcle.Play();
            list[count+1].GetComponent<EnemyBossBullet>().FireBullet(Targeting, RPartlcle.transform.position);
            yield return new WaitForSeconds(0.2f);
            count+=2;
            StartCoroutine(AttackFunction());
        }   
    }

}

