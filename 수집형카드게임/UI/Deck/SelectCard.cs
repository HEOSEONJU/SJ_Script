using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCard : MonoBehaviour
{
    public int CardNumber;
    public int CardCCost;
    public string CardName;
    public string CardEffect;


    public void SettingSelect(int num,int cost,string Name, string Effect)
    {


        CardNumber = num;
        CardCCost = cost;
        CardName = Name;
        CardEffect = Effect;
    }
}
