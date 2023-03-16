using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmallCharList : MonoBehaviour
{
    public int MYID;
    

    
    public void Setting(int i)
    {
        MYID = i;
    }
    public void ResetState()//가지고있
    {
        if(FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(MYID)==null)//보유하지않고있다면 잠금 이펙트 활성화
        {
            transform.GetChild(2).gameObject.SetActive(true);
            return;
        }
        transform.GetChild(2).gameObject.SetActive(false);
        


    }


}
