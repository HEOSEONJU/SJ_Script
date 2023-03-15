using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{

    public float DelayTrigger = 0;
    public NavMeshAgent agent = null;
    public Transform[] Waypoint = null;
    public int WayCount;
    public Transform Target = null;  

    public Animator animator;

    

    
    // Start is called before the first frame update




    public virtual void Cal_Dis()
    {
        return;
    }
    public virtual void Move_Target()
    {
        return;
    }
    public virtual void Follow_Target()
    {
        return;
    }
    public virtual void Stop_Enemy()
    {
        return;
    }
    public virtual float Distance()
    {
        return 1f;
    }
    public virtual bool RobotSearchPlayer(Vector3 Pos, Vector3 forwardDir, float angle, Vector3 targetPos, float distance)//자기위치,전방벡터,앵글,타겟위치,최대탐색거리
    {
        return false;
    }
    public virtual void CannonShot()
    {

    }
}
