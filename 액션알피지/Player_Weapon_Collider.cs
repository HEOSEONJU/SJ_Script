using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Weapon_Collider : MonoBehaviour
{
    public Collider _Collider;

    Player_Manager _manager;

    private void Start()
    {
        _manager = GetComponentInParent<Player_Manager>();
    }


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
            if (_manager.Attacking)
            {
                Enemy_Base_Status temp = other.GetComponentInParent<Enemy_Base_Status>();
                if (_manager.Enemy_IDList.Count >= 1)
                {
                    foreach (int id in _manager.Enemy_IDList)
                    {
                        if (id == temp.ID)
                        {                   
                            return;
                        }
                    }
                }
                _manager.Enemy_Damaged(temp);
                //Debug.Log(other.name);
            }
        }
    }
}
