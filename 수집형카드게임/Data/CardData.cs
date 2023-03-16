using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public static CardData instance;

    public CardScriptable CardDataFile;//���� ī�嵥����
    public CharCardScriptTable CharDataFile;//ĳ���� ������
    public GameObject PlayerInfoFile;//�α����� �÷��̾� ����
    public EnemyScriptTable EnemyDataFile;//�� ������
    public Char_Skill_ScriptTable CharSkillFile;//ĳ���� ��ų������

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
