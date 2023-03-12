using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class CardGetUI : MonoBehaviour
{

    public Camera Maincam;
    public CardScriptable CardDataBase;
    public CharCardScriptTable CharDataBase;
    public Camera Subcam;
    public Camera SubcamChar;
    public GameObject Card;
    public GameObject Monster;
    public GameObject Result;
    public Transform LightPosi;
    public TextMeshProUGUI PackText;
    public Image FadeImage;
    public RandomScene RanFade;
    public RandomScene CharanFade;
    public FailWinodw POPUP_1;
    
    Vector3 a;
    Vector3 b;
    bool check;
    
    //public TMP_InputField CostValue;
    
    [Header("스펠카드등급별 나누기")]
    public List<int> CardGet1;
    public List<int> CardGet2;
    public List<int> CardGet3;


    [Header("캐릭터카드등급별 나누기")]
    public List<int> CharGet1;
    public List<int> CharGet2;
    public List<int> CharGet3;

    private void Awake()
    {
        

        
        FireBaseDB.instacne.Player_Data_instacne.GetPack = new List<int>();

        check = true;

        CardGet1 = new List<int>();
        CardGet2 = new List<int>();
        CardGet3 = new List<int>();

        CharGet1 = new List<int>();
        CharGet2 = new List<int>();
        CharGet3 = new List<int>();

        foreach(SpellCardData SCD in CardData.instance.CardDataFile.cards)
        {
            switch(SCD.Rank) 
            {
                case 1:
                    CardGet1.Add(SCD.id);
                    break;
                case 2:
                    CardGet2.Add(SCD.id);
                    break;
                case 3:
                    CardGet3.Add(SCD.id);
                    break;
                default:
                    break;

            }
        }

        foreach (CharCardData CCD in CardData.instance.CharDataFile.Monster)
        {
            switch (CCD.Rank)
            {
                case 1:
                    CharGet1.Add(CCD.id);
                    break;
                case 2:
                    CharGet2.Add(CCD.id);
                    break;
                case 3:
                    CharGet3.Add(CCD.id);
                    break;
                default:
                    break;

            }
        }

        a = new Vector3(-0.8888889f, -4.481482f, 90);
        b = new Vector3(-0.05740738f, -4.481482f, 90);
    }



    private void OnEnable()
    {
        PackText.text = "" + FireBaseDB.instacne.Player_Data_instacne.SpellCardPack;
    }
    public void ResetPackText()
    {
        PackText.text = "" + FireBaseDB.instacne.Player_Data_instacne.SpellCardPack;
    }



    public void UseCardPack_Char(int n)//n은 뽑은횟수
    {
        if (FireBaseDB.instacne.Player_Data_instacne.SpellCardPack < n)
        {
            POPUP_1.View_Text("티켓이 모자랍니다");
            return;
        }
        int Gold;
        FireBaseDB.instacne.Player_Data_instacne.GetPack.Clear();
        for (int i = 0; i < n; i++)
        {
            int num = Random_Result(UnityEngine.Random.Range(1, 100), true, out Gold);//뽑기결과를 인덱스와Gold 출력
            
            MonsterInfo T = FireBaseDB.instacne.Player_Data_instacne.MonsterCards.Find(x => x.ID == num);//보유하고있는지 찾기
            if (T != null)//null이 아니라는것은 보유중
            {
                Check_Char_Limit(T, Gold);//중복으로 한계돌파 한계돌파이상이면 랭크에 따른 골드증가
            }
            else//새로운 캐릭터 획득 보유리스트에 추가
            {
                MonsterInfo temp = new MonsterInfo();
                temp.Init_Monset(num);
                FireBaseDB.instacne.Player_Data_instacne.MonsterCards.Add(temp);
            }
            FireBaseDB.instacne.Player_Data_instacne.GetPack.Add(num);//뽑은기록 저장
            FireBaseDB.instacne.Player_Data_instacne.SpellCardPack -= 1;//뽑기티켓소모
        }
        ResetPackText();//뽑기티켓갯수갱신
        FireBaseDB.instacne.Player_Data_instacne.UsePack = n;//사용한뽑기횟수

        FireBaseDB.instacne.Upload_Data(StoreTYPE.PACK);
        FireBaseDB.instacne.Upload_Data(StoreTYPE.CHAR);
        FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
        //Data.Saved_Data();//결과를저장
        StartCoroutine(FadeScene(0.5f));//뽑기화면으로이동
    }
    public void UseCardPack_Spell(int n)//n은 뽑은횟수
    {
        if (FireBaseDB.instacne.Player_Data_instacne.SpellCardPack < n)
        {
            POPUP_1.View_Text("티켓이 모자랍니다");
            return;
        }
        int Gold;
        FireBaseDB.instacne.Player_Data_instacne.GetPack.Clear();
        for (int i = 0; i < n; i++)
        {
            int num=Random_Result(UnityEngine.Random.Range(1, 100), false, out Gold);//뽑기결과를 인덱스와Gold 출력

            if ((FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count(Element => Element == num) + FireBaseDB.instacne.Player_Data_instacne.HaveCard.Count(Element => Element == num))<3) //중복카드보유가 3장미만이라면 보유카드에 추가
            {
                FireBaseDB.instacne.Player_Data_instacne.HaveCard.Add(num);
            }
            else//3장이상인경우 골드로 치환해서 추가
            {
                FireBaseDB.instacne.Player_Data_instacne.Gold += Gold;
            }
            FireBaseDB.instacne.Player_Data_instacne.GetPack.Add(num);//뽑은기록 저장
            FireBaseDB.instacne.Player_Data_instacne.SpellCardPack -= 1;//뽑기티켓 소모
        }
        ResetPackText();//뽑기티켓갯수갱신
        FireBaseDB.instacne.Player_Data_instacne.UsePack = n;//사용한뽑기횟수
        FireBaseDB.instacne.Upload_Data(StoreTYPE.PACK);
        FireBaseDB.instacne.Upload_Data(StoreTYPE.HAVE);
        FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
        StartCoroutine(FadeScene(0.5f));//뽑기화면으로이동   
    }

    IEnumerator FadeScene(float time)
    {
        Color color = FadeImage.color;
        color.a = 0.0f;
        FadeImage.color = color;
        while (color.a <= 1.0)
        {
            color = FadeImage.color;
            color.a += Time.deltaTime * 2;
            FadeImage.color = color;
            yield return new WaitForSeconds(Time.deltaTime / 2);
        }


        Maincam.gameObject.SetActive(false);
        
        if (check == true)
        {
            SubcamChar.gameObject.SetActive(true);
            CharanFade.OnEnableScene();
        }
        else
        {
            Subcam.gameObject.SetActive(true);
            RanFade.OnEnableScene();
        }
        

    }
    public void BackScene()
    {
        StartCoroutine(Fadeout(0.5f));
    }
    IEnumerator Fadeout(float time)
    {
        Color color = FadeImage.color;
        color.a = 1.0f;
        FadeImage.color = color;
        while (color.a > 0)
        {
            color = FadeImage.color;
            color.a -= Time.deltaTime * 2;
            FadeImage.color = color;
            yield return new WaitForSeconds(Time.deltaTime / 2);
        }


    }
    public void SwapGetItem()//뽑기화면 전환 캐릭터<=>스펠
    {
        check = !check;
        if (check)
        {
            Card.SetActive(false);
            Monster.SetActive(true);
            LightPosi.position = a;
        }
        else
        {
            Card.SetActive(true);
            Monster.SetActive(false);
            LightPosi.position = b;
        }

    }



    public bool Check_Char_Limit(MonsterInfo T,int Gold)
    {
        if (T.BreaK_Lim <= 4)
        {
            T.BreaK_Lim++;
            return true;
        }
        else
        {
            FireBaseDB.instacne.Player_Data_instacne.Gold += Gold;
            return false;
        }
    }


    public int Random_Result(int NUM,bool Char,out int Gold)//True면 캐릭터 false면 스펠
    {
        switch(Char)
        {
            case true:
                if (NUM >= 90)
                {
                    Gold = 1000;
                    return  CharGet3[UnityEngine.Random.Range(0, CharGet3.Count)];
                }
                else if (NUM >= 50)
                {
                    Gold = 100;
                    return CharGet2[UnityEngine.Random.Range(0, CharGet2.Count)];
                }
                else
                {
                    Gold = 10;
                    return CharGet1[UnityEngine.Random.Range(0, CharGet1.Count)];
                }
                
            case false:
                if (NUM >= 90)
                {
                    Gold = 1000;
                    return  CardGet3[UnityEngine.Random.Range(0, CardGet3.Count)];
                }
                else if (NUM >= 50)
                {
                    Gold = 100;
                    return  CardGet2[UnityEngine.Random.Range(0, CardGet2.Count)];
                }
                else
                {
                    Gold = 10;
                    return CardGet1[UnityEngine.Random.Range(0, CardGet1.Count)];
                }
        }
    }
}
