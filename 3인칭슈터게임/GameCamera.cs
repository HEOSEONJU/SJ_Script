using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameCamera : MonoBehaviour //사용중인클래스
{
    public float min=-60;
    public float max=60;
    public float currXRot = 0;
    public float vertical;
    public Transform OriginPos, PlayerPos;
    public Transform Sideposi;
    public Transform Wall;
    
    public Vector3 ch;
    public bool Check;
    
    public bool side;

    RaycastHit hit;
    RaycastHit[] hits;


    public Gun AimCheck;
    public bool Action;
    // Start is called before the first frame update
    void Start()
    {
        vertical = 0;
        Check = false;
        side = false;
        ch = OriginPos.position;
        hits = null;
        Action = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Action & GameManager.instance.Hp>0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                side = !side;
            }
            //CameraWallColl();
            CameraMoving();
            YRotateCamera();
        }
    }

    bool CameraWallColl()
    {
        bool ck = false;
         float dis=3.0f;
        
        Vector3 rot=Vector3.zero;
        if (side == true)
        {
            dis = Vector3.Distance(Sideposi.position, PlayerPos.position);
            rot = Sideposi.position - PlayerPos.position;
        }
        else if(side == false)
        {
            dis = Vector3.Distance(OriginPos.position, PlayerPos.position);
            rot = OriginPos.position - PlayerPos.position;
        }
        //Debug.DrawRay(PlayerPos.position,Vector3.Normalize(OriginPos.position-PlayerPos.position)*dis,Color.red);
        //Debug.DrawRay(PlayerPos.position, Sideposi.position - PlayerPos.position, Color.blue);
        if (Physics.SphereCast(PlayerPos.position, 0.1f, rot, out hit, dis))
        {
            //Debug.Log((hit.collider.gameObject.layer));
            hits = Physics.SphereCastAll(PlayerPos.position, 0.1f, rot, dis);
            
            Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
            float min = dis;
            
            
            for (int i = 0; i < hits.Length; i++)
            {
                switch(hits[i].collider.gameObject.layer)
                {
                    case 8:
                        if (Vector3.Distance(PlayerPos.position, hits[i].point) < min)
                        {
                            min = Vector3.Distance(PlayerPos.position, hits[i].point);
                            hit.point = hits[i].point;
                            ck = true;
                        }
                        break;
                }
            }
        }
        switch(ck)
        {
            case true:
                return true;       
            default:
                return false;
        }
    }
    void CameraMoving()
    {
        if (CameraWallColl())
        {
            float distance;
            if(Vector3.Distance(hit.point,PlayerPos.position)<1.0f)
            {
                distance =1 / 2;
            }
            else
            {
                distance = 4 / 5f;
            }
            Vector3 temp = PlayerPos.position + ((hit.point - PlayerPos.position) * distance);
            transform.position = Vector3.Lerp(transform.position, temp, Time.deltaTime*10);
            transform.localPosition += new Vector3(0, 0, 0.01f);
        }

        else if (side == true)
        {
            transform.position = Vector3.Lerp(transform.position, Sideposi.position, Time.deltaTime * 10);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, OriginPos.position, Time.deltaTime * 10);
        }
    }
    void YRotateCamera()
    {
        Vector3 Height = transform.localPosition;
        Height.y = 1.6f - currXRot / max;
        transform.localPosition = Height;
        vertical = Input.GetAxis("Mouse Y");
        currXRot += vertical;
        currXRot = Mathf.Clamp(currXRot, min, max);
        transform.localRotation = Quaternion.AngleAxis(currXRot, -1 * Vector3.right);

    }

}
