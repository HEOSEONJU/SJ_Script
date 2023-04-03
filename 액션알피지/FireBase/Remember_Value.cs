using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Remember")]
public class Remember_Value : ScriptableObject
{
    [SerializeField]
    string ID;
    [SerializeField]
    string Password;


    public string ID_Value
    {
        get { return ID; }
        
    }
    public string PW_Value
    {
        get { return Password; }
        
    }


    public void Remember_Member_Data(string key=null, string value=null)
    {
        ID = key;
        Password=value;
    }

}
