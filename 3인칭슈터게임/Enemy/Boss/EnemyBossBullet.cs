using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossBullet : CommonBullet
{
    public GameObject EffectBoss;
    public Vector3 Origin;
    // Start is called before the first frame update
    void Awake()
    {
        Origin = transform.position;
        HP = 1;
        diff = 1;
        speed = 2.0f;
        EffectBoss =transform.GetChild(0).gameObject;
        EffectBoss.transform.parent = null;
        EffectBoss.SetActive(false);
        

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet(speed);
    }
    public void FindPooling(Transform Posi)
    {
        EffectBoss.transform.parent = Posi;
    }
    public void FireBullet(Transform Posi,Vector3 StartPosi)
    {
        transform.position = StartPosi;
        transform.LookAt(Posi);
        gameObject.SetActive(true);
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
        EffectBoss.transform.position = transform.position;
        EffectBoss.transform.rotation = Quaternion.identity;
        EffectBoss.SetActive(true);
        DamgePlayer();












        Invoke("OriginPosi", 1.0f);
        transform.position = Origin;
        gameObject.SetActive(false);
    }
    public void OriginPosi()
    {
        EffectBoss.transform.position= Origin;
        EffectBoss.SetActive(false);
    }
    public void DamgePlayer()
    {

        RaycastHit[] hitInfos;

        int count = 0;
        Ray ray = new Ray();
        ray.direction = Vector3.up;
        ray.origin = transform.position;
        hitInfos = Physics.SphereCastAll(ray, 0.5f);
        for (int i = 0; i < hitInfos.Length; i++)
        {



            if (hitInfos[i].collider.gameObject.layer == 9)
            {
                count++;
            }
        }
        if (count > 0)
        {
            GameManager.instance.PlayerDamage(50);
        }
    }
}
