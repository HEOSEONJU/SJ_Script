using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect
{

    public int ID;
    public int TURN;

    public List<Effect_Value> Effect_Type_Value;
    /*
    public int Value_Enemy_Effect_Num;
    public int Value_Char_Effect_Num;

    public int Effect_PercentATK;
    public int Effect_PercentDEF;
    public int Effect_PercentHP;

    public int Effect_ATK;
    public int Effect_DEF;
    public int Effect_HP;
    
    public int Magic_Regi;
    public int Regen_HP;
    public int Effect_AttackCount;
    public int TypeChecker;
    public int CP;
    public int CD;
    public int DrewCount;
    */



    public void ClearValue()
    {
        
        Effect_Type_Value=new List<Effect_Value>();
}

private void OnEnable()
    {
        
}

    public void Init(int _ID)
    {
        
        ID= _ID;

    }
    

    public bool UsingTurn()
    {
        TURN -= 1;

        if(TURN<=0)
        {
            
            return true;
            
        }
        return false;
    }
    public void DestroyEffect()
    {
        //Destroy(gameObject);
    }
}
/*
    public void Apply_Skill_Effect(CharSkillData temp)
    {
        ClearValue();
        TURN = temp.Turn;
        /*
        Effect_ATK = temp.Value_A;
        Effect_DEF = temp.Value_D;
        Effect_HP = temp.Value_H;
        Magic_Regi = temp.Value_MR;
        Effect_AttackCount = temp.Value_AC;
        TypeChecker = temp.Value_TY;
        Regen_HP = temp.Value_R;
        CP = temp.Value_CP;
        CD = temp.Value_CD;
        DrewCount = temp.Value_Drew;
        Effect_PercentATK = temp.Value_PA;
        Effect_PercentDEF = temp.Value_PD;
        Effect_PercentHP = temp.Value_PH;

        Value_Enemy_Effect_Num=temp.Value_Enemy_Effect_Num;
        Value_Char_Effect_Num=temp.Value_Char_Effect_Num;
        


    }
    
    public void Apply_Spell_Effect(SpellCard temp)
    {
        ClearValue();
        TURN = temp.Turn;
        /*
        Effect_ATK = temp.Value_A;
        Effect_DEF = temp.Value_D;
        Effect_HP = temp.Value_H;
        Magic_Regi = temp.Value_MR;
        Effect_AttackCount = temp.Value_AC;
        TypeChecker = temp.Value_TY;
        Regen_HP = temp.Value_R;
        CP = temp.Value_CP;
        CD = temp.Value_CD;
        DrewCount = temp.Value_Drew;
        Effect_PercentATK=temp.Value_PA;
        Effect_PercentDEF=temp.Value_PD;
        Effect_PercentHP=temp.Value_PH;
        Value_Enemy_Effect_Num = temp.Value_Enemy_Effect_Num;
        Value_Char_Effect_Num = temp.Value_Char_Effect_Num;
        
    }




    public void Setting_Multiple_Effect(int Turn,int Atk=0,int Def=0,int Hp=0,int MagicRegi=0,int AttackCount=0,int CharType=0,int Regen_hp=0,int Cp=0,int Cd=0,int Drewcount=0)//다중효과시사용
    {
        ClearValue();
        TURN = Turn;
        /*
        Effect_ATK = Atk;
        Effect_DEF= Def;
        Effect_HP= Hp;
        Magic_Regi = MagicRegi;
        Effect_AttackCount = AttackCount;
        TypeChecker = CharType;
        Regen_HP = Regen_hp;
        CP = Cp;
        CD = Cd;
        DrewCount= Drewcount;
        

    }



*/