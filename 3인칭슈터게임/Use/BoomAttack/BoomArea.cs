using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetInfo : MonoBehaviour
{
    public Transform m_Target;
    public int m_ID=0;
}


public class BoomArea : MonoBehaviour
{
    public List<TargetInfo> TargetPosition;
    
    public List<GameObject> Bullet;
    public int SpwanCount;
    public int BulletCount;
    public int EnemyCount;
    public bool Check;
    public int Count;
    public int MAXCount;
    bool StartAttack;
    Ray ray;
    private void Start()
    {
        Count = 0;
        MAXCount=40;
        Check = false;
        BulletCount = 0;
        EnemyCount= 0;
        TargetPosition = new List<TargetInfo>();
        StartAttack = false;
        

        SpwanCount = transform.GetChild(3).GetComponent<Transform>().childCount;
        

        for(int i =0; i < SpwanCount; i++)
        {
            Bullet.Add(transform.parent.GetChild(1).GetChild(i).gameObject);
        }
        Invoke("StartAttackFunction", 5.0f);

        ray = new Ray();
        ray.origin = transform.position;
        ray.direction = transform.up;

        Destroy(transform.parent.gameObject, 26.0f);

    }
    void StartAttackFunction()
    {
        StartAttack= true;
        StartCoroutine(EnemySearch());
        if (TargetPosition.Count >= 1)
        {
            StartCoroutine(BoomStart());
            Check = true;
        }
    }
    
    IEnumerator BoomStart()
    {
        
        if(TargetPosition.Count == 0 ||MAXCount<=Count)
        {
            Check = false;
            if(MAXCount>=Count)
            {
                //Destroy(transform.parent.gameObject,0.3f);
            }
            yield break;
        }
        
        var e = Bullet[BulletCount].gameObject;

        
        e.GetComponent<BoomBullet>().ActiveBullet(TargetPosition[EnemyCount].m_Target);

        yield return new WaitForSeconds(0.5f);
        
        BulletCount++;
        
        
        if(BulletCount >= SpwanCount)
        {
            BulletCount = 0;
        }
        EnemyCount++;
        if(EnemyCount>=TargetPosition.Count)
        {
            EnemyCount=0;
        }
        Count++;
        if(TargetPosition.Count<=0)
        { 
            Check=false;
        }
        else
        StartCoroutine(BoomStart());
    }

    IEnumerator EnemySearch()
    {
        RaycastHit[] hit;
        List<TargetInfo> temp = new List<TargetInfo>();
        if (Physics.SphereCast(ray, 12.0f))
        {
            hit = Physics.SphereCastAll(ray, 12, 30);
            if (hit != null)
            {

                for (int i = 0; i < hit.Length; i++)
                {
                    int k = 0;
                    bool ck = true;
                    if (hit[i].transform.GetComponent<EnemyNumber>() != null)
                    {
                        //Debug.Log(hit[i].collider.gameObject.name);
                        switch (hit[i].transform.GetComponent<EnemyNumber>().EnemyNumberName)
                        {
                            case 1:
                                if (hit[i].transform.GetComponent<EnemyRobotHP>() != null)
                                {

                                    k = hit[i].transform.GetComponent<EnemyRobotHP>().ID;
                                }
                                break;
                            case 2:
                                if (hit[i].transform.GetComponent<EnemyOldTankHP>() != null)
                                {

                                    k = hit[i].transform.GetComponent<EnemyOldTankHP>().ID;
                                }
                                break;
                            case 3:

                                if (hit[i].transform.GetComponent<EnemySoldierHP>() != null)
                                {

                                    k = hit[i].transform.GetComponent<EnemySoldierHP>().ID;

                                }

                                break;
                            default:
                                k = 0;
                                break;
                        }

                        for (int j = 0; j < temp.Count; j++)
                        {
                            if (temp[j].m_ID == k)
                            {

                                ck = false;
                            }
                        }
                        if (ck == true)
                        {
                            TargetInfo targetInfo = new TargetInfo();
                            targetInfo.m_Target = hit[i].transform;
                            targetInfo.m_ID = k;
                            temp.Add(targetInfo);
                        }
                    }
                }
                TargetPosition = temp;
                
            }

        }
        if(Check==false & TargetPosition.Count>=1)
            StartCoroutine(BoomStart());
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(EnemySearch());
    }
   




}
