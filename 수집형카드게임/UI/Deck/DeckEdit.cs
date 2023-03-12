using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;
using System.Linq;

public class DeckEdit : MonoBehaviour
{
    public Decklist DeckInfo;
    
    public int CardNumber;//자기 카드번호
    public SpellCard number;
    [SerializeField]
    List<Material> materials;
    int num;

    public void Init(Decklist DeckInfos,int num)
    {
        DeckInfo = DeckInfos;
        CardNumber = num + 1;
        GetComponent<Image>().enabled=false;
        
        transform.parent.GetChild(0).GetComponent<Image>().sprite = CardData.instance.CardDataFile.cards[CardNumber].BG_Image;
        




    }
    public void imageChange()
    {
        
        
        DeckInfo.CountText.text = "" + FireBaseDB.instacne.Player_Data_instacne.HaveCard.Count(Element => Element == CardNumber)+ FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count(Element => Element == CardNumber);//다시확인해보기
        DeckInfo.CountSentenceText.text = "Have";


        DeckInfo.MainCardPosi.GetChild(0).GetComponent<Image>().sprite = CardData.instance.CardDataFile.cards[CardNumber].BG_Image;
        Image Temp_Image = DeckInfo.MainCardPosi.GetChild(1).GetComponent<Image>();
        Temp_Image.sprite = CardData.instance.CardDataFile.cards[CardNumber].Image;
        Temp_Image.material = materials[CardData.instance.CardDataFile.cards[CardNumber].Rank - 1];


        DeckInfo.MainCardPosi.GetChild(4).GetComponent<TextMeshProUGUI>().text = "" + CardData.instance.CardDataFile.cards[CardNumber].cost;
        DeckInfo.MainCardPosi.GetChild(5).GetComponent<TextMeshProUGUI>().text = CardData.instance.CardDataFile.cards[CardNumber].CardName;
        DeckInfo.MainCardPosi.GetChild(6).GetComponent<TextMeshProUGUI>().text = CardData.instance.CardDataFile.cards[CardNumber].exp;
        DeckInfo.MainCardPosi.GetChild(7).GetComponent<Image>().enabled = false;

        DeckInfo.Main.CardNumber = CardNumber;
        






    }
}
