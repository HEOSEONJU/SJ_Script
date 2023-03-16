using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Third_Gun : Base_Gun
{
    [SerializeField]
    BoomGun BoomGunObject;

    public override void Init(Gun_Manager main, Animator _ani)
    {
        Main = main;
        _animator = _ani;
        
    }

    public override void Shoot()
    {

        if (Main.Shooting == false)
        {
           if(Input.GetMouseButton(0) && Delay <= 0)
            {
                Main.SpineAction.SetTrigger("HandGunShot");
                Main.PIN = false;

                Delay = CurrentDelay;
                BoomGunObject.Shoot(cam.transform,layer);
            }
        }
    }
    public override void Reload_Function()
    {
        
        return;
    }
    
    public override void Reload_After_Function()
    {

        return;
    }

    // Update is called once per frame


}
