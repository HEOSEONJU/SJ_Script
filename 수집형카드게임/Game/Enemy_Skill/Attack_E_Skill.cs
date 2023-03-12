using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_E_Skill : Base_E_Skill
{
    

    public override IEnumerator SkillCoroutine_(EnemyManager _E, Transform Skill)//공격코루틴
    {
        Vector3 origin = Skill.transform.position;
        float time;
        Skill.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Vector3 TargetPois = _E.Char_Manager.CombatChar[_E.TargetList[0]].transform.position;
        TargetPois = new Vector3(TargetPois.x, TargetPois.y, -15);
        time = 0;
        float boomTime = 0.25f;
        while (time <= boomTime)
        {
            Skill.transform.position = Vector3.Lerp(Skill.transform.position, TargetPois, time);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Skill.gameObject.SetActive(false);//도착
        _E.Skill_HIT_Effect[_E.TYPE + 8].position = TargetPois;
        _E.Skill_HIT_Effect[_E.TYPE + 8].gameObject.SetActive(true);
        StartCoroutine(_E.StopEffect(_E.Skill_HIT_Effect[_E.TYPE + 8]));
        _E.SkillDamaged(_E.TargetList[0], Value);
        int Half_Damage = Value / 2;//직격타
        if (_E.TargetList[0] != 0)//왼쪽데미지
        {
            if (_E.Char_Manager.CombatChar[_E.TargetList[0] - 1].Live)
            {
                Debug.Log("절반데미지" + Half_Damage);
                _E.SkillDamaged(_E.TargetList[0] - 1, Half_Damage);//주변피해
            }
        }
        if (_E.TargetList[0] != 3)//오른쪽데미지
        {
            if (_E.Char_Manager.CombatChar[_E.TargetList[0] + 1].Live)
            {
                _E.SkillDamaged(_E.TargetList[0] + 1, Half_Damage);//주변피해
            }
        }
        Skill.transform.position = origin;
        _E.EndAttack();
        yield return null;
    }
}
