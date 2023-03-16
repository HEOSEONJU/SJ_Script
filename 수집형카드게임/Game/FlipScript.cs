using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipScript : MonoBehaviour
{
    
    public GameObject Front;
    public GameObject Back;
    public bool CardFBCheck;
    int timer;
    public float x, y, z;
    public void Start()
    {
        timer = 0;
        CardFBCheck = false;
        x = 0;
        y = 1;
        z = 0;

    }
    public void Flip()//카드회전
    {
        CardFBCheck = !CardFBCheck;
        

        if (CardFBCheck)
        {
            Front.SetActive(true);
            Back.SetActive(false);
            
        }
        else
        {
            Front.SetActive(false);
            Back.SetActive(true);
            
        }

    }
    public void StartFlip()
    {
        StartCoroutine(CardFlip());
    }
    IEnumerator CardFlip()
    {
        
        for (int i = 0; i < 60; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Back.transform.Rotate(new Vector3(x, y * 3, z));
            Front.transform.Rotate(new Vector3(x, y * 3, z));

            timer++;
            
            if (timer == 30 || timer == -30)
            {
                
                Flip();

            }
        }
        timer = 0;
    }
}
