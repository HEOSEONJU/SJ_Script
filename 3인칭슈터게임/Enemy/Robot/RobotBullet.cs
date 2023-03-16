using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBullet : MonoBehaviour
{
    public Vector3 Endpoint;
    public bool st;
    
    void Start()
    {
        st = true;
        transform.LookAt(GameManager.instance.Char_Player_Trace.transform);
        Destroy(gameObject,5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (st)
        {
            transform.position = Vector3.Lerp(transform.position, transform.forward, Time.deltaTime * 0.2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            GameManager.instance.Hp -= 10;
            Destroy(gameObject);
        }
    }
    //IEnumerator stgo()
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    st=true;
    //}
}
