using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomScene : MonoBehaviour
{


    public Camera Maincam;
    public Camera Subcam;
    
    public Image FadeImage;
    public CardGetUI Fade;
    public UIDataUpdate UIData;

    public void MoveScene()
    {
        StartCoroutine(FadeScene(0.5f));
    }
    public void OnEnableScene()
    {
        StartCoroutine(Fadeout(0.5f));
    }

    IEnumerator Fadeout(float time)
    {
        Color color = FadeImage.color;
        color.a = 1.0f;
        FadeImage.color=color;
        while (color.a>0)
        {
            color = FadeImage.color;
            color.a -= Time.deltaTime*2;
            FadeImage.color = color;
            yield return new WaitForSeconds(Time.deltaTime/2);
        }
        


        

        
        


    }
    IEnumerator FadeScene(float time)
    {
        Color color = FadeImage.color;
        color.a = 0.0f;
        FadeImage.color=color;
        while (color.a <= 1)
        {
            color = FadeImage.color;
            color.a += Time.deltaTime*2;
            FadeImage.color = color;
            yield return new WaitForSeconds(Time.deltaTime/2);
        }
        

        Maincam.gameObject.SetActive(true);
        UIData.UpdateData();
        
        Subcam.gameObject.SetActive(false);
        
        Fade.BackScene();
    }
}
