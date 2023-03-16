using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemySoldierHP : EnemyHp
{
    // Start is called before the first frame update
    [SerializeField]
    EnemySoldier_Manager _Manager;
    Animator animator;
    //public Animation Ani;
    
    public Collider[] col;
    public EnemySoldierMove Move;
    public EnemySoldierMove[] squad;
    AudioSource audio;

    public void Awake()
    {
        Live = true;
        hp = 100;
        animator = GetComponent<Animator>();
        audio=GetComponent<AudioSource>();
            
        Move = GetComponent<EnemySoldierMove>();
        
    }
    void Start()
    {
        InstID();
        
    }
    
    public override void Damged(int Da, bool special = false)
    {
        _Manager.Targeting(GameManager.instance.Char_Player_Trace.transform);

        Debug.Log(Da);
        

        StartCoroutine(CallSquad());
        
        hp -= Da;
        
        if (hp <= 0 & Live == true)
        {

            Live = false;
            StartCoroutine(DieEnemy());
        }
    }
    IEnumerator DieEnemy()
    {
        for(int i = 0; i < col.Length; i++)
        {
            col[i].enabled = false;
        }
        _Manager.DesBullet();
        DelID();
        GameManager.instance.Score += 10;
        animator.SetTrigger("Death");
        audio.Play();
        yield return new WaitForSeconds(1.3f);
        Destroy(gameObject);
    }
    IEnumerator CallSquad()
    {
        EnemySoldierMove[] Squad=squad;        
        for(int i=0;i<squad.Length;i++) //각각 squad멤버와 플레이어간의 거리 할당
        {
            Squad[i].SquadDis = Vector3.Distance(GameManager.instance.Char_Player_Trace.transform.position, Squad[i].Myposi);
        }
        Array.Sort(Squad, (EnemySoldierMove x,EnemySoldierMove y)=>x.SquadDis.CompareTo(y.SquadDis));//가까운순으로 정렬

        foreach (EnemySoldierMove move in Squad)
        {
            
            if (_Manager != null & _Manager.Target ==null)
            {
                _Manager.Targeting(GameManager.instance.Char_Player_Trace.transform);
                 
                 yield return new WaitForSeconds(1.0f);
                
            }

        }
        yield return null;

    }
}
