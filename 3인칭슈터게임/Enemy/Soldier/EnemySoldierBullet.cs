using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldierBullet : CommonBullet
{
    
    // Start is called before the first frame update
    void Start()
    {
        HP = 20;
        diff = 3f;
        speed = 4; 
        
        
        //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
    }
    public void BulletAction()
    {
        StartCoroutine(SetOffActive());
    }
    // Update is called once per frame
    void Update()
    {
        MoveBullet(speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.PlayerDamage(50);

            SettingBullet();
        }
        else if (other.gameObject.layer == 8)
        {
            SettingBullet();
        }

        
    }
    IEnumerator SetOffActive()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
    }

}
