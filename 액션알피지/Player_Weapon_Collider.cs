using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Weapon_Collider : MonoBehaviour
{
    public Collider _Collider;

    

    


    public void AbleCollider(bool able)
    {
        if(_Collider == null)
        {
           _Collider = GetComponent<Collider>();
            _Collider.isTrigger = true;
        }
        _Collider.enabled = able;


    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy_Hit_Box"))
        {
            if (Game_Master.instance.PM._isAttacking)
            {
                Enemy_Base_Status temp = other.GetComponentInParent<Enemy_Base_Status>();
                if (Game_Master.instance.PM._enemyIDList.Count >= 1)
                {
                    foreach (int id in Game_Master.instance.PM._enemyIDList)
                    {
                        if (id == temp.ID)
                        {                   
                            return;
                        }
                    }
                }
                Game_Master.instance.PM.Enemy_Damaged(temp);
                //Debug.Log(other.name);
            }
        }
    }
}
