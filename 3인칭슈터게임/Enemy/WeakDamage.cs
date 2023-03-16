using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakDamage : MonoBehaviour
{
    
    public EnemyOldTankHP THP;
    public EnemySoldierHP SHP;
    public EnemyRobotHP RHP;

    public void WeakDamaged(int num,int damge)
    {
        Debug.Log("headshot");
        switch (num)
        {
            case 1:
                RHP.Damged(damge);
                break;
            case 2:

                THP.Damged(damge);

                break;
            case 3:
                SHP.Damged(damge);
                break;
            default:
                break;
        }

    }


}
