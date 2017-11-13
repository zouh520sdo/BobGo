using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvexLens : MonoBehaviour
{

    // A list to prevent reflected laser infinitely 
    // generates new reflected laser at the same mirror
    public List<GameObject> refractingLasers;

    static float _refratedRatio = 60f * 100f / 200f;
    public static float refratedRatio { get { return _refratedRatio; } }

    void Awake()
    {
        refractingLasers = new List<GameObject>();
        gameObject.layer = LayerMask.NameToLayer("MirrorLayer");
        tag = "Mirror";
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Laser" && col.gameObject.activeSelf && !refractingLasers.Contains(col.gameObject))
        {
            print(name + " " + col.transform.parent.name);

            LaserPointBase laserPoint = col.transform.parent.GetComponent<LaserPointBase>();
            laserPoint.StartRefract(this);
        }
    }
    
    void OnTriggerStay2D (Collider2D col)
    {

    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Laser" && col.gameObject.activeSelf && !refractingLasers.Contains(col.gameObject))
        {
            print(name + " " + col.transform.parent.name);

            LaserPointBase laserPoint = col.transform.parent.GetComponent<LaserPointBase>();
            laserPoint.StopRefract(this);
        }
    }

    public void addToRefractingList(GameObject go)
    {
        if (!refractingLasers.Contains(go))
        {
            refractingLasers.Add(go);
        }
    }

    public void removeFromRefractingList(GameObject go)
    {
        if (refractingLasers.Contains(go))
        {
            refractingLasers.Remove(go);
        }
    }
}