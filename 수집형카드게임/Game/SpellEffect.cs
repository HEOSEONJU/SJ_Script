using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect//적용된 카드의 효과와 지속시간을 기록하는 클래스
{

    public int ID;
    public int TURN;

    public List<Effect_Value> Effect_Type_Value;
    

    public void ClearValue()
    {
        Effect_Type_Value=new List<Effect_Value>();
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

}