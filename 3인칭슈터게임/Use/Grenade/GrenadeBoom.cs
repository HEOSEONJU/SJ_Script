using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GrenadeBoom : MonoBehaviour
{

    public float Delay;
    public float CountDown;
    bool ThrowStart = false;
    public Vector3 Origin;
    public Vector3 ThrowPoint;
    public GameObject Effect;
    //public float Power = 40.0f;
    public LineRenderer MoveLine;
    bool BC;
    float Radius = 5.0f;
    float Size = 0.1f;
    int Damage_Point=400;
    // Start is called before the first frame update
    private void Start()
    {
        BC = false;
        Origin = transform.position;
        Delay = 1.0f;
        ThrowStart = false;
        Effect = transform.GetChild(0).gameObject;




    }
    public void Throw(Vector3 Direction, float Power, float Angle)
    {
        ThrowStart = true;
        CountDown = Delay;
        ThrowPoint = transform.position;
        StartCoroutine(Movement(Direction, Power, Angle));

    }


    IEnumerator Movement(Vector3 direction, float power, float angle)
    {
        int count = 0;
        while (MoveLine.positionCount >= count)
        {
            transform.position = MoveLine.GetPosition(count);
            count += 5;
            ExplodeCheck();
            yield return null;
        }
    }

    public void ExplodeCheck()
    {
        RaycastHit[] hit = Physics.SphereCastAll(transform.position, Size, transform.forward, 0.1f);

        if (hit != null)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                Base_HP temp = hit[i].transform.GetComponent<Base_HP>();
                if(temp != null)                
                {
                    Explode();
                    return;
                }
                else if (hit[i].collider.gameObject.layer == 8)
                {
                    Explode();
                    return;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (ThrowStart == true)
        {
            CountDown -= Time.deltaTime;
            if (CountDown <= 0 & ThrowStart == true)
            {
                Explode();
            }
        }
    }
    void Explode()
    {
        #region//기초세팅
        ThrowStart = false;
        RaycastHit[] hit;
        CountDown = Delay;
        Effect.SetActive(true);
        LayerMask mask = LayerMask.NameToLayer("Enemy");
        #endregion
        hit = Physics.SphereCastAll(transform.position, Radius, transform.forward, 0.1f);
        if (hit.Length > 0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                Base_HP temp = hit[i].transform.GetComponent<Base_HP>();
                if (temp != null)
                {
                    temp.Damged(Damage_Point);
                }
            }
        }
        #region//초기화
        Effect.transform.parent = null;
        Invoke("findParent", 3.0f);
        transform.rotation = Quaternion.identity;
        transform.position = Origin;
        StopAllCoroutines();
        #endregion

    }
    void findParent()
    {
        Effect.transform.parent = transform;
        Effect.transform.localPosition = new Vector3(0, 4, 0);
        Effect.SetActive(false);
    }

}

