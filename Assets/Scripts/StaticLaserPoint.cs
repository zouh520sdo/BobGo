using UnityEngine;
using System.Collections;

public class StaticLaserPoint : LaserPointBase{

    protected override void OnAwake()
    {
        base.OnAwake();
        Vector3 scale = laser.transform.localScale;
        scale.y *= 5f;
        laser.transform.localScale = scale;
        SetLaserPosition();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (laser.activeInHierarchy)
        {
            SetLaserPositionFromRay();
            SetRefractedLaserFromRay();
        }
    }

    public override void Fire()
    {
        //print(name + " Fire!");
        SetLaserScaleFromRay();

        if (!laser.activeInHierarchy)
            laser.SetActive(true);
        else
        {
            SetLaserPositionFromRay();

            /*RaycastHit2D hit;

            hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity);
            if (hit.collider != null)
            {
                print(name + " hit sth.");
                print(name + " hit distance " + hit.distance);
                print(name + " hit collider " + hit.collider.tag);
                print(name + " hit transform " + hit.transform.tag);
                print(name + " hit name " + hit.transform.name);

                //if (hit.collider.tag == "Bound")
                {
                    print(name + " hit bound.");
                    Vector3 scale = laser.transform.localScale;
                    scale.y = hit.distance / LASER_LENGTH;
                    laser.transform.localScale = scale;
                }
            }
            SetLaserPosition();*/
        }

    }

    public override void Stop()
    {
        //print(name + " Stop!");
        laser.SetActive(false);
    }

    public override void StartReflect(Mirror m)
    {
        base.StartReflect(m);
        if (m.reflectingLasers.Contains(gameObject))
            return;

        _isReflected = true;

        // Delete existing reflected laser
        //Destroy(reflectedLaserPoint);
        //reflectedLaserPoint = null;

        reflectedLaserPoint = Instantiate(laserPointPrefab);
        reflectedLaserPoint.name += m.name;

        // Add to mirror
        LaserPointBase reflected = reflectedLaserPoint.GetComponent<LaserPointBase>();
        m.addToReflectingList(reflected.theLaser);
        reflected.laserPointPrefab = laserPointPrefab;
        reflectedLaserPoint.transform.SetParent(transform.parent);
    }

    public override void StopReflect(Mirror m)
    {
        base.StopReflect(m);
        _isReflected = false;

        LauncherBase launcher = transform.parent.GetComponent<LauncherBase>();

        if (reflectedLaserPoint)
        {
            LaserPointBase reflected = reflectedLaserPoint.GetComponent<LaserPointBase>();
            launcher.RemoveFromLaserPoints(reflected);

            // Recover
            Vector3 scale = laser.transform.localScale;
            scale.y = 5f;
            laser.transform.localScale = scale;
            SetLaserPosition();
            // Remove from mirror
            m.removeFromReflectingList(reflected.theLaser);
            // Delete reflected laser
            Destroy(reflectedLaserPoint);
            reflectedLaserPoint = null;
        }
    }

    public override void StartRefract(ConvexLens c)
    {
        base.StartRefract(c);
        if (c.refractingLasers.Contains(gameObject))
            return;

        _isRefracted = true;

        // Delete existing reflected laser
        //Destroy(reflectedLaserPoint);
        //reflectedLaserPoint = null;

        refractedLaserPoint = Instantiate(laserPointPrefab);
        refractedLaserPoint.name += c.name;

        // Add to mirror
        LaserPointBase refracted = refractedLaserPoint.GetComponent<LaserPointBase>();
        c.addToRefractingList(refracted.theLaser);
        refracted.laserPointPrefab = laserPointPrefab;
        refractedLaserPoint.transform.SetParent(transform.parent);
    }

    public override void StopRefract(ConvexLens c)
    {
        base.StopRefract(c);
        _isRefracted = false;

        LauncherBase launcher = transform.parent.GetComponent<LauncherBase>();

        if (refractedLaserPoint)
        {
            LaserPointBase reflected = refractedLaserPoint.GetComponent<LaserPointBase>();
            launcher.RemoveFromLaserPoints(reflected);

            // Recover
            Vector3 scale = laser.transform.localScale;
            scale.y = 5f;
            laser.transform.localScale = scale;
            SetLaserPosition();
            // Remove from mirror
            c.removeFromRefractingList(reflected.theLaser);
            // Delete reflected laser
            Destroy(refractedLaserPoint);
            refractedLaserPoint = null;
        }
    }
}
