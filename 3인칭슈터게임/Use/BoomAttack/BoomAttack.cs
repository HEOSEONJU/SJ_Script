using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomAttack : MonoBehaviour
{
    int Damage;
    public RaycastHit[] hit;
    private void OnEnable()
    {
        Damage = 100;
        LayerMask mask = LayerMask.NameToLayer("Enemy");
        
        hit = Physics.SphereCastAll(transform.position, 5.0f, transform.forward, 0.1f);




        if (hit != null)
        {
            List<int> attackEnemy = new List<int>();
            for (int i = 0; i < hit.Length; i++)
            {
                int k = 0;
                bool ck = true;
                if (hit[i].transform.GetComponent<EnemyNumber>() != null)
                {
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
                        case 4:

                            if (hit[i].transform.GetComponent<EnemyBossHP>() != null)
                            {

                                k = hit[i].transform.GetComponent<EnemyBossHP>().ID;
                            }
                            break;

                        default:
                            k = 0;
                            break;
                    }


                    for (int j = 0; j < attackEnemy.Count; j++)
                    {
                        if (attackEnemy[j] == k)
                        {
                            ck = false;
                        }
                    }

                    if (ck == true)
                    {
                        attackEnemy.Add(k);
                        Debug.Log("hit!");
                        if (hit[i].collider.gameObject.layer == 10)
                        {

                            if (hit[i].transform.GetComponent<EnemyNumber>() != null)
                            {
                                switch (hit[i].transform.GetComponent<EnemyNumber>().EnemyNumberName)
                                {
                                    case 1:
                                        if (hit[i].transform.GetComponent<EnemyRobotHP>() != null)
                                        {

                                            hit[i].transform.GetComponent<EnemyRobotHP>().Damged(Damage);
                                        }
                                        
                                        break;
                                    case 2:
                                        if (hit[i].transform.GetComponent<EnemyOldTankHP>() != null)
                                        {

                                            hit[i].transform.GetComponent<EnemyOldTankHP>().Damged(Damage*2);
                                        }
                                        
                                        break;
                                    case 3:
                                        
                                        if (hit[i].transform.GetComponent<EnemySoldierHP>() != null)
                                        {

                                            hit[i].transform.GetComponent<EnemySoldierHP>().Damged(Damage*2);
                                        }
                                        break;
                                    case 4:

                                        if (hit[i].transform.GetComponent<EnemyBossHP>() != null)
                                        {

                                            hit[i].transform.GetComponent<EnemyBossHP>().Damged(Damage);
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    else if (hit[i].collider.gameObject.layer == 9)
                    {

                        GameManager.instance.PlayerDamage(100);

                    }



                }
            }
        }
    }
        




}


