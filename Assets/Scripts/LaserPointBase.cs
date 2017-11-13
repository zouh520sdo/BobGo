using UnityEngine;
using System.Collections;

/// <summary>
/// Laser point's base class, intented to be inherited
/// 
/// </summary>
/// 
public class LaserPointBase : MonoBehaviour {

    public GameObject laserPointPrefab;

    protected static float LASER_LENGTH = 1000f / 100f;
    protected GameObject laser;
    public GameObject theLaser { get { return laser; } }
    protected bool _isReflected;
    protected bool _isRefracted;
    public bool isReflected { get { return _isReflected; } }
    public bool isRefracted { get { return _isRefracted; } }
    public Mirror reflectingMirror;
    public ConvexLens refractingLens;
    protected GameObject reflectedLaserPoint;
    protected GameObject refractedLaserPoint;

    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    // Some essential parts need to be initialized
    // It will also need to be called in inherited class
    protected virtual void OnAwake()
    {
        tag = "LaserPoint";
        gameObject.layer = LayerMask.NameToLayer("Launcher");
        GameObject temp = GameObject.FindGameObjectWithTag("MainLaser");
        laser = Instantiate(temp);
        laser.tag = "Laser";
        SetLaserPosition(); // Set laser's position
        laser.transform.rotation = transform.rotation;
        laser.transform.SetParent(gameObject.transform);
        laser.SetActive(false);

        _isReflected = false;
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    // Corretly positioning laser
    protected virtual void SetLaserPosition()
    {
        if (laser)
        {
            float laserLength = LASER_LENGTH * laser.transform.localScale.y;
            laser.transform.position = transform.position + transform.up * (0.5f * laserLength);
        }
    }

    // Corretly scaling laser
    public virtual void SetLaserScaleFromRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.5f, transform.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("MirrorLayer"));
        if (hit.collider != null)
        {
            // Modify origin laser
            Vector3 scale = laser.transform.localScale;
            scale.y = (hit.distance + 0.5f) / LASER_LENGTH;
            laser.transform.localScale = scale;
            SetLaserPosition();

            /*
            // Modify reflected laser
            reflectedLaserPoint.transform.position = hit.point;
            Vector3 targetRot = Vector3.Reflect(transform.up, hit.normal);
            float rotatedRadian = Mathf.Atan2(targetRot.y, targetRot.x) - Mathf.PI / 2f;
            reflectedLaserPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotatedRadian * Mathf.Rad2Deg);
            LaserPointBase reflected = reflectedLaserPoint.GetComponent<LaserPointBase>();
            //reflected.Fire();
            LauncherBase launcher = transform.parent.GetComponent<LauncherBase>();
            launcher.AddToLaserPoints(reflected);*/
        }
    }

    // Correctly set position and scale of reflecting laser
    public virtual void SetLaserPositionFromRay()
    {
        if (isReflected)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.5f, transform.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("MirrorLayer"));
            if (hit.collider != null)
            {
                // Modify origin laser
                Vector3 scale = laser.transform.localScale;
                scale.y = (hit.distance + 0.5f) / LASER_LENGTH;
                laser.transform.localScale = scale;
                SetLaserPosition();

                // Modify reflected laser
                reflectedLaserPoint.transform.position = hit.point;
                Vector3 targetRot = Vector3.Reflect(transform.up, hit.normal);
                float rotatedRadian = Mathf.Atan2(targetRot.y, targetRot.x) - Mathf.PI / 2f;
                reflectedLaserPoint.transform.rotation = Quaternion.Euler(0f, 0f, rotatedRadian * Mathf.Rad2Deg);
                LaserPointBase reflected = reflectedLaserPoint.GetComponent<LaserPointBase>();
                LauncherBase launcher = transform.parent.GetComponent<LauncherBase>();
                launcher.AddToLaserPointsAndFire(reflected);
                //reflected.Fire();
            }
        }
    }

    public virtual void SetRefractedLaserFromRay()
    {
        if (_isRefracted)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.5f, transform.up, Mathf.Infinity, 1 << LayerMask.NameToLayer("MirrorLayer"));
            if (hit.collider)
            {
                // Modify origin laser
                Vector3 scale = laser.transform.localScale;
                scale.y = (hit.distance + 0.5f) / LASER_LENGTH;
                laser.transform.localScale = scale;
                SetLaserPosition();

                // Modify refracted laser
                refractedLaserPoint.transform.position = hit.point;
                Vector3 localHitPoint = refractingLens.transform.InverseTransformPoint(hit.point);
                float refractedDegree = localHitPoint.x * Mathf.Sign(localHitPoint.y) * ConvexLens.refratedRatio;
                refractedLaserPoint.transform.rotation = transform.rotation * Quaternion.Euler(0f, 0f, refractedDegree);
                LaserPointBase refracted = refractedLaserPoint.GetComponent<LaserPointBase>();
                LauncherBase launcher = transform.parent.GetComponent<LauncherBase>();
                launcher.AddToLaserPointsAndFire(refracted);
            }
        }
    }

    public virtual void Fire()
    {

    }

    public virtual void Stop()
    {

    }

    public virtual void StartReflect(Mirror m)
    {
        reflectingMirror = m;
    }

    public virtual void StopReflect(Mirror m)
    {
        reflectingMirror = null;
    }

    public virtual void StartRefract(ConvexLens c)
    {
        refractingLens = c;
    }

    public virtual void StopRefract(ConvexLens c)
    {
        refractingLens = null;
    }
}