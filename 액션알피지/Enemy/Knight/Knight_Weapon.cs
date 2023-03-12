using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Weapon : MonoBehaviour
{
    [SerializeField]
    Collider WeaponCollider;

    Knight_Enemy Manager;
    private void Start()
    {
        WeaponCollider = GetComponent<Collider>();
        WeaponCollider.enabled = false;
        Manager =GetComponentInParent<Knight_Enemy>();
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
        if (other.gameObject.layer == 8)
        {
            Manager.Player_to_Damaged(other);
        }
    }
}
