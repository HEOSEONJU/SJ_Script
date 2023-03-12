using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Deck : MonoBehaviour
{
    [SerializeField]
    Manager manager;
    [SerializeField]
    Canvas CounterCanvas;

    private void OnMouseOver()
    {
        //Debug.Log("�������콺�ö�");
        manager.DeckMouseOver(CounterCanvas);
    }

    private void OnMouseExit()
    {


        manager.DeckMouseExit(CounterCanvas);
    }
}
