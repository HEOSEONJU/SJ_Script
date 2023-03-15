using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Test_Enemy : MonoBehaviour
{
    float HP = 0;

    public  void Damaged(float Damaged_Point , int Type = 0)
    {
        HP -= Damaged_Point;
        
    }



}
