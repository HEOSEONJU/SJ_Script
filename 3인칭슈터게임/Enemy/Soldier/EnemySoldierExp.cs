using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierExp : MonoBehaviour
{
    ParticleSystem my;

    private void Start()
    {
        transform.parent = null;
    }
    public void DoSetoff()
    {
        my = GetComponent<ParticleSystem>();
        StartCoroutine(SetOff());
    }
    IEnumerator SetOff()
    {
        yield return new WaitForSeconds(1.0f);
        my.Stop();
        gameObject.SetActive(false);
    }
}
