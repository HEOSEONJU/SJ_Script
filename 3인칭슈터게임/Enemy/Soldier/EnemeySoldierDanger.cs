using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeySoldierDanger : MonoBehaviour
{
    public EnemySoldierMove Danger;
    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Danger.SetTargeting(other.transform);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Danger.ReMoveTargeting();
        }
    }
}

