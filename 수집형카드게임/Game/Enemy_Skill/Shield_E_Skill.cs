using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Shield_E_Skill : Base_E_Skill
{
    
    public override IEnumerator SkillCoroutine_(EnemyManager _E, Transform Skill)//실드코루틴
    {
        _E.Shield_Point += Value;
        Skill.gameObject.SetActive(true);
        _E.ResetBar();
        _E.ShieldCoolDown = MAX_CoolDown;
        yield return new WaitForSeconds(2.0f);
        _E.EndAttack();

    }
}
