using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{

    public Gun MyGun;

    public GameObject beamLineRendererPrefab;
    public GameObject beamStartPrefab;
    public GameObject beamEndPrefab;
    public LineRenderer line;
    public float textureScrollSpeed = 0f; //How fast the texture scrolls along the beam, can be negative or positive.
    public float textureLengthScale = 1f;   //Set this to the horizontal length of your texture relative to the vertical. 
    public bool LaserDraw;
    public Vector3 LaserEnd;
    public Vector3 LaserVector = Vector3.zero;





    public void LaserStart()
    {

    }


    void IineLaser(RaycastHit hit)
    {
        beamStartPrefab.transform.position = MyGun.L2.transform.position;
        beamStartPrefab.transform.LookAt(hit.point);

        beamEndPrefab.transform.position = hit.point;
        beamEndPrefab.transform.LookAt(beamStartPrefab.transform.position);

        line.SetPosition(1, hit.point);
        



        float distance = Vector3.Distance(MyGun.L2.transform.position, hit.point);
        line.material.mainTextureScale = new Vector2(distance / textureLengthScale, 1); //This sets the scale of the texture so it doesn't look stretched
        line.material.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0); //This scrolls the texture along the beam if not set to 0
        LaserVector = hit.point;
    }
}
