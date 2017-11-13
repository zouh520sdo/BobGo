using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Mirror class
/// Detect incoming laser and generated outgoing laser
/// to achieve Reflection.
/// </summary>

public class Mirror : MonoBehaviour {

    // A list to prevent reflected laser infinitely 
    // generates new reflected laser at the same mirror
    public List<GameObject> reflectingLasers;

    void Awake()
    {
        tag = "Mirror";
        gameObject.layer = LayerMask.NameToLayer("MirrorLayer");
        reflectingLasers = new List<GameObject>();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

	}

    // Do nothing
    void OnCollisionEnter2D(Collision2D col)
    {
        print("Mirror enter");
    }

    // Do nothing
    void OnCollisionExit2D(Collision2D col)
    {
        print("Mirror Exit");
    }

    // Do nothing
    void OnCollisionStay2D(Collision2D col)
    {
        print(col.collider.tag);
        if (col.collider.tag == "Laser")
        {
            print("Laser mirror");
            print(col.transform.parent.name);
        }
    }

    // When laser enter, generate reflected laser
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Laser" && col.gameObject.activeSelf && !reflectingLasers.Contains(col.gameObject))
        {
            print(name + " " + col.transform.parent.name);
            LaserPointBase laserPoint = col.transform.parent.GetComponent<LaserPointBase>();
           /* if (laserPoint.reflectingMirror != null && laserPoint.reflectingMirror != this)
                laserPoint.StopReflect(laserPoint.reflectingMirror);
                */
            laserPoint.StartReflect(this);
        }
    }

    // Do nothing
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Laser" && col.gameObject.activeSelf && !reflectingLasers.Contains(col.gameObject))
        {
            /*LaserPointBase laserPoint = col.transform.parent.GetComponent<LaserPointBase>();
            laserPoint.SetLaserPositionFromRay();*/
        }
    }

    // When laser exit, remove reflected laser
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Laser" && col.gameObject.activeSelf && !reflectingLasers.Contains(col.gameObject))
        {
            //reflectingLasers.Remove(col.gameObject);
            LaserPointBase laserPoint = col.transform.parent.GetComponent<LaserPointBase>();
            laserPoint.StopReflect(this);
        }
    }

    public void addToReflectingList(GameObject go)
    {
        if (!reflectingLasers.Contains(go))
        {
            reflectingLasers.Add(go);
        }
    }

    public void removeFromReflectingList(GameObject go)
    {
        if (reflectingLasers.Contains(go))
        {
            reflectingLasers.Remove(go);
        }
    }
}
