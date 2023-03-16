using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotHP : EnemyHp
{
    
    
    void Awake()
    {
        Live = true;
        hp = 100;
    }
    public override void Damged(int Da,bool special=false)
    {
        hp -= Da;

        if (hp <= 0 & Live == true)
            StartCoroutine(DieEnemy());
    }
    IEnumerator DieEnemy()
    {
        Live = false;
        GameObject p = transform.parent.gameObject; ;
        p.transform.GetComponent<RobotMove>().Live = false;
        p.transform.LookAt(GameManager.instance.Char_Player_Trace.transform.position);
        Animator animator = transform.GetComponentInParent<Animator>();
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(2.0f);
        
        Destroy(p);
    }
}
