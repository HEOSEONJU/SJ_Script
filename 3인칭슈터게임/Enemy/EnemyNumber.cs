using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNumber : MonoBehaviour
{
        public int EnemyNumberName;
        public bool LiveState;
    

    private void Awake()
    {
        switch (EnemyNumberName)
        {
            case 1://로봇
                if(GetComponent<EnemyRobotHP>()!=null)
                LiveState=GetComponent<EnemyRobotHP>().Live;
                break;
            case 2://올드탱크
                if (GetComponent<EnemyOldTankHP>() != null)
                    LiveState =GetComponent<EnemyOldTankHP>().Live;
                break;
            case 3://솔저
                if (GetComponent<EnemySoldierHP>() != null)
                    LiveState =GetComponent<EnemySoldierHP>().Live;
                break;
            default:
                break;
        }
    }
}
