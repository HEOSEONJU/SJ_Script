using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Weapon : MonoBehaviour
{
    [SerializeField]
    Collider WeaponCollider;

    [SerializeField]
    Knight_Enemy Manager;
    private void Start()
    {
        
        WeaponCollider.enabled = false;
        
    }

    public void WeaponColliderEnable()
    {
        WeaponCollider.enabled = true;
    }
    public void WeaponColliderDisable()
    {
        WeaponCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player_Hit_Box"))
        {
            Manager.Player_to_Damaged(other);
        }
    }
}
