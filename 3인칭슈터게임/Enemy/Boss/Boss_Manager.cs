using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Boss_Manager : Enemy_Manager
{
    

    private void Start()
    {
        Target = GameManager.instance.Char_Player_Trace.transform;


    }

    private void FixedUpdate()
    {
        _Move.Cal_Dis();
    }

    public override bool Search_Target()
    {
        return true;
    }
    public override void Attack_Function()
    {
        return;
    }
    public override bool Check_Attack()
    {
        if (Current_Delay <= MAX_Delay)
        {
            Debug.Log("ÀåÀüÁß");
            return false;
        }

        RaycastHit[] hit;

        hit = Physics.SphereCastAll(transform.position, Attack_Range, Vector3.up, 1.0f, Layer);
        if (hit.Length > 0)
        {
            return true;
        }




        return false;
    }

}
