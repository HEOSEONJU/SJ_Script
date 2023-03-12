using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRegen : MonoBehaviour
{
    public Transform RegenCenter;
    //public Transform AmmoParent;
    public List<Transform> RegenPosi;

    public GameObject GunAmmoPrefab;
    public GameObject BoomAmmoPrefab;
    int count;
    int CurrnetIndex;
    void Start()
    {
        RegenPosi = new List<Transform>();
        for (int i = 0; i < RegenCenter.childCount; i++)
        {
            RegenPosi.Add(RegenCenter.GetChild(i));
        }
        CurrnetIndex = 0;
        StartCoroutine(RegenCorountine());
        
    }



    IEnumerator RegenCorountine()
    {
        CurrnetIndex = Random.Range(0, 4);
        count = 0;
        for(int i = 0; i < RegenPosi.Count; i++)
        {
            count+=RegenPosi[i].childCount;
        }

        if (count <= 3)
        {
            if (RegenPosi[CurrnetIndex].childCount>=1)
            {
                int temp_count = 0;
                for(int i=0;i< RegenCenter.childCount; i++)
                {
                    temp_count++;
                    CurrnetIndex++;
                    if (CurrnetIndex >= RegenCenter.childCount)
                    {
                        CurrnetIndex = 0;
                    }
                    if (RegenPosi[CurrnetIndex].childCount == 0)
                        break;

                }
                if(temp_count >= RegenCenter.childCount)
                {
                    yield return new WaitForSeconds(5);
                    StartCoroutine(RegenCorountine());
                    yield break;
                }
            }
            int random = Random.Range(0, 10);
            GameObject tempObject=GunAmmoPrefab;
            Vector3 SpwanPois = RegenPosi[CurrnetIndex].position;

            if (random>=7)
            {
                tempObject = BoomAmmoPrefab;
                SpwanPois = new Vector3(SpwanPois.x, SpwanPois.y+0.813f, SpwanPois.z);
            }


            var e = Instantiate(tempObject, SpwanPois, Quaternion.identity, RegenPosi[CurrnetIndex]);
            CurrnetIndex++;
            if(CurrnetIndex>=4)
            {
                CurrnetIndex = 0;
            }

            yield return new WaitForSeconds(5);
        }

        yield return new WaitForSeconds(10);
        StartCoroutine(RegenCorountine());
    }


    
}
