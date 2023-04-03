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

    public void Button_Create()//버툰으로 아이디생성함수 작동
    {
        CreateUser(ID.text, PW.text);
    }
    void CreateUser(string ID, string Password)//아이디생성
    {

        FBM.auth.CreateUserWithEmailAndPasswordAsync(ID, Password).ContinueWith(
           task =>
           {
               if (!task.IsCanceled && !task.IsFaulted)
               {
                   FBM.ES_ADD("회원 가입 완료");
               }
               else
               {
                   FBM.ES_ADD("회원 가입 실패 이미 존재하는 아이디");
               }
           });

    }
}
