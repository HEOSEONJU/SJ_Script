using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRegen : MonoBehaviour
{
    public GameObject Boss;
    public Transform Position;
    public GameUIManager manager;

    public Transform Pooling;
    void Start()
    {
        var E = Instantiate(Boss, Position.position, Position.rotation);
        manager.InitBoss(E.GetComponent<EnemyBossHP>());
        E.transform.GetChild(0).GetComponent<EnemyBossBulletAttack>().FindPooling(Pooling);
        E.transform.GetChild(0).GetComponent<EnemyBossRocketAttack>().FindPooling(Pooling);
        GameManager.instance.InitBoss(E.GetComponent<EnemyBossMove>());
    }

}
