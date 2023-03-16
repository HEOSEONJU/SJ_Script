using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    
    // Start is called before the first frame update
    public Transform Player;
    public float dis;
    private void Start()
    {
        Player = GameManager.instance.Char_Player_Attack.transform;
        GetComponent<MeshRenderer>().enabled = false;
    }

    

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.layer==9)
        {
            Debug.Log("´ê´ÂÁß");
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.layer == 9)
        {
            Debug.Log("ÇØÁ¦");
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
