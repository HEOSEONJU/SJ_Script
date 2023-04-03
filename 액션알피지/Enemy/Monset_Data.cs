using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Monster/Monser_Data")]
public class Monset_Data : ScriptableObject
{
    public string Name;
    public int Monster_ID;
    public int MAXHP;
    public int SPeed;
    public int Attack;
    
    [SerializeField]
    public Drop_Table_Script Drop;
}
