using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldierMove : EnemyMove
{
    [SerializeField]
    public EnemySoldier_Manager _Manager;
    
    public Vector3 Myposi;
    public float SquadDis;

    public float Distance_value;
    public override float Distance()
    {

        return Distance_value;


    }
    public override void Cla_Dis()
    {
        if (_Manager.Target == null)
        {
            Distance_value= Mathf.Infinity;
            return;
        }

         Distance_value= Vector3.Distance(_Manager.Target.position, transform.position);
    }
    public override void Follow_Target()
    {
        agent.SetDestination(_Manager.Target.position); 

        agent.speed = 4.5f;
        
        float dis = Vector3.Distance(_Manager.Target.position, transform.position);
        if (dis > _Manager.MAX_Dis)
            _Manager.Aggro -= Time.deltaTime;
    }
    public override void Move_Target()
    {
        if (Waypoint.Length == 0)
        {
            return;
        }
        
        agent.speed = 3.5f;
        
        

        agent.SetDestination(Waypoint[WayCount].position);

    }
    public override void Stop_Enemy()
    {
        agent.speed = 3.5f;
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
    }



    public void OnTriggerEnter(Collider other)
    {
        if (Waypoint.Length == 0)
        {
            return;
        }
        if (_Manager.Target == null & DelayTrigger >= 1.0f)
        {
            if (other.tag == "Patroll" && Waypoint[WayCount].gameObject.name == other.name)
            {
                DelayTrigger = 0.0f;
                WayCount++;
                if (WayCount >= Waypoint.Length)
                {
                    WayCount = 0;
                }
                //agent.SetDestination(Waypoint[WayCount].position);

                
            }
        }
    }
}
