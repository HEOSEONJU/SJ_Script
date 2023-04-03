using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FireBase_Create_ID : MonoBehaviour
{
    [SerializeField]
    FireBase_Manager FBM;
    
    [SerializeField]
    TMP_InputField ID;
    [SerializeField]
    TMP_InputField PW;

    public void Button_Create()//�������� ���̵�����Լ� �۵�
    {
        CreateUser(ID.text, PW.text);
    }
    void CreateUser(string ID, string Password)//���̵����
    {

        FBM.auth.CreateUserWithEmailAndPasswordAsync(ID, Password).ContinueWith(
           task =>
           {
               if (!task.IsCanceled && !task.IsFaulted)
               {
                   FBM.ES_ADD("ȸ�� ���� �Ϸ�");
               }
               else
               {
                   FBM.ES_ADD("ȸ�� ���� ���� �̹� �����ϴ� ���̵�");
               }
           });

    }
}
