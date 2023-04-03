using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player_Connect_Object : MonoBehaviour
{
    
    [SerializeField]
    List<Interaction_Function> Object_List;
    [SerializeField]
    Interaction_Function Connect_IF;

    public bool IF
    {
        get
        {
            if(Connect_IF==null)
            {
                return false;
            }
            return true;
        }
    }

    Collider Object_Detect_Collider;
    Transform CharPosi;

    


    public void Init()
    {
        Object_Detect_Collider= GetComponent<Collider>();
        
        Object_List=new List<Interaction_Function>();
        
        CharPosi = Game_Master.instance.PM.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Object"))
        {
            Interaction_Function temp = other.GetComponent<Interaction_Function>();
            Object_List.Add(temp);
            temp.Connect_Object(Game_Master.instance.PM);
            
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


            if(Connect_IF!=null)
            {
                foreach (Interaction_Function func in Object_List)//���� �ֺ��� �����NPC��������
                {

                    if (Connect_IF.Object_ID == func.Object_ID)//�ֺ�NPC ������ �����Ǿ����� ���������NPC�� �Ÿ��� ������������� ���������ʰ� ����
                    {
                        temp.DisConnect_Object();//NPC�� �Ŵ����Ҵ�����
                        return;
                    }
                }

                
            }
            DisConnect_Object();
            temp.DisConnect_Object();//NPC�� �Ŵ����Ҵ�����










        }
    }



    public void Connect_IF_Function()
    {
        if(Object_List.Count==0)//������ ������Ʈ ����
        {
            return;
        }

        Sorting_Distance();
        Connect_IF = Object_List.First();
        Connect_IF.Function(IF);

    }

    public void DisConnect_Object()//�����¿��� NPC��������
    {
        if(Connect_IF == null)
        {
            return;
        }
        Game_Master.instance.PM._Manager_Inventory.Open_Close_Inventory_Window(false);
        
        Connect_IF.Function(false);
        Make_IF_NULL();

    }

    public void Make_IF_NULL()
    {
        Connect_IF = null;
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
