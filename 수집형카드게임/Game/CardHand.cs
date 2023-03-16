using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
    public List<GameObject> Cards;
    public int MaxHand;//최대매수
    public int CurrentHand;//현재 패에 가진 카드 수
    public GameObject HandPoint;
    public Transform myCardLeft;
    public Transform myCardRight;
    public void Init()//패를 기초 세팅하는 함수
    {
        Cards.Clear();
        MaxHand = 9;
        CurrentHand = 0;    
    }
    public void AddCard(GameObject card)//패에 카드를 추가하는 함수
    {
        Cards.Add(card);
        card.gameObject.SetActive(true);
        CurrentHand = Cards.Count;
    }

    public void UseCard(SpellCard card)//카드를 패에서 삭제하는 함수
    {   
            if (Cards.Remove(card.gameObject))
            {
                Destroy(card.gameObject);
                CurrentHand = Cards.Count;
            }
    }
    public void ThrowCard(GameObject card)//카드를 패에서 버리는 함수
    {
        Cards.Remove(card);
        CurrentHand = Cards.Count;
        StartCoroutine(ThrowCoroutine(card));
    }
    IEnumerator ThrowCoroutine(GameObject card)//카드를 패에서 버리는 코루틴
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


