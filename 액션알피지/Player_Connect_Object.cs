using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Player_Connect_Object : MonoBehaviour
{

    [SerializeField]
    LinkedList<Interaction_Function> Object_List;
    [SerializeField]
    Interaction_Function Connect_IF;

    public bool IF
    {
        get
        {
            if (Connect_IF == null)
            {
                return false;
            }
            return true;
        }
    }
    [SerializeField]
    Collider Object_Detect_Collider;
    Transform CharPosi;




    public void Init()
    {
        

        Object_List = new LinkedList<Interaction_Function>();

        CharPosi = Game_Master.instance.PM.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            Interaction_Function temp = other.GetComponent<Interaction_Function>();
            Object_List.AddLast(temp);
            temp.Connect_Object(Game_Master.instance.PM);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            Interaction_Function temp = other.GetComponent<Interaction_Function>();
            if (temp == null)
            {
                return;
            }
            Object_List.Remove(temp);


            if (Connect_IF != null)
            {
                foreach (Interaction_Function func in Object_List)//아직 주변에 연결된NPC가있을때
                {

                    if (Connect_IF.Object_ID == func.Object_ID)//주변NPC 연결이 해제되었을때 아직연결된NPC와 거리가 일정범위내라면 해제하지않고 리턴
                    {
                        temp.DisConnect_Object();//NPC에 매니저할당해제
                        return;
                    }
                }


            }
            DisConnect_Object();
            temp.DisConnect_Object();//NPC에 매니저할당해제
        }
    }

    public void Connect_IF_Function()//플레이어 입력으로 주변 IF와 접촉시도
    {
        if (Object_List.Count == 0)//접촉할 오브젝트 없음 혹은 이미 접촉중
        {
            return;
        }
        else if (IF)
        {
            Connect_IF.Function(false);
            Make_IF_NULL();
                
            return;
        }

        Connect_IF = Return_Close_IF();
        Connect_IF.Function(IF);

    }

    public void DisConnect_Object()// 접속상태 해제
    {
        if (Connect_IF == null)
        {
            return;
        }
        Game_Master.instance.UI.Close_All_UI();
        Connect_IF = null;


    }

    public void Make_IF_NULL()
    {
        Connect_IF = null;
    }
    public Interaction_Function Return_Close_IF()
    {
        if (Object_List.Count == 1)
        {
            return Object_List.First();
        }
        Interaction_Function TEMP = null;
        float Value = float.MaxValue;

        foreach (Interaction_Function func in Object_List)
        {
            Debug.Log(func.name +Vector3.Distance(CharPosi.position, func.transform.position));
            float distance = Vector3.Distance(CharPosi.position, func.transform.position);
            if (Value > distance)
            {
                Value = distance;
                TEMP = func;
            }
        }
        return TEMP;
        //Object_List.Sort((Interaction_Function x, Interaction_Function y) => x.Distance.CompareTo(y.Distance));
    }
}
