using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerInfoDataObject", order = 1)]
public class PlayerInfoData : ScriptableObject
{
    public List<PlayerData> Player = new List<PlayerData>();

}

