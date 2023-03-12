using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deck : MonoBehaviour
{
    public List<MonsterCard> Monstes;
    public List<SpellCard> SpellCards;
    public List<SpellCard> CardInfo;
    public CardHand Hand;
    [SerializeField]
    Transform CreatePosi;
    public int Count;
    public int MaxCount;
    public Manager manager;
    public GameObject DeckCardObject;
    public List<GameObject> DeckCards;

    //public PlayerInfos Data;

    public CardScriptable CardDataBase;
    int StartNumber;
    [SerializeField]
    List<Material> materials;
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    Transform Center;

    [SerializeField]
    FailWinodw POPUP_1;

    [SerializeField]
    List<ParticleSystem> Init_Effect;
    public void Init()
    {
        StartNumber = 0;

        InitDeck();
        MaxCount = 40;
        







    }
    public void InitDeck()
    {

        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count; i++)
        {
            
            int TempID = FireBaseDB.instacne.Player_Data_instacne.DeckCards[i];

            var temp = Instantiate(DeckCardObject, canvas.transform.position, Quaternion.identity, canvas.transform);
            temp.name = StartNumber + "카드";
            StartNumber++;
            SpellCard card = temp.GetComponent<SpellCard>();
            Init_Card(card, TempID);
            
            temp.transform.Rotate(0, 180, 0);



            temp.transform.localScale = Vector3.one * 0.15f;
            temp.transform.GetChild(1).gameObject.SetActive(false);


            DeckCards.Add(temp);


        }

        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count; i++)//카드이미지입력
        {

            int TempID = DeckCards[i].GetComponent<SpellCard>().CardNumber;

            //Debug.Log(DeckCards[i].transform.GetChild(1).GetChild(1).GetComponent<Image>().name + "백그라운드이미지이름");
            DeckCards[i].transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = CardDataBase.cards[TempID].BG_Image;

            Image temp = DeckCards[i].transform.GetChild(1).GetChild(2).GetComponent<Image>();
            temp.sprite = CardDataBase.cards[TempID].Image;
            temp.material = materials[CardDataBase.cards[TempID].Rank - 1];
            DeckCards[i].transform.GetChild(1).GetChild(8).GetComponent<Image>().enabled = false;
            DeckCards[i].transform.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = CardDataBase.cards[TempID].cost + "";
            DeckCards[i].transform.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = CardDataBase.cards[TempID].CardName + "";
            DeckCards[i].transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text = CardDataBase.cards[TempID].exp + "";

        }

        ShuffleDeck();

    }
    public void Play_Particle( )
    {
        for(int i=0;i<Init_Effect.Count;i++)
        {
            Init_Effect[i].Play();
        }
    }

     IEnumerator Stop_Particle(float time)
    {
        yield return new WaitForSeconds(time*0.1f+1.2f);
        for (int i = 0; i < Init_Effect.Count; i++)
        {
            Init_Effect[i].Stop();
        }
    }
    public void Function_Stop_Particle(float time)
    {
        StartCoroutine(Stop_Particle(time));
    }

    public void Stop_Particle_Invoke(float Time)
    {
        Invoke("StopParticle", Time + 1.3f);
    }
    public void StopParticle()
    {
        for (int i = 0; i < Init_Effect.Count; i++)
        {
            Init_Effect[i].Stop();
        }
    }


    public void Special_InitCard(int ID,int num)
    {

        int TempID = ID;

        var temp = Instantiate(DeckCardObject, Center.position, Quaternion.identity, CreatePosi);
        temp.name = StartNumber + "카드";
        StartNumber++;
        SpellCard card = temp.GetComponent<SpellCard>();
        Init_SpcialCard(card, TempID);

        temp.transform.Rotate(0, 180, 0);



        temp.transform.localScale = Vector3.one * 0.15f;
        temp.transform.GetChild(1).gameObject.SetActive(false);


        DeckCards.Add(temp);


        int INDEX = DeckCards.Count - 1;




        DeckCards[INDEX].transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = CardDataBase.Special_cards[TempID].BG_Image;

        Image temp_image = DeckCards[INDEX].transform.GetChild(1).GetChild(2).GetComponent<Image>();
        temp_image.sprite = CardDataBase.Special_cards[TempID].Image;


        //temp_image.material = materials[CardDataBase.cards[TempID].Rank - 1];

        DeckCards[INDEX].transform.GetChild(1).GetChild(8).GetComponent<Image>().enabled = false;
        DeckCards[INDEX].transform.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = CardDataBase.Special_cards[TempID].cost + "";
        DeckCards[INDEX].transform.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = CardDataBase.Special_cards[TempID].CardName + "";
        DeckCards[INDEX].transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text = CardDataBase.Special_cards[TempID].exp + "";


        //Vector3 enlargePos = new Vector3(Card.originPosi.pos.x, Card.originPosi.pos.y + 1.5f, -10f);
        StartCoroutine(Extra_Move(card, num));



    }
    public void Extra_InitCard(int ID, int num)
    {

        int TempID = ID;

        var temp = Instantiate(DeckCardObject, Center.position, Quaternion.identity, CreatePosi);
        temp.name = StartNumber + "카드";
        StartNumber++;
        SpellCard card = temp.GetComponent<SpellCard>();
        Init_Card(card, TempID);
        
        temp.transform.Rotate(0, 180, 0);



        temp.transform.localScale = Vector3.one * 0.15f;
        temp.transform.GetChild(1).gameObject.SetActive(false);

        
        DeckCards.Add(temp);


        int INDEX = DeckCards.Count - 1;



        
        DeckCards[INDEX].transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite = CardDataBase.cards[TempID].BG_Image;
       
        Image temp_image = DeckCards[INDEX].transform.GetChild(1).GetChild(2).GetComponent<Image>();
        temp_image.sprite = CardDataBase.cards[TempID].Image;


        temp_image.material = materials[CardDataBase.cards[TempID].Rank - 1];

        DeckCards[INDEX].transform.GetChild(1).GetChild(8).GetComponent<Image>().enabled = false;
        DeckCards[INDEX].transform.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = CardDataBase.cards[TempID].cost + "";
        DeckCards[INDEX].transform.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = CardDataBase.cards[TempID].CardName + "";
        DeckCards[INDEX].transform.GetChild(1).GetChild(7).GetComponent<TextMeshProUGUI>().text = CardDataBase.cards[TempID].exp + "";


        //Vector3 enlargePos = new Vector3(Card.originPosi.pos.x, Card.originPosi.pos.y + 1.5f, -10f);
        StartCoroutine(Extra_Move(card, num));
        

    }

    void Init_Card(SpellCard card,int TempID)
    {
        
        card.CardNumber = TempID;
        card.manager = manager;
        card.Cost = CardDataBase.cards[TempID].cost;
        card.CardType = CardDataBase.cards[TempID].CardType;
        /*
        card.SpellType = CardDataBase.cards[TempID].SpellType;
        card.Value_A = CardDataBase.cards[TempID].Value_A;
        card.Value_D = CardDataBase.cards[TempID].Value_D;
        card.Value_H = CardDataBase.cards[TempID].Value_H;
        card.Value_PA = CardDataBase.cards[TempID].Value_PA;
        card.Value_PD = CardDataBase.cards[TempID].Value_PD;
        card.Value_PH = CardDataBase.cards[TempID].Value_PH;
        card.Value_R = CardDataBase.cards[TempID].Value_R;
        card.Value_AC = CardDataBase.cards[TempID].Value_AC;
        card.Value_TY = CardDataBase.cards[TempID].Value_TY;
        card.Value_MR = CardDataBase.cards[TempID].Value_MR;
        card.Value_CP = CardDataBase.cards[TempID].Value_CP;
        card.Value_CD = CardDataBase.cards[TempID].Value_DC;
        card.Value_Cost = CardDataBase.cards[TempID].Value_Cost;
        card.Value_MAXCost = CardDataBase.cards[TempID].Value_MAXCost;
        card.Value_Char_Damage = CardDataBase.cards[TempID].Value_Char_Damage;
        card.Value_Drew = CardDataBase.cards[TempID].Value_Drew;
        card.Value_Hand_Less = CardDataBase.cards[TempID].Value_Hand_Less;
        card.Value_Create_Deck = CardDataBase.cards[TempID].Value_Create_Deck;
        card.Value_Create_Deck1 = CardDataBase.cards[TempID].Value_Create_Deck1;
        card.Value_Create_Deck2 = CardDataBase.cards[TempID].Value_Create_Deck2;
        card.Value_Create_Deck3 = CardDataBase.cards[TempID].Value_Create_Deck3;
        card.Value_Create_Deck4 = CardDataBase.cards[TempID].Value_Create_Deck4;
        card.HEAL = CardDataBase.cards[TempID].HEAL;
        card.Value_Enemy_Damage = CardDataBase.cards[TempID].Value_Enemy_Damage;
        card.Turn = CardDataBase.cards[TempID].Turn;
        card.Special = CardDataBase.cards[TempID].Special;
        */
        card.Value_Enemy_Damage_Effect = CardDataBase.cards[TempID].Value_Enemy_Damage_Effect;
        card.Value_Char_Effect_Num = CardDataBase.cards[TempID].Value_Char_Effect_Num;
        card.Value_Enemy_Effect_Num = CardDataBase.cards[TempID].Value_Enemy_Effect_Num;
        
        

    }
    void Init_SpcialCard(SpellCard card, int TempID)
    {
        card.CardNumber = TempID;
        card.manager = manager;
        card.Cost = CardDataBase.Special_cards[TempID].cost;
        card.CardType = CardDataBase.Special_cards[TempID].CardType;
        card.Value_Enemy_Damage_Effect = CardDataBase.Special_cards[TempID].Value_Enemy_Damage_Effect;
        card.Value_Char_Effect_Num = CardDataBase.Special_cards[TempID].Value_Char_Effect_Num;
        card.Value_Enemy_Effect_Num = CardDataBase.Special_cards[TempID].Value_Enemy_Effect_Num;
        /*card.SpellType = CardDataBase.Special_cards[TempID].SpellType;
        card.Value_A = CardDataBase.Special_cards[TempID].Value_A;
        card.Value_D = CardDataBase.Special_cards[TempID].Value_D;
        card.Value_H = CardDataBase.Special_cards[TempID].Value_H;
        card.Value_PA = CardDataBase.Special_cards[TempID].Value_PA;
        card.Value_PD = CardDataBase.Special_cards[TempID].Value_PD;
        card.Value_PH = CardDataBase.Special_cards[TempID].Value_PH;
        card.Value_R = CardDataBase.Special_cards[TempID].Value_R;
        card.Value_AC = CardDataBase.Special_cards[TempID].Value_AC;
        card.Value_TY = CardDataBase.Special_cards[TempID].Value_TY;
        card.Value_MR = CardDataBase.Special_cards[TempID].Value_MR;
        card.Value_CP = CardDataBase.Special_cards[TempID].Value_CP;
        card.Value_CD = CardDataBase.Special_cards[TempID].Value_DC;
        card.Value_Cost = CardDataBase.Special_cards[TempID].Value_Cost;
        card.Value_MAXCost = CardDataBase.Special_cards[TempID].Value_MAXCost;
        card.Value_Char_Damage = CardDataBase.Special_cards[TempID].Value_Char_Damage;
        card.Value_Drew = CardDataBase.Special_cards[TempID].Value_Drew;
        card.Value_Hand_Less = CardDataBase.Special_cards[TempID].Value_Hand_Less;
        card.Value_Create_Deck = CardDataBase.Special_cards[TempID].Value_Create_Deck;
        card.Value_Create_Deck1 = CardDataBase.Special_cards[TempID].Value_Create_Deck1;
        card.Value_Create_Deck2 = CardDataBase.Special_cards[TempID].Value_Create_Deck2;
        card.Value_Create_Deck3 = CardDataBase.Special_cards[TempID].Value_Create_Deck3;
        card.Value_Create_Deck4 = CardDataBase.Special_cards[TempID].Value_Create_Deck4;
        card.HEAL = CardDataBase.Special_cards[TempID].HEAL;

        
        
        card.Value_Enemy_Damage = CardDataBase.Special_cards[TempID].Value_Enemy_Damage;
        
        card.Turn = CardDataBase.Special_cards[TempID].Turn;
        card.Special = CardDataBase.Special_cards[TempID].Special;
        */

    }
    IEnumerator Extra_Move(SpellCard card, int num)
    {
        card.MoveTransForm(new PRS(card.transform.position, Quaternion.identity, Vector3.zero), false);
        yield return new WaitForSeconds(0.3f);
        float Delay = num * 0.1f;
        
        card.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5;
        yield return new WaitForSeconds(Delay);
        
        card.transform.parent = canvas.transform;
        card.MoveTransForm(new PRS(canvas.transform.position, Quaternion.identity, Vector3.one * 0.15f), true, 0.5f);
        yield return new WaitForSeconds(0.15f);
        card.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
    }


    public bool Drew()
    {
        if (DeckCards.Count > 0 & Hand.CurrentHand < Hand.MaxHand)
        {
            Hand.AddCard(DeckCards[0]);
            DeckCards[0].transform.parent = Hand.transform.GetChild(0);

            DeckCards.Remove(DeckCards[0]);

            Hand.Cards[Hand.CurrentHand - 1].GetComponent<SpellCard>().StartFlip();

            return true;

        }
        else if (Hand.CurrentHand == 10)
        {
            POPUP_1.View_Text("패가 가득찼습니다.");
            //Debug.Log("패가 가득찼습니다.");
            return false;
        }
        else if (DeckCards.Count == 0)
        {
            POPUP_1.View_Text("덱이없습니다");
            //Debug.Log("덱이없습니다");
            return false;
        }
        else
            return false;
    }










    public void ShuffleDeck()
    {
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < DeckCards.Count; i++)
            {
                int rn = Random.Range(Count, DeckCards.Count - 1);
                GameObject temp = DeckCards[i];
                DeckCards[i] = DeckCards[rn];
                DeckCards[rn] = temp;
            }
        }
        int C = DeckCards.Count;
        


    }

    public void Reset_Deck_Hand()
    {

        for (int i = DeckCards.Count - 1; i >= 0; i--)
        {
            Destroy(DeckCards[i].gameObject);

        }
        DeckCards.Clear();
        for (int i = Hand.Cards.Count - 1; i >= 0; i--)
        {
            Destroy(Hand.Cards[i].gameObject);
        }
        Hand.Cards.Clear();
    }

    private void OnDisable()
    {
        Reset_Deck_Hand();
    }



}
