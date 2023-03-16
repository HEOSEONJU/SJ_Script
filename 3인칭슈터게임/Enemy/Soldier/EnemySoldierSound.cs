using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierSound : MonoBehaviour
{
    public AudioSource footStep;
    public AudioSource RunStep;

    public void ActiveStepSound()
    {
        footStep.Play();
    }
    public void ActiveRunSound()
    {
        RunStep.Play();
    }
}
