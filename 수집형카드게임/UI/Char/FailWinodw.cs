using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FailWinodw : MonoBehaviour
{
    [SerializeField]
     TextMeshProUGUI Window_Text;
    
    [SerializeField]
    Image temp_Image;
    [SerializeField]
    Image temp_Image2;
    float FadeTime;
    bool Active;

    
    void OnEnable()
    {
        FadeTime = -0.1f;
        Active = false;
        Change_Alpha(FadeTime);



    }
    private void OnDisable()
    {
        StopCoroutine(Corountine_Window());
    }

    void Change_Alpha(float value)
    {

        Color Image_Color = temp_Image.color;
        Image_Color.a = value;
        temp_Image.color = Image_Color;

        Color Image_Color2 = temp_Image2.color;
        Image_Color2.a = value;
        temp_Image2.color = Image_Color2;


        Color Text_Color = Window_Text.color;
        Text_Color.a = value;
        Window_Text.color = Text_Color;
        
    }


    public void View_Text(string text)
    {


        Window_Text.text = text;

        if(Active)
        {
            FadeTime = 2.0f;
        }
        else
        {
            StartCoroutine(Corountine_Window());
        }
    }
    IEnumerator Corountine_Window()
    {
        FadeTime = 2.0f;
        Active=true;
        while(FadeTime >= 0)
        {
            FadeTime -= Time.deltaTime;
            Change_Alpha(FadeTime);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Active = false ;
    }
}
