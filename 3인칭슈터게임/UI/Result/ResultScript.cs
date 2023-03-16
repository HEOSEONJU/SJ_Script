using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScript : MonoBehaviour
{
    int Result;
    

    public GameObject Victory;
    public GameObject Defeat;
    public Image Fade;
    public GameObject Score;
    public Text ScoreBoard;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Result = PlayerPrefs.GetInt("Result");
        float tempScore = PlayerPrefs.GetFloat("SCORE");
        tempScore=Mathf.Floor(tempScore);
        ScoreBoard.text =""+ tempScore;
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        Color color = Fade.color;
        while(color.a>0)
        {
            color = Fade.color;
            color.a-=Time.deltaTime;
            Fade.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        switch(PlayerPrefs.GetInt("Result"))
        {
            case 1:

                Victory.SetActive(true);
                break;
                
            default:
                Defeat.SetActive(true);
                
                break;

        }

    }

    public void ViewScore()
    {
        Score.GetComponent<Animation>().Play();
    }


}
