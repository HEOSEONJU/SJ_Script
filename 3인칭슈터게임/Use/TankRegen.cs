using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRegen : MonoBehaviour
{
    public Transform RegenCenter;
    public Transform TankParent;
    public List<Transform> RegenPosi;

    public GameObject TankPrefab;
    public Transform Pooling;
    int count;
    int CurrentIndex;
    void Start()
    {
        RegenPosi= new List<Transform>();
     for (int i = 0; i < RegenCenter.childCount; i++)
        {
            RegenPosi.Add(RegenCenter.GetChild(i));
        }
        StartCoroutine(RegenCorountine());
    }



    IEnumerator RegenCorountine()
    {
        count=TankParent.childCount;

        if (count <= 10)
        {
            int num = Random.Range(0, RegenCenter.childCount);



            var e = Instantiate(TankPrefab, RegenPosi[num].position, Quaternion.identity, TankParent);
            EnemyOldTankMove Temp_e = e.transform.GetComponent<EnemyOldTankMove>();
            Temp_e.FindPooling(Pooling);
            Temp_e.MaxAggro = 100000;
            Temp_e.Aggro = 100000;
            Temp_e.BossArea = true;
            Temp_e.Target =  GameManager.instance.Char_Player_Trace.transform;
            Temp_e.Angle = 179;

            
        }

        yield return new WaitForSeconds(20);
        StartCoroutine(RegenCorountine());
    }


}
