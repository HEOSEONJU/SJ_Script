using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireBase_Login : MonoBehaviour
{
    [SerializeField]
    FireBase_Manager FBM;
    bool Delay_Trigger = false;
    [SerializeField]
    TMP_InputField ID;
    [SerializeField]
    TMP_InputField PW;
    [SerializeField]
    Change_Char CC;
    [SerializeField]
    TextMeshProUGUI TEXT;

    [SerializeField]
    Toggle Remember;

    [SerializeField]
    Remember_Value RV;


    bool State;
    private void Awake()
    {
        State = false;
        if(RV.ID_Value!= null) 
        {
            
            ID.text = RV.ID_Value;
            PW.text = RV.PW_Value;
        }
        
    }

    public void Button_Login()//버툰으로 로그인함수 작동
    {
        if(!State)
        Login(ID.text, PW.text);
    }
    void Login(string INPUT_ID, string INPUT_PW)
    {
        State = true;
        FBM.ES_ADD("로그인시도중...");
        Delay_Trigger = false;
        FBM.auth.SignInWithEmailAndPasswordAsync(INPUT_ID, INPUT_PW).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                FBM.ES_ADD("로그인시도  취소");
                State =false;
                StopCoroutine(LoginCoroutine());
                return;
            }
            if (task.IsFaulted)
            {
                FBM.ES_ADD("로그인시도  실패");
                State = false;
                StopCoroutine(LoginCoroutine());
                return;
            }
            if (task.IsCompleted)
            {
                
                FBM.user = FBM.auth.CurrentUser;
                
                FBM.ES_ADD("로그인 성공");
                
                if (Remember.isOn)
                {
                    RV.Remember_Member_Data(INPUT_ID, INPUT_PW);
                }
                else
                {
                    RV.Remember_Member_Data();
                }
                Delay_Trigger = true;
            }
        });
        StartCoroutine(LoginCoroutine());
    }
    IEnumerator LoginCoroutine()
    {
        while (!Delay_Trigger)
        {
            yield return null;

        }
        CC.Move_Char();//캐릭터선택창으로 이동
        yield break;
    }
}
