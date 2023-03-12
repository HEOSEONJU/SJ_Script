using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldTankBullet : CommonBullet
{
   
    void OnEnable()
    {
        
        HP = 30.0f;
        diff = 3f;
        speed = 2.0f;
        Effect.transform.parent = transform;
    }
    void Update()
    {
        MoveBullet(speed);
    }
    public void OnTriggerEnter(Collider other)
    {
        LayerMask Ground = 8;
        LayerMask Player = 9;
        LayerMask Enemy = 10;

        if (other.gameObject.layer ==Player| other.gameObject.layer == Ground | other.gameObject.layer == Enemy)
        {
            //Debug.Log(other.gameObject.name);
            //Debug.Log(other.gameObject.layer);
            Effect.transform.parent = null;
            SettingBullet();

            
        }

        
    }
    


}

