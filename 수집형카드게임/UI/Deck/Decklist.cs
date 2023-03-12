using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Linq;

public class Decklist : MonoBehaviour
{
    
    //public CardScriptable CardDataFile;
    
    public int MaxCount = 40;
    //public SelectCard CardNum;
    public SpellCard Main;
    //public List<int> CardHaveNumber;

    public Transform MainCardPosi;
    public GameObject DeckCardObject;
    public GameObject ObjectPrefab;

    public TextMeshProUGUI CountText;
    public TextMeshProUGUI CountSentenceText;

    public DeckEdit First;

    public FailWinodw POPUP_1;

    [SerializeField]
    GameObject Prefab_Object;
    [SerializeField]
    Transform CreateParent;
    private void Awake()
    {

        
        



        for(int i=0;i< CardData.instance.CardDataFile.cards.Count-1;i++)
        {
            
                var e = Instantiate(Prefab_Object, CreateParent);
                e.transform.GetChild(1).GetComponent<DeckEdit>().Init(this, i);
            
        }






        for(int i=0; i<FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count; i++)
        {

            CardCreate(FireBaseDB.instacne.Player_Data_instacne.DeckCards[i]);


        }
        CreateParent.GetChild(0).GetChild(1).GetComponent<DeckEdit>().imageChange();
        //First.imageChange();



    }
    
    public void AddCard()
    {
        int CardOverlapCount = FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count(Element => Element == Main.CardNumber);
        int check = FireBaseDB.instacne.Player_Data_instacne.HaveCard.Count(Element => Element == Main.CardNumber);
        int num=0;

        

        if (MaxCount > FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count & CardOverlapCount <= 2 & check>=1)
        {
            for (int i = 0; i < CardData.instance.CardDataFile.cards.Count; i++)
            {
                if (CardData.instance.CardDataFile.cards[i].id == Main.CardNumber)
                {
                    num = i;
                    break;
                }

            }
            CardCreate(num);

            FireBaseDB.instacne.Player_Data_instacne.DeckCards.Add(Main.CardNumber);
            FireBaseDB.instacne.Player_Data_instacne.HaveCard.Remove(Main.CardNumber);
            FireBaseDB.instacne.Upload_Data(StoreTYPE.HAVE);
            FireBaseDB.instacne.Upload_Data(StoreTYPE.DECK);
            ReloadCount(Main.CardNumber);
            SortingDeckCard();  


        }
        else if(CardOverlapCount>=3)
        {
            POPUP_1.View_Text("덱에 같은 카드가 3장들어있습니다");
        }
        else if(check<=0)
        {
            POPUP_1.View_Text("덱에 넣을 카드가 남아있지 않습니다.");
        }
        else
        {
            POPUP_1.View_Text("덱이 이미 가득찼습니다.");
        }
    }

    public void SubCard()
    {
        bool Active = true;
        if (0 < FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count)
        {
            for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count; i++)
            {
                if(DeckCardObject.transform.GetChild(i).GetChild(1).GetComponent<DeckEdit>().CardNumber== Main.CardNumber)
                {

                    FireBaseDB.instacne.Player_Data_instacne.DeckCards.Remove(Main.CardNumber);
                    FireBaseDB.instacne.Player_Data_instacne.HaveCard.Add(Main.CardNumber);
                    Destroy(DeckCardObject.transform.GetChild(i).gameObject);
                    ReloadCount(Main.CardNumber);
                    Active = false;
                    SortingDeckCard();
                    break;
                }
            }

            if(Active)
            {
                POPUP_1.View_Text("덱에서 뺄 카드가 남아있지 않습니다.");
            }

            
        }
        else
        {
            POPUP_1.View_Text("덱에 카드가 남아있지 않습니다.");
        }
        
    }

    public void CardCreate(int num)
    {
        var e = Instantiate(Prefab_Object, DeckCardObject.transform);
        e.transform.GetChild(1).GetComponent<DeckEdit>().Init(this, num-1);
        //e.transform.GetChild(0).GetComponent<Image>().sprite = CardDataFile.cards[num].Image;
        //e.transform.GetChild(0).GetComponent<DeckEdit>().CardNumber = CardDataFile.cards[num].id;
        //e.transform.GetChild(0).GetComponent<DeckEdit>().number = Main;
        
        //e.transform.GetChild(0).GetComponent<DeckEdit>().DeckInfo = this;
        e.transform.parent = DeckCardObject.transform;


        
        



        SortingDeckCard();
    }
    public void ReloadCount(int num)
    {
        CountText.text = "" + FireBaseDB.instacne.Player_Data_instacne.HaveCard.Count(Element => Element == num) + FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count(Element => Element == num);
    }

    public void SortingDeckCard()
    {
        for (int j = 0; j < DeckCardObject.transform.childCount - 1; j++)
        {
            
            for (int i = 0; i < DeckCardObject.transform.childCount - 1; i++)
            {
                
                if (DeckCardObject.transform.GetChild(i).GetChild(1).GetComponent<DeckEdit>().CardNumber < DeckCardObject.transform.GetChild(i + 1).GetChild(1).GetComponent<DeckEdit>().CardNumber)
                {

                    //DeckCardObject.transform.GetChild(i).SetSiblingIndex(i + 1);



                }


            }
        }

    }

    public void ExitDeckEdit()
    {
        if(FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count>=10)
        {
            GameObject.Find("Main Camera").GetComponent<ManagementMainUI>().CloseDeckUI();
            
        }
            else
        {
            POPUP_1.View_Text("덱에 최소10장의 카드가 있어야힙니다.");
        }
    }

}
