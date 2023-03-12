using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerLine : MonoBehaviour
{
    
    //LineRenderer LR;
    public Vector3 Endpoint;
    public GameObject bullet;
    public Vector3 Myposi;
    
    // Start is called before the first frame update
    void Start()
    {
        Myposi = transform.position;
        GameObject Clone = Instantiate(bullet, Myposi, transform.rotation);
        Clone.GetComponent<RobotBullet>().Endpoint = Endpoint;
        Destroy(gameObject);
        //Destroy(gameObject, 3f);    
        //StartCoroutine(BulletUse());
        //LR.SetWidth(0.5f, 0.5f);
        //LR.SetPosition(0, transform.position);
        //LR.SetPosition(1, Endpoint);
    }

    // Update is called once per frame


    //IEnumerator BulletUse()
    //{

    //    yield return new WaitForSeconds(1.0f);

    //    GameObject Clone = Instantiate(bullet, Myposi, transform.rotation);
    //    Clone.GetComponent<RobotBullet>().Endpoint = Endpoint;


    //    Destroy(gameObject);
    //}
}
