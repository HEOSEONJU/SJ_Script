using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRobotAttack : MonoBehaviour
{
    
    public GameObject  LaserBeam;
    public GameObject bullet;
    LayerMask layerMask;
    RaycastHit hit;
    //Transform TargetInfo;
     void Update()
    {
        //if(Input.GetKeyUp(KeyCode.Space))
        //{
        //    DangerMarker();
        //}
    }
    public void DangerMarker(Transform TargetInfo)
    {
        Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        transform.rotation = Quaternion.LookRotation(TargetInfo.position - transform.position);//적방향으로 회전
        Physics.Raycast(NewPosition, -1 * transform.right, out hit, 30000);
        //Debug.DrawRay(NewPosition, transform.right*-100, Color.black, 10000.0f); 
        GameObject Clone = Instantiate(LaserBeam, NewPosition, TargetInfo.rotation);
        Clone.GetComponent<DangerLine>().Endpoint = hit.point;
        Clone.GetComponent<DangerLine>().bullet = bullet;

    }
    //public void DangerMarker()
    //{
    //    Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    //    if (Physics.Raycast(NewPosition, -1 * transform.right, out hit, 30000))
    //    { //Debug.DrawRay(NewPosition, transform.right*-100, Color.black, 10000.0f); 
    //        TargetInfo.rotation = Quaternion.LookRotation(TargetInfo.position - transform.position);//적방향으로 회전
            
    //            Debug.Log(1);
    //            GameObject Clone = Instantiate(LaserBeam, NewPosition, TargetInfo.rotation);
    //            Clone.GetComponent<DangerLine>().Endpoint = hit.point;
    //            Clone.GetComponent<DangerLine>().bullet = bullet;
            
    //    }
    //}
}
