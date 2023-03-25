using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyBossRocketAttack : MonoBehaviour
{
    
    
    public Transform Targeting;
    public GameObject RocketParent;
    public List<GameObject> list = new List<GameObject>();
    public List<Transform> StartPoint;
    public int count,StartCount;
    public bool StartFire;
    bool Stop;
    private void Awake()
    {
        for (int i = 0; i < RocketParent.transform.childCount; i++)
        {
            list.Add(RocketParent.transform.GetChild(i).gameObject);
        }
        RocketParent.transform.parent = null;
        
        Stop = false;

        StartFire = false;
    }

    public void FindPooling(Transform posi)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.parent = posi;
            list[i].GetComponent<EnemyBossEXP>().FindPooling(posi);
        }
    }

    public void StopAttack()
    {
        if (StartFire == true)
        {
            Stop = true;
            StartFire = false;
        }
    }

    public void AttackRocket(Transform CenterPoint)
    {
        StartFire = true;
        count = 0;
        StartCount = 0;
        Stop = false;
        Targeting = CenterPoint;
        StartCoroutine(AttackFunction());
    }

    IEnumerator AttackFunction()
    {

        while (count < list.Count)
        {
            if (Stop) break;
            Vector3 Sp = StartPoint[StartCount++].position;
            if (StartCount >= StartPoint.Count)
            {
                StartCount = 0;
            }


            list[count++].GetComponent<EnemyBossEXP>().FireBullet(Targeting, Sp);

            yield return new WaitForSeconds(1.0f);
            
            
        }
        Stop = false;
        count = 0;
        yield return new WaitForSeconds(7.5f);
        StartFire = false;

    }
    private void OnDisable()
    {
        Destroy(RocketParent);
    }

}
