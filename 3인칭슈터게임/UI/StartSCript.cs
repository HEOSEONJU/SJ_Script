using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartSCript : MonoBehaviour
{
    public float Progress;
    public Slider Loadbar;

    public Camera Main;
    public Camera Loading;
    public GameObject Load;

    public void StartButton()
    {

        Main.enabled = false;
        Loading.enabled = true;
        Load.SetActive(true);
        //SceneManager.LoadScene("MainScene");
        StartCoroutine(LoadScene("MainScene"));
        
    }
    public void EXITButton()
    {
        Application.Quit();
    }

    IEnumerator LoadScene(string SceneName)
    {

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync(SceneName);
        while(!asyncOper.isDone)
        {
            yield return null;
            //Loadbar.value = asyncOper.progress;


        }

    }
}

