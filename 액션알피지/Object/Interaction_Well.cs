using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Well : Interaction_Function
{
    
    [SerializeField]
    float Time=180;
    [SerializeField]
    MeshRenderer WaterMesh;
    public override void Function(bool State)
    {
        if(!WaterMesh.enabled || State==false)
        {
            return;
        }
        if (Game_Master.instance.PM._status.Heal(int.MaxValue))
        {
            
            Game_Master.instance.PM._connect_Object.Make_IF_NULL();
            
            StartCoroutine(Idle_Time());
            WaterMesh.enabled = false;
        }
    }
    IEnumerator Idle_Time()
    {

        yield return new WaitForSeconds(Time);
        
        WaterMesh.enabled = true;

    }
    
}
