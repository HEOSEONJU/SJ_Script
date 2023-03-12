using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{
    public int Cost;
    public int CardType;
    //public int SpellType;
    //public int HEAL;
    public int Value_Enemy_Damage;
    public int Value_Enemy_Damage_Effect;
    public int Value_Enemy_Effect_Num;
    public int Value_Char_Effect_Num;
    /*
    public int Value_A;
    public int Value_D;
    public int Value_H;
    public int Value_PA;
    public int Value_PD;
    public int Value_PH;
    public int Value_R;
    public int Value_AC;
    public int Value_TY;
    public int Value_MR;
    public int Value_CP;
    public int Value_CD;
    public int Value_DC;
    public int Value_Cost;
    public int Value_MAXCost;
    public int Value_Char_Damage;
    public int Value_Drew;
    public int Value_Hand_Less;
    public int Value_Create_Deck;
    public int Value_Create_Deck1;
    public int Value_Create_Deck2;
    public int Value_Create_Deck3;
    public int Value_Create_Deck4;
    public int Turn;
    */
    public GameObject Front;
    public GameObject Back;
    public bool CardFBCheck;
    public Manager manager;
    int timer;
    public float x, y, z;
    public bool Special;

    //TrailRenderer trailRenderer;

    public List<Order> SummonEffect;
    public void Start()
    {
        InitCard();
        timer = 0;
        if (Front != null & Back != null)
            Front.SetActive(false);
        CardFBCheck = false;
        x = 0;
        y = 1;
        z = 0;





    }


    






    public void Try_Find_Effect()
    {
        SummonEffect = new List<Order>();
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            if (transform.GetChild(1).GetChild(i).TryGetComponent<Order>(out Order Trail))
            {
                SummonEffect.Add(Trail);
                continue;
            }
        }
    }

    public void Change_Effect_Size(float size)
    {
        if (SummonEffect.Count >= 1)
        {
            //Debug.Log("크기변경중");
            for (int i = 0; i < SummonEffect.Count; i++)
            {

                for (int j = 0; j < SummonEffect[i].transform.childCount; j++)
                {

                    SummonEffect[i].transform.GetChild(j).localScale = new Vector3(size, 1, 1);
                }
            }
        }


    }

    public void Stop_Particle()
    {
        if (SummonEffect.Count >= 1)
        {
            //Debug.Log("끄는중");
            for (int i = 0; i < SummonEffect.Count; i++)
            {
                SummonEffect[i].gameObject.SetActive(false);
            }
        }
    }

    public void Reset_Particle()
    {
        if (SummonEffect.Count >= 1)
        {
            //Debug.Log("키는중");
            for (int i = 0; i < SummonEffect.Count; i++)
            {
                SummonEffect[i].gameObject.SetActive(true);
            }
        }
    }

    public void InitCardFunction()
    {

    }
    public void Flip()
    {
        CardFBCheck = !CardFBCheck;
        if (CardFBCheck)
        {
            Front.SetActive(true);
            Back.SetActive(false);
        }
        else
        {
            Front.SetActive(false);
            Back.SetActive(true);
        }

    }

    public void StartFlip()
    {
        StartCoroutine(CardFlip());
    }
    public void BackCardPosi()
    {

        Back.transform.rotation = Quaternion.Euler(new Vector3(x, 180, z));
        Front.transform.rotation = Quaternion.Euler(new Vector3(x, 180, z));

        CardFBCheck = false;

        Front.SetActive(false);
        Back.SetActive(true);


    }


    IEnumerator CardFlip()
    {
        for (int i = 0; i < 60; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Back.transform.Rotate(new Vector3(x, y * 3, z));
            Front.transform.Rotate(new Vector3(x, y * 3, z));

            timer++;
            if (timer == 30 || timer == -30)
            {
                Flip();

            }
        }
        timer = 0;
    }



    private void OnMouseOver()
    {
        if (CardFBCheck)
        {

            manager.CardMouseOver(this);
        }
    }
    private void OnMouseExit()
    {
        if (CardFBCheck)
        {
            manager.CardMouseExit(this);
        }
    }

    private void OnMouseDown()
    {
        if (CardFBCheck)
        {
            manager.CardMouseDown();
        }
    }
    private void OnMouseUp()
    {
        if (CardFBCheck)
        {
            manager.CardMouseUp();
        }
    }



}
/*
    public void Skill_Apply(CharSkillData temp)
    {

        Cost = 0;
        Value_Cost=temp.Value_Cost;
        Value_MAXCost=temp.Value_MAXCost;
        Value_Char_Damage=temp.Value_Char_Damage;
        Value_Drew=temp.Value_Drew;
        Value_Hand_Less=temp.Value_Hand_Less;
        Value_Create_Deck=temp.Value_Create_Deck;
        Value_Create_Deck1 = temp.Value_Create_Deck1;
        Value_Create_Deck2 = temp.Value_Create_Deck2;
        Value_Create_Deck3 = temp.Value_Create_Deck3;
        Value_Create_Deck4 = temp.Value_Create_Deck4;
        Value_Enemy_Effect_Num=temp.Value_Enemy_Effect_Num;
        Value_Char_Effect_Num=temp.Value_Char_Effect_Num;

    }
    */