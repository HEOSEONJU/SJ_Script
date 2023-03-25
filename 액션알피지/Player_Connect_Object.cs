using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player_Connect_Object : MonoBehaviour
{
    Player_Manager _manager;
    [SerializeField]
    List<Interaction_Function> Object_List;
    [SerializeField]
    Interaction_Function Connect_IF;
    Collider Object_Detect_Collider;
    Transform CharPosi;

    bool Connecting=false;



    public bool Connecting_Check()
    {
        return Connecting;
    }

    public void Init(Player_Manager Posi)
    {
        Object_Detect_Collider= GetComponent<Collider>();
        
        Object_List=new List<Interaction_Function>();
        _manager = Posi;
        CharPosi = Posi.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Object"))
        {
            Interaction_Function temp = other.GetComponent<Interaction_Function>();
            Object_List.Add(temp);
            temp.Connect_Object(_manager);
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            Interaction_Function temp = other.GetComponent<Interaction_Function>();
            if(temp==null)
            {
                return;
            }
            Object_List.Remove(temp);

            foreach(Interaction_Function func in Object_List)//아직 주변에 연결된NPC가있을때
            {

                if (Connect_IF.Object_ID == func.Object_ID)//주변NPC 연결이 해제되었을때 아직연결된NPC와 거리가 일정범위내라면 해제하지않고 리턴
                {
                    temp.DisConnect_Object();//NPC에 매니저할당해제
                    return;
                }
            }
            
            DisConnect_Object();
            temp.DisConnect_Object();//NPC에 매니저할당해제









        }
    }

    public bool Connect()
    {
        if (Object_List.Count >= 1)
        {
            Sorting_Distance();
            return true;
        }

        return false;

    }


    public void Connect_IF_Function()
    {
        Connect_IF = Object_List.First();
        Connecting = true;
        Connect_IF.Open_Window();

    }
    public void Close_NPC()
    {
        _manager._Manager_Inventory.Open_Close_Inventory_Window(false);
        Connecting = false;
    }
    public void DisConnect_Object()//내상태에서 NPC관련해제
    {
        if(Connect_IF == null)
        {
            return;
        }
        _manager._Manager_Inventory.Open_Close_Inventory_Window(false);
        Connect_IF.Close_Window();
        Connect_IF = null;
        Connecting = false;
    }

    public void Sorting_Distance()
    {
        if(Object_List.Count>=2)
        {
            foreach(Interaction_Function func in Object_List)
            {
                func.Distance = Vector3.Distance(CharPosi.position, func.transform.position);
            }
            Object_List.Sort((Interaction_Function x, Interaction_Function y) => x.Distance.CompareTo(y.Distance));
        }
    }
}
