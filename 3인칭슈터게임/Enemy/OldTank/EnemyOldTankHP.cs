using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyOldTankHP : EnemyHp
{
    // Start is called before the first frame update
    
    
    public ParticleSystem DieEff;
    public Animation Ani;
    public Transform turret;
    public EnemyOldTankMove Move;
    public AudioSource DieSound;
    public void Awake()
    {
        Live = true;
        hp = 400;
        Ani = transform.GetChild(0).transform.GetChild(3).GetComponent<Animation>();
        DieEff = transform.GetChild(0).transform.GetChild(4).GetComponent<ParticleSystem>();
        Move=GetComponent<EnemyOldTankMove>();
        
    }
    void Start()
    {
        
        InstID();
    }
    public override void Damged(int Da,bool special=false)
    {
        hp -= Da;
        Move.SetTargeting(GameManager.instance.Char_Player_Trace.transform);
        Move.LockOnTarget();
        if (hp <= 0 & Live == true)
        {

            Live = false;
            StartCoroutine(DieEnemy());
        }
        
    }
    IEnumerator DieEnemy()
    {
        //turret.SetParent(null);
        
        Rigidbody R;
        R = GetComponent<Rigidbody>();
        R.isKinematic = true;
        R.useGravity = false;
        R.velocity = Vector3.zero;
        GetComponent<NavMeshAgent>().enabled= false;

        
        Ani.Play("Cannon");
        DieEff.Play();
        DieSound.Play();

        DelID();

        GameManager.instance.Score += 20;
        
        //Vector3 posi = GameManager.instance.Char_Player_Trace.transform.position-transform.position;
        //ParticleSystem Clone = Instantiate(DieEff, posi/3, Quaternion.identity);

        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
