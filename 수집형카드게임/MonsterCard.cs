using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCard : Card
{
    
    public int CurrentHP;
    public int CurrentAP;
    public int CurrentLevel;
    

    public int MAXHP;
    public int MAXLevel;
    

    public int BASEHP;
    public int BASEAP;
    public int GrowhHP;
    public int GrowhAP;


    public void Setting()
    {

        MAXHP = BASEHP * ((CurrentLevel - 1) * GrowhHP);
        BASEHP = MAXHP;
        CurrentAP = BASEAP * ((CurrentLevel - 1) * GrowhAP);
    }

    

    

}
