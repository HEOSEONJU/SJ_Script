using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EnemyHp : Base_HP
{

    protected void InstID()
    { 
        while (true)
        {
            ID = Random.Range(1, 10000);
            if (GameManager.instance.InstID(ID))
            {
                
                break;
            }
        }
    }
    protected void DelID()
    {
        GameManager.instance.EnemyID.Remove(ID);
    }

    public override void Damged(int a, bool special = false)
    {
        return;
    }

}

