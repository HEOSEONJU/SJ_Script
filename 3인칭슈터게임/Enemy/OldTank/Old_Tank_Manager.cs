using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_Tank_Manager : Enemy_Manager
{

    [SerializeField]
    bool Boss_Area;

    [SerializeField]
    public Animator _Animator;


    [SerializeField]
    float Angle;
    float Search_Dis;

    [SerializeField]
    GameObject Cannon;
    [SerializeField]
    GameObject Tank_Head;
    public void Update()
    {
        if (!_HP.Live)
        {
            
            return;
        }
        

        Current_Delay += Time.deltaTime;

        if (_Move.Distance() >= MAX_Dis)
        {
            Aggro -= Time.deltaTime;
        }
        if(Target !=null)
        {
            Cannon.transform.LookAt(GameManager.instance.Char_Player_Attack.transform);
            Tank_Head.transform.LookAt(GameManager.instance.Char_Player_Attack.transform);
        }
        /*
            TankSearch();
            TankOrderMove();

            if (Aggro <= 0 & Target != null)
            {
                ReMoveTargeting();
            }

            if (Delay <= AttackDelay)
            {
                Delay += Time.deltaTime;
            }
            TankMoveSoundControl();

            
        }
         
         */

    }
    private void FixedUpdate()
    {
        _Move.Cal_Dis();
    }


    public override void Attack_Function()
    {

        _Move.Attack_Order();

    }
    public override bool Search_Target()
    {
        if(Boss_Area) 
        {
            Target=GameManager.instance.Char_Player_Trace.transform;
            MAX_Aggro = Mathf.Infinity;
            Aggro=Mathf.Infinity;
        }
        if(Target!=null)
        {
            return true;
        }


        if(_Move.RobotSearchPlayer(transform.position, transform.forward, Angle, GameManager.instance.Char_Player_Trace.transform.position, Search_Dis))
        {
            Targeting(GameManager.instance.Char_Player_Trace.transform);
            Target = GameManager.instance.Char_Player_Trace.transform;
            return true;
        }


        
        return false;





    }
    

    public void Targeting(Transform Temp)
    {
        if (Aggro <= 0)
        {
            Aggro = MAX_Aggro;
        }
        Target = Temp;



    }


}
