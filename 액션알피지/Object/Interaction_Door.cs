using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Door : Interaction_Function
{
    [SerializeField]
    Animation OpenDoor;


    [SerializeField]
    string Open;
    [SerializeField]
    string Close;



    public override void Open_Window()
    {
        OpenDoor.Play(Open);
    }
    public override void Close_Window()
    {
        OpenDoor.Play(Close);
    }
}
