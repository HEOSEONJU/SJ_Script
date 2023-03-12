using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_E_Skill : MonoBehaviour
{
    public int Percentage;
    public int Value;
    public int MAX_CoolDown;
    public SKill_Enum TYPESKILL;
    public virtual IEnumerator SkillCoroutine_(EnemyManager _E ,Transform Skill)//실드코루틴
    {
        yield return null;

    }
    
}
public enum SKill_Enum
{
    Attack,
    Shield
};
