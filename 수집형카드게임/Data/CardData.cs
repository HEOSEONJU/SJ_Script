using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public static CardData instance;

    public CardScriptable CardDataFile;//스펠 카드데이터
    public CharCardScriptTable CharDataFile;//캐릭터 데이터
    public GameObject PlayerInfoFile;//로그인한 플레이어 정보
    public EnemyScriptTable EnemyDataFile;//적 데이터
    public Char_Skill_ScriptTable CharSkillFile;//캐릭터 스킬데이터

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
