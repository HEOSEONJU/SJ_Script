using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar_Weapon : MonoBehaviour
{
    [SerializeField]
    Collider WeaponCollider;

    Boar_Enemy Manager;
    private void Start()
    {
        WeaponCollider = GetComponent<Collider>();
        WeaponCollider.enabled = false;
        Manager = GetComponentInParent<Boar_Enemy>();
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
        Debug.Log(other.CompareTag("Player_Hit_Box"));

        if (other.CompareTag("Player_Hit_Box"))
        {
            Manager.Player_to_Damaged(other);
        }
    }
}
