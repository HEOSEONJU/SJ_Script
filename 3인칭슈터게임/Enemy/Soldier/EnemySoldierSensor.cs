using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierSensor : MonoBehaviour
{
    public Camera cam;
    Plane[] plane;
    [SerializeField]
    EnemySoldierMove Move;
    [SerializeField]
    EnemySoldier_Manager _Manager;
    // Update is called once per frame
     void Start()
    {
        
        plane = GeometryUtility.CalculateFrustumPlanes(cam);
        
        
        
    }
    void Update()
    {
        plane=GeometryUtility.CalculateFrustumPlanes(cam);
        Collider[] Player = Physics.OverlapSphere(transform.position, 20, 1 << LayerMask.NameToLayer("Player"));

        foreach(var enemy in Player)
        {
            if(GeometryUtility.TestPlanesAABB(plane,enemy.bounds))
            {

                
                
                _Manager.Targeting(GameManager.instance.Char_Player_Trace.transform);
                
                //Move.SetTargeting(GameManager.instance.Char_Player_Trace.transform);
                return;
            }
            
        }
        
    }
}
