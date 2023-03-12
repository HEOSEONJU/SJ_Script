using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HaveCardData
{
    public int id;
    
    

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HaveCardDataObject", order = 1)]
public class HaveScriptTable : ScriptableObject
{
    public List<int> cards = new List<int>();

}