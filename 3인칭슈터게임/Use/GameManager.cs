using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    

    //적이 탐지하는플레이어와 플레이어를조준하기위한지점
    public GameObject Char_Player_Trace;
    public GameObject Char_Player_Attack;
    public Transform Cam_Player_Position;
    //필드의적
    //public GameObject[] Char_Enemy;
    public List<int>EnemyID=new List<int>();
    //
    public Player_Manager PlayerMove;
    public GameUIManager GameUI_Manager;
    EnemyBossMove BossScript;

    public GameObject EnemyPoolingObject;

    
    //플레이어스테이터스
    public int Hp;
    public float oldHp;
    public int MAXHp;



    public bool Reloading;//장전애니메이션중
    public bool Action;


    //적의수
    public int EnemyCount;

    public Renderer DamageRender;
    public float range;
    Color color;
    public Text HpText;
    public Image HpImage;
    public float Hit;
    public float Heal;

    public float Score;
    public bool Boss;
    // Start is called before the first frame update
    void Awake()
    {
        
        instance = this;
        MAXHp = Hp = 1000;


        Score = 10000;
        Boss = false;
        Heal = Hit = 0;
        EnemyCount = 0;
        oldHp = Hp;
        color = DamageRender.material.GetColor("_RimColor");

    

    }
   

    private void Update()
    {
        
        if (range>=0)
        {
            range -= Time.deltaTime;
        }
        color.a = range;
        DamageRender.material.SetColor("_RimColor", color);
        if(Boss)
        {
            Score-=Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.RightAlt))
        {
            //PlayerDamage(1500);
        }
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            //BossScript.GetComponent<EnemyBossHP>().Damged(3000);
        }
    }


    public void PlayerDamage(int a)
    {
        Hit = 5.0f;
        oldHp = Hp;
        Hp -= a;
        range = 1.5f;
        if(Hp < 0 & PlayerMove.enabled)
        {
            PlayerDefeat();
        }
    }
    void PlayerDefeat()
    {
        PlayerMove.CharAni.SetTrigger("Die");
        PlayerMove.LegAni.SetTrigger("Die");
        if (Boss)
        {
            BossScript.BossStop();
        }
        PlayerMove.enabled = false;
        PlayerMove.transform.GetComponent<Gun>().enabled = false;
        EnemyPoolingObject.SetActive(false);
        PlayerPrefs.SetFloat("SCORE", Score - 10000);
        PlayerPrefs.SetInt("Result", 0);
    }

    public void PlayerVictory()
    {
        PlayerMove.ResetAllAnimation();
        PlayerMove.enabled = false;
        PlayerMove.transform.GetComponent<Gun>().enabled = false;
        EnemyPoolingObject.SetActive(false);
        
        PlayerPrefs.SetFloat("SCORE", Score);
        PlayerPrefs.SetInt("Result", 1);
        GameUI_Manager.FadeOut();


    }
    public bool InstID(int id)
    {
        bool c = true;
        for (int i = 0; i < EnemyID.Count; i++)
        {
            if (EnemyID[i] == id)
            {
                c = false;
                break;
            }
        }
        EnemyID.Add(id);
        return c;
    }
    public void InitBoss(EnemyBossMove temp)
    {
        Boss = true;
        BossScript = temp;
    }



    
}
