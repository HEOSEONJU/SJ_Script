using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
    public List<GameObject> Cards;
    public int MaxHand;
    public int CurrentHand;
    public GameObject HandPoint;

    public Transform myCardLeft;
    public Transform myCardRight;
    



    public void Init()
    {
        
        Cards.Clear();
        MaxHand = 9;
        CurrentHand = 0;    
    }
    public void AddCard(GameObject card)
    {
        Cards.Add(card);
        
        card.gameObject.SetActive(true);

        CurrentHand = Cards.Count;

    }

    public void UseCard(SpellCard card)
    {
        
            if (Cards.Remove(card.gameObject))
            {
                Destroy(card.gameObject);
                CurrentHand = Cards.Count;
            }
        
        
        
        //card.gameObject.SetActive(false);

    }
    public void ThrowCard(GameObject card)
    {
        Cards.Remove(card);
        CurrentHand = Cards.Count;
        StartCoroutine(ThrowCoroutine(card));
        
    }
    IEnumerator ThrowCoroutine(GameObject card)
    {
        
        card.GetComponent<PolygonCollider2D>().enabled = false;
        float time = 0;
        card.transform.rotation = Quaternion.identity;
        while (time <=0.3f)
        {
            time += Time.deltaTime;
            card.transform.position=new Vector3(card.transform.position.x,card.transform.position.y-0.15f,card.transform.position.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(card);
        


    }

}



//public void SortingSpellLayer()
//{
//    int n = 4;
//    Transform Posi;
//    for (int i = 0; i < Cards.Count; i++)
//    {
//        //Posi = Cards[i].transform.GetChild(1);
//        //카드배경
//        //Posi.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1 + n * i;
//        //카드이미지
//        //Posi.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 2 + n * i;
//        //카드프레임
//        //Posi.GetChild(2).GetComponent<SpriteRenderer>().sortingOrder = 3 + n * i;
//        //카드코스트
//        //Posi.GetChild(3).GetComponent<SpriteRenderer>().sortingOrder = 4 + n * i;

//    }

//}
//public void SortingMonserLayer()
//{
//    int n = 6;
//    Transform Posi;
//    for(int i=0; i<Cards.Count;i++)
//    {
//        Posi = Cards[i].transform.GetChild(1);
//        //카드틀
//        Posi.GetComponent<SpriteRenderer>().sortingOrder = 2 + n * i;
//        //카드배경
//        Posi.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1 + n * i;
//        //카드이미지
//        Posi.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = 2 + n * i;
//        //카드 레어표시
//        Posi.GetChild(2).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 3 + n * i;
//        //스킬틀
//        Posi.GetChild(3).GetComponent<SpriteRenderer>().sortingOrder = 4 + n * i;
//        Posi.GetChild(4).GetComponent<SpriteRenderer>().sortingOrder = 4 + n * i;
//        Posi.GetChild(5).GetComponent<SpriteRenderer>().sortingOrder = 4 + n * i;
//        //스킬아이콘
//        Posi.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5 + n * i;
//        Posi.GetChild(4).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5 + n * i;
//        Posi.GetChild(5).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 5 + n * i;
//        //공수버툰
//        Posi.GetChild(6).GetComponent<SpriteRenderer>().sortingOrder= 5 + n * i;
//        Posi.GetChild(7).GetComponent<SpriteRenderer>().sortingOrder = 5 + n * i;

//        //공수텍스트
//        Posi.GetChild(6).GetChild(0).GetComponent<Canvas>().sortingOrder = 6 + n * i;
//        Posi.GetChild(7).GetChild(0).GetComponent<Canvas>().sortingOrder = 6 + n * i;
//        //이름텍스트
//        Posi.GetChild(8).GetComponent<Canvas>().sortingOrder = 6 + n * i;


//    }

//}
