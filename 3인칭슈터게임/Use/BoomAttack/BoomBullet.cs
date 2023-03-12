using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBullet : MonoBehaviour
{
    public Vector3 Posi;
    bool Active;
    
    
    public Vector3 start;
    public Vector3 end;
    public float speed = 0.5f;
    public float elapsed = 0;
    public GameObject Boom;
    MeshRenderer mr;
    private void Start()
    {
        mr= GetComponent<MeshRenderer>();
        Posi= transform.position;
        Active = false;
        Boom = transform.GetChild(0).gameObject;
        Boom.SetActive(false);
        Boom.transform.parent = null;
    }
    void Update()
    {
        if(Active)
        {
            elapsed += Time.deltaTime / speed;
            
            transform.position = Vector3.Lerp(start, end, elapsed);
            if (elapsed >= 1)
            {
                Active = false;
                elapsed = 0.0f;
                transform.position = Posi;
                
            }
        }
    }


    public void ActiveBullet(Transform Position)
    {
        mr.enabled = true;
        transform.position = Posi;
        Boom.transform.position = Posi;
        Boom.SetActive(false);
        Active =true;
        

        end = Position.position;
        start = transform.position;

        Vector3 dir =  (Position.position - transform.position).normalized;



        end = Position.position +   dir * 10;

        transform.LookAt(Position);
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }


    void BoomFire()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Boom.transform.position = transform.position;
        Boom.transform.rotation = Quaternion.Euler(-90, 0, 0);
        Boom.SetActive (true);
        mr.enabled = false;

        
    }
    private void OnTriggerEnter(Collider other)
    {

        

        if(other.gameObject.layer == 8)
        {
            BoomFire();


        }
        else if (other.gameObject.layer == 10 )
        {
            BoomFire();


        }
    }
}
