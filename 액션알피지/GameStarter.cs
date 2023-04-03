using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Start()
    {
        FireBase_Manager FBM = GameObject.Find("FireBase").GetComponent<FireBase_Manager>();
        FBM.NPC_Setting();
        StartCoroutine(FBM.Init_Player_Char());

    }
}
