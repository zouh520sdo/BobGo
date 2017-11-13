using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Laser launcher's base class
/// </summary>
public class LauncherBase : MonoBehaviour {

    public List<LaserPointBase> laserPoints;
    public delegate void LasersFire();
    public delegate void LasersStop();

    // Hold all laser fire and stop function of laser points which are attachted
    // to this launcher
    public LasersFire lasersFire;
    public LasersStop lasersStop;

    void Awake()
    {
        OnAwake();
    }

	// Use this for initialization
	void Start () {
        OnStart();
    }
	
	// Update is called once per frame
	void Update () {
        OnUpdate();
	}

    protected virtual void OnAwake()
    {
        tag = "Launcher";
        gameObject.layer = LayerMask.NameToLayer("Launcher");
        
        laserPoints = new List<LaserPointBase>();
        foreach (Transform child in transform)
        {
            LaserPointBase laserPoint = child.GetComponent<LaserPointBase>();
            AddToLaserPoints(laserPoint);
        }
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    // Use to add reflected laser point's function to this launcher and call fire function
    public virtual void AddToLaserPointsAndFire(LaserPointBase laserPoint)
    {
        if (laserPoint && !laserPoints.Contains(laserPoint))
        {
            laserPoints.Add(laserPoint);
            lasersFire += laserPoint.Fire;
            lasersStop += laserPoint.Stop;
            laserPoint.Fire();
        }
    }

    // Use to add reflected laser point's function to this launcher
    public virtual void AddToLaserPoints(LaserPointBase laserPoint)
    {
        if (laserPoint && !laserPoints.Contains(laserPoint))
        {
            laserPoints.Add(laserPoint);
            lasersFire += laserPoint.Fire;
            lasersStop += laserPoint.Stop;
        }
    }

    // Use to remove reglected laser point's function from this launcher
    public virtual void RemoveFromLaserPoints(LaserPointBase laserPoint)
    {
        if (laserPoint && laserPoints.Contains(laserPoint))
        {
            laserPoints.Remove(laserPoint);
            lasersFire -= laserPoint.Fire;
            lasersStop -= laserPoint.Stop;
        }
    }
}