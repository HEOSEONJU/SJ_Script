using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckCardData
{
    public int id;
    

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DeckCardDataObject", order = 1)]
public class DeckScriptTable : ScriptableObject
{
    public List<int> cards = new List<int>();

}
