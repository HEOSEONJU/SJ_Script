using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player_Connect_NPC : MonoBehaviour
{
    Player_Manager _manager;
    [SerializeField]
    List<Base_NPC> NPC_List;
    Base_NPC Connect_NPC_Object;
    Collider NPC_Detect_Collider;
    Transform CharPosi;

    bool Connecting=false;



    public bool Connecting_Check()
    {
        return Connecting;
    }

    public void Init(Player_Manager Posi)
    {
        NPC_Detect_Collider= GetComponent<Collider>();
        NPC_List =new List<Base_NPC>();
        _manager = Posi;
        CharPosi = Posi.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("NPC"))
        {
            Base_NPC temp = other.GetComponent<Base_NPC>();
            NPC_List.Add(temp);
            temp.Connecting_NPC(_manager);
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Base_NPC temp= other.GetComponent<Base_NPC>();
            if(temp==null)
            {
                return;
            }
            NPC_List.Remove(temp);

            for (int i = 0; i < NPC_List.Count; i++)//아직 주변에 연결된NPC가있을때
            {
                if (temp.Object_ID == NPC_List[i].Object_ID)//주변NPC 연결이 해제되었을때 아직연결된NPC와 거리가 일정범위내라면 해제하지않고 리턴
                {
                    temp.DisConnecting_NPC();//NPC에 매니저할당해제
                    return;
                }
                    
            }
            DisConnect_NPC();
            temp.DisConnecting_NPC();//NPC에 매니저할당해제









        }
    }

    public bool Connect()
    {
        if (NPC_List.Count >= 1)
        {
            Sorting_Distance();
            return true;
        }

        return false;

    }


    public void Connect_NPC()
    {
        Connect_NPC_Object = NPC_List[0];
        Connecting = true;
        Connect_NPC_Object.Open_Window();


    }
    public void Close_NPC()
    {
        _manager._Manager_Inventory.Open_Close_Inventory_Window(false);
        Connecting = false;
    }
    public void DisConnect_NPC()
    {
        if(Connect_NPC_Object==null)
        {
            return;
        }
        _manager._Manager_Inventory.Open_Close_Inventory_Window(false);
        Connect_NPC_Object.Close_Window();
        Connect_NPC_Object = null;
        Connecting = false;
    }

    public void Sorting_Distance()
    {
        if(NPC_List.Count>=2)
        {
            for(int i=0;i< NPC_List.Count;i++)//각 NPC간의 거리 재계산
            {
                NPC_List[i].distance = Vector3.Distance(CharPosi.position, NPC_List[i].transform.position);
            }


            NPC_List.Sort((Base_NPC x, Base_NPC y) => x.distance.CompareTo(y.distance));
        }
    }
}
