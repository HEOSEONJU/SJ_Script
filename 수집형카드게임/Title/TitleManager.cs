using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    
    // Start is called before the first frame update




    [Header("�Է��ʵ�")]
    [SerializeField]
    TMP_InputField _INPUT_ID;
    [SerializeField]
    TMP_InputField _INPUT_PASSWORD;
    [SerializeField]
    TMP_InputField _INPUT_GAME_NAME;
    [SerializeField]
    TMP_InputField _INPUT_LOGINID;
    [SerializeField]
    TMP_InputField _INPUT_LOGINPASSWORD;

    string INPUT_ID;
    string INPUT_PASSWORD;
    string INPUT_GAME_NAME;


    string LogIn_ID;
    string LogIn_Password;


    

    [Header("������Ʈ")]
    [SerializeField]
    GameObject Compelete;
    [SerializeField]
    GameObject LogIn_POPUP;
    [SerializeField]
    GameObject Create_ID_POPUP;
    [SerializeField]
    GameObject Loading;
    [SerializeField]
    Slider Gage_Value;

    FailWinodw POPUP_1;
    [SerializeField]
    GameObject POPUP;

    private void OnEnable()
    {
        POPUP.SetActive(true);
        Compelete.SetActive(false); 
        LogIn_POPUP.SetActive(false);
        Create_ID_POPUP.SetActive(false);
        Loading.SetActive(false);
        POPUP_1=GetComponent<FailWinodw>();
    }


    public void Init_Accoutnt()
    {
        FireBaseDB.instacne.Create_ID(_INPUT_ID.text, _INPUT_PASSWORD.text, _INPUT_GAME_NAME.text);
        
    }


    public void Duplication_Text()
    {
        POPUP_1.View_Text("�̹� �ִ� ���̵��Դϴ�");
    }
    public void Empty_Text()
    {
        POPUP_1.View_Text("���̵� Ȥ�� ��й�ȣ�� �����Դϴ�.");
    }
    public void Init_Text()
    {
        POPUP_1.View_Text("���̵� ������ �����Ͽ����ϴ�.");
        Create_ID_POPUP.SetActive(false);
        FireBaseDB.instacne.Player_Data_instacne.Init_Account(_INPUT_ID.text, _INPUT_PASSWORD.text, _INPUT_GAME_NAME.text);

    }





    public void Try_LogiN()
    {
        LogIn_ID = _INPUT_LOGINID.text;
        LogIn_Password = _INPUT_LOGINPASSWORD.text;

        FireBaseDB.instacne.Login(LogIn_ID, LogIn_Password);



    }
    public void Sucess_Login()
    {
        LogIn_POPUP.SetActive(false);
        Compelete.SetActive(true);
        POPUP_1.View_Text("�α��ο� ���� �Ͽ����ϴ�.");
    }
    public void Faile_Text()
    {
        POPUP_1.View_Text("���̵� Ȥ�� ��й�ȣ�� Ʋ�ǽ��ϴ�.");
    }

    public void OpenCreate()
    {
        Create_ID_POPUP.SetActive (true);
    }
    public void CloseCreate()
    {
        _INPUT_GAME_NAME.text = "";
        _INPUT_ID.text = "";
        _INPUT_PASSWORD.text = "";
        Create_ID_POPUP.SetActive(false);;
    }

    public void OpenLOGIN()
    {
        LogIn_POPUP.SetActive(true);
    }
    public void CloseLOGIN()
    {
        _INPUT_LOGINID.text = "";
        _INPUT_LOGINPASSWORD.text = "";

        LogIn_POPUP.SetActive(false); ;
    }

    public void CloseCreate_OpneLOGIN()
    {
        CloseCreate();
        OpenLOGIN();

    }
    public void QuitGame()
    {
        Application.Quit();
    }


    public void MoveGame()
    {
        POPUP.SetActive(false);
        Loading.SetActive(true);
        
        StartCoroutine(SceneLoading());
        //SceneManager.LoadScene("MainScene");
    }
    IEnumerator SceneLoading()
    {

        
        Gage_Value.value = 0;
        Debug.Log("�ε�����");
        TextMeshProUGUI LP=Gage_Value.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        LP.text = "Loading... 0%";

        // ���� Ÿ�̸ӷ� ������ ���� ����
        float dummyTime = Random.Range(0.8f, 1.5f);

        // �������� ǥ���Ǵ� loading �� ������
        float loadingTime = 0.0f;   // �ð� ��� ��
        float progress = 0.0f;      // ������ ��


        // Ÿ�̸� ������ ó�� 
        while (loadingTime <= dummyTime)
        {
            InfiniteLoopDetector.Run();
            //Debug.Log(loadingTime);
            // ������ �� �ð��� ���� 
            loadingTime += Time.deltaTime;

            // AsyncOperation �� ���� �߰� �ε� ó���� ���� 0.9 �� �� �����ȭ 
            // ���� opertaion.progress �� 0.9 ��ġ ó�� �ǰ� �Ϸ� �ȴ�.
            progress = Mathf.Clamp01(loadingTime / (0.9f + dummyTime));

            // �����̴����� �� ���� ó��
            Gage_Value.value = progress;
            LP.text = "Loading... " + Gage_Value.value * 100 + "%";
            yield return null;
        }
        yield return null;
        // "AsyncOperation"��� "�񵿱����� ������ ���� �ڷ�ƾ�� ����"
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");
        operation.allowSceneActivation = true;


        // �ε� �� ��ŸƮ ��ư ó���� ���� ����
        // �� �׸��� ������ �ٷ� �ε� �� Scene �̵��� ó�� ��


        // �ε��� ����Ǳ� �������� �ε�â ������ ó��
        while (!operation.isDone)
        {
            LP.text = "Loading... " + Gage_Value.value * 100 + "%";
            // �񵿱� �ε� ���࿡ ���� ������ ó��
            progress = Mathf.Clamp01((operation.progress + loadingTime) / (0.9f + dummyTime));
            // �����̴� ���� ó��
            Gage_Value.value = progress;
            yield return null;
        }
        yield break;












        //AsyncOperation Load = SceneManager.LoadSceneAsync("Main Scene");
        //while (!Load.isDone)
        //{
        //    Gage_Value.value = Load.progress;
        //    Debug.Log(Load.progress);
        //    LP.text = "Loading... " + Gage_Value.value * 100 + "%";
        //    yield return null;
        //}
    }


}
