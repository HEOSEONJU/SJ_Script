using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallCharList : MonoBehaviour
{
    public int MYID;
    List<MonsterInfo> Monster;

    
    public void Setting()
    {
        Monster = FireBaseDB.instacne.Player_Data_instacne.MonsterCards;
    }
    public void ResetState()
    {
        
        bool ck = false;
        for (int i = 0; i < Monster.Count; i++)
        {

            if (Monster[i].ID == MYID)
            {
                ck = true;
                //Debug.Log("보유ID" + Monster[i].ID + "/ 자기아이디" + MYID);
            }
        }

        if (ck == true)
        {
            transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }

    }


}
