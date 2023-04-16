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

    public void Button_Login()//�������� �α����Լ� �۵�
    {
        if(!State)
        Login(ID.text, PW.text);
    }
    void Login(string INPUT_ID, string INPUT_PW)
    {
        State = true;
        FBM.ES_ADD("�α��νõ���...");
        Delay_Trigger = false;
        FBM.auth.SignInWithEmailAndPasswordAsync(INPUT_ID, INPUT_PW).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                FBM.ES_ADD("�α��νõ�  ���");
                State =false;
                StopCoroutine(LoginCoroutine());
                return;
            }
            if (task.IsFaulted)
            {
                FBM.ES_ADD("�α��νõ�  ����");
                State = false;
                StopCoroutine(LoginCoroutine());
                return;
            }
            if (task.IsCompleted)
            {
                
                FBM.user = FBM.auth.CurrentUser;
                
                FBM.ES_ADD("�α��� ����");
                
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
        CC.Move_Char();//ĳ���ͼ���â���� �̵�
        yield break;
    }
}
