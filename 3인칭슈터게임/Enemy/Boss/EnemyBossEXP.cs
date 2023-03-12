using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossEXP : CommonBullet
{
    public GameObject EffectBoss;
    public Transform LockPosi;
    Transform Pooling;
    public Vector3 Origin;
    public float time;
    // Start is called before the first frame update
    void Awake()
    {
        
        LockPosi = null;
        Origin = transform.position;
        HP = 100;
        diff = 1;
        speed = 1.5f;
        EffectBoss = transform.GetChild(0).gameObject;
        EffectBoss.transform.parent = null;
        EffectBoss.SetActive(false);


        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        HP = 100;
        diff = 1;
        speed = 1.5f;
    }
    // Update is called once per frame
    void Update()
    {
        LockFunction();
        MoveBullet(speed);
    }
    public void FindPooling(Transform Posi)
    {
        EffectBoss.transform.parent = Posi;
        Pooling = Posi;
    }
    public void FireBullet(Transform Posi, Vector3 StartPosi)
    {
        transform.position = StartPosi;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        gameObject.SetActive(true);
        StartCoroutine(LockOn(Posi));
    }
    IEnumerator LockOn(Transform Posi)
    {
        time = 0;
        
        yield return new WaitForSeconds(2.0f);
        speed = 2.0f;
        LockPosi = Posi;
        
        yield return new WaitForSeconds(1.0f);
        
        LockPosi = null;
        
        


    }
    void LockFunction()
    {
        if(LockPosi!=null)
        {
            time+=Time.deltaTime;
            //transform.LookAt(LockPosi);
            



            Quaternion lookOnLook =Quaternion.LookRotation(LockPosi.position - transform.position);

            transform.rotation =Quaternion.Slerp(transform.rotation, lookOnLook, time);

        }

    }
    public void OnTriggerEnter(Collider other)
    {
        LayerMask Ground = 8;
        LayerMask Player = 9;


        if (other.gameObject.layer == Player | other.gameObject.layer == Ground)
        {


            BossSettingBullet();
        }
    }


    public void BossSettingBullet()
    {
        EffectBoss.transform.parent = null;
        EffectBoss.transform.position = transform.position;
        EffectBoss.transform.rotation = Quaternion.identity;
        EffectBoss.SetActive(true);
        EffectBoss.GetComponent<AudioSource>().Play();
        DamgePlayer();
        Invoke("OriginPosi", 2.0f);
        transform.position = Origin;
        gameObject.SetActive(false);
    }
    public void OriginPosi()
    {
        if (Pooling != null)
        {
            EffectBoss.transform.parent = Pooling;
        }
        EffectBoss.transform.position = Origin;
        EffectBoss.SetActive(false);
    }
    public void DamgePlayer()
    {

        RaycastHit[] hitInfos;

        int count = 0;
        Ray ray = new Ray();
        ray.direction = Vector3.up;
        ray.origin = transform.position;
        hitInfos = Physics.SphereCastAll(ray, 5.0f);
        for (int i = 0; i < hitInfos.Length; i++)
        {
            if (hitInfos[i].collider.gameObject.layer == 10)
            {
                if (hitInfos[i].collider.transform.tag != "Head")
                {

                        Base_HP temp = hitInfos[i].transform.GetComponent<Base_HP>();
                    if(temp !=null)
                    {
                        if (temp.Live)
                        {

                            temp.Damged(50);
                        }
                        break;
                    }
                    
                }
            }


            if (hitInfos[i].collider.gameObject.layer == 9)
            {
                count++;
            }
        }
        if (count > 0)
        {
            GameManager.instance.PlayerDamage(100);
        }
    }
}
