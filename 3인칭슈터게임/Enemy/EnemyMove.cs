using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    
    
    public NavMeshAgent agent = null;
    public Transform[] Waypoint = null;
    public int WayCount;
    public Transform Target = null;
    
    public bool LockOn = false;
    public float Delay;
    public float AttackDelay;
    public Animator animator;
    
    
    
    public float Aggro;
    public float dis;
    public float MaxDis;
    public float MaxAggro;
    // Start is called before the first frame update


    public void SetTargeting(Transform PlayerPosi)
    {
        CancelInvoke();
        Target = PlayerPosi;
        Aggro = MaxAggro;
    }

    public void ReMoveTargeting()
    {
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
        
        Target = null;
    }

    public void LockOnTarget()
    {
        LockOn = true;
        
    }
    public void LostOnTarget()
    {
        LockOn = false;

    }

}
