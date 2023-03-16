using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyOldTankHP : EnemyHp
{
    // Start is called before the first frame update
    [SerializeField]
    Old_Tank_Manager _Manager;
    public ParticleSystem DieEff;
    public Animation Ani;
    
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
        _Manager.Targeting(GameManager.instance.Char_Player_Trace.transform);
        
        
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
        _Manager._Animator.SetTrigger("Death");
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
