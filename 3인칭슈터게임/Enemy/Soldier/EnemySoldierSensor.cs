using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierSensor : MonoBehaviour
{
    public Camera cam;
    Plane[] plane;
    EnemySoldierMove Move;
    
    // Update is called once per frame
     void Start()
    {
        //cam = GetComponent<Camera>();
        plane = GeometryUtility.CalculateFrustumPlanes(cam);
        
        Move=GetComponent<EnemySoldierMove>();
        
    }
    void Update()
    {
        plane=GeometryUtility.CalculateFrustumPlanes(cam);
        Collider[] Player = Physics.OverlapSphere(transform.position, 20, 1 << LayerMask.NameToLayer("Player"));

        foreach(var enemy in Player)
        {
            if(GeometryUtility.TestPlanesAABB(plane,enemy.bounds))
            {
                Move.LockOnTarget();
                Move.SetTargeting(enemy.transform);
                
            }
            
        }
        
    }
}
