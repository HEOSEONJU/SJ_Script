using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{

    [SerializeField]
    public EnemyMove _Move;
    [SerializeField]
    public EnemyHp _HP;
    [SerializeField]
    public Transform Target;

    public float Current_Delay;
    public float MAX_Delay;
    public float MAX_Dis;
    public float Attack_Range;

    public float Aggro;
    public float MAX_Aggro;
    [SerializeField]
    protected LayerMask Layer = 9;
    public void Update()
    {
        Current_Delay += Time.deltaTime;
    }
    public virtual bool Search_Target()
    {
        if(Target != null)
        {
            return true;
        }
        //색적방법



        return false;
    }
    public virtual void Attack_Function()
    {
        return;
    }

    public virtual  bool Check_Attack()
    {
        
        if (Current_Delay <= MAX_Delay)
        {
            Debug.Log("장전중");
            return false;
        }

        RaycastHit[] hit;
        
        hit = Physics.RaycastAll(transform.position, GameManager.instance.Char_Player_Attack.transform.position - transform.position, 50, Layer);
        if(hit.Length>0 )
        {
            return true;
        }
        
        
        

        return false;
    }

}
