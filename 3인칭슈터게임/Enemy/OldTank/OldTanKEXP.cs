using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTanKEXP : MonoBehaviour
{

    LayerMask Player;
    LayerMask Enemy;

    
    private void OnEnable()
    {
        int count = 0;
        Player = 9;
        Enemy = 10;




        RaycastHit[] hitInfos;


        hitInfos = Physics.SphereCastAll(transform.position, 4f, transform.forward, 0.001f);
        for (int i = 0; i < hitInfos.Length; i++)
        {
            
                
            if (hitInfos[i].collider.gameObject.layer == Enemy)
            {
                if (hitInfos[i].collider.transform.tag != "Head")
                {
                    if (hitInfos[i].transform.GetComponent<EnemyNumber>() != null)
                    {
                        switch (hitInfos[i].transform.GetComponent<EnemyNumber>().EnemyNumberName)
                        {
                            case 3://솔저
                                EnemySoldierHP tempSoldier = hitInfos[i].transform.GetComponent<EnemySoldierHP>();
                                if (tempSoldier.Live)
                                {

                                    tempSoldier.Damged(100);
                                }
                                break;
                            case 4://보스
                                EnemyBossHP tempBoss= hitInfos[i].transform.GetComponent<EnemyBossHP>();
                                if (tempBoss.Live)
                                {

                                    tempBoss.Damged(100);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            if (hitInfos[i].collider.gameObject.layer == Player)
            {
                
                count++;
                
            }

        }
        if(count > 0)
        {

            GameManager.instance.PlayerDamage(200);
        }
    }


    public void StartAction()
    {
        StartCoroutine(Setactiveoff());

    }
    IEnumerator Setactiveoff()
    {

        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
}
