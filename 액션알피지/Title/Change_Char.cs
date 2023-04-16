using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Change_Char : MonoBehaviour
{
    [SerializeField]
    FireBase_Manager FBM;

    [SerializeField]
    GameObject Login;
    [SerializeField]
    GameObject Char_;


    [SerializeField]
    List<GameObject> Char_List;


    bool isLogin = false;
    public void Move_Char()
    {
        Login.SetActive(false);
        Char_.SetActive(true); ;
    }


    public void Choice(int i)
    {
        if(isLogin)
        {
            return;
        }
        isLogin=true;
        FBM.Player_Char = Char_List[i];
        
        FBM.Set_ID = i;
        
        StartCoroutine(FBM.SceneLoading("Game Scene"));
    }
    
}
