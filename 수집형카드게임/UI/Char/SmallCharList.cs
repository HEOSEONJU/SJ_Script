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
    public void ResetState()//��������
    {
        if(FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(MYID)==null)//���������ʰ��ִٸ� ��� ����Ʈ Ȱ��ȭ
        {
            transform.GetChild(2).gameObject.SetActive(true);
            return;
        }
        transform.GetChild(2).gameObject.SetActive(false);
        


    }


}
