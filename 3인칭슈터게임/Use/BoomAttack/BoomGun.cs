using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomGun : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TargetMarker;
    
    private void Start()
    {
        
    }
    


    public void Shoot(Transform cam,LayerMask layer)
    {
//        Gun Mian= GetComponent<Gun>();
//        GameObject posi = Mian.L1;
        RaycastHit hit;
        
        
        //if (Physics.Raycast(posi.transform.position, posi.transform.forward, out hit, 300.0f, layer))
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 300.0f, layer))
        {
            if(TargetMarker!=null)
            {    
                var e = Instantiate(TargetMarker);
                e.transform.position = hit.point;
                e.transform.GetChild(0).GetChild(0).transform.rotation= Quaternion.LookRotation(hit.normal);
                e.transform.GetChild(0).GetChild(1).transform.position = new Vector3(hit.point.x, GameManager.instance.Char_Player_Trace.transform.position.y, hit.point.z);
            }
        }
    }
}
