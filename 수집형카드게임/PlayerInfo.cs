using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int cost;
    public int Maxcost;
    public int recoverycost;

    public void init()
    {
        cost = 0;
        Maxcost = 0;
        recoverycost = 5;
    }

    public void StartTurn()
    {
        cost += recoverycost;
        if(cost > Maxcost)
        {
            cost = Maxcost;
        }
    }
    public bool UseCost(int n)
    {
        if (cost < n)
            return false;
        else
        {
            cost -= n;
            return true;
        }


    }

    public void CalCost(int n)
    {
        recoverycost +=n;
    }


}
