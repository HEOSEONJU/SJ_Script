using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
    public List<GameObject> Cards;
    public int MaxHand;//�ִ�ż�
    public int CurrentHand;//���� �п� ���� ī�� ��
    public GameObject HandPoint;
    public Transform myCardLeft;
    public Transform myCardRight;
    public void Init()//�и� ���� �����ϴ� �Լ�
    {
        Cards.Clear();
        MaxHand = 9;
        CurrentHand = 0;    
    }
    public void AddCard(GameObject card)//�п� ī�带 �߰��ϴ� �Լ�
    {
        Cards.Add(card);
        card.gameObject.SetActive(true);
        CurrentHand = Cards.Count;
    }

    public void UseCard(SpellCard card)//ī�带 �п��� �����ϴ� �Լ�
    {   
            if (Cards.Remove(card.gameObject))
            {
                Destroy(card.gameObject);
                CurrentHand = Cards.Count;
            }
    }
    public void ThrowCard(GameObject card)//ī�带 �п��� ������ �Լ�
    {
        Cards.Remove(card);
        CurrentHand = Cards.Count;
        StartCoroutine(ThrowCoroutine(card));
    }
    IEnumerator ThrowCoroutine(GameObject card)//ī�带 �п��� ������ �ڷ�ƾ
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


