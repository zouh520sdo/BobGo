using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotatingLauncher : LauncherBase {

    public float angleSpeed = 20f;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Rotate();
        /*if (lasersFire != null)
        {
            lasersFire();
        }*/
        /*if (lasersStop != null)
        {
            lasersStop();
        }*/
    }

    void Rotate()
    {
        transform.Rotate(Vector3.forward * angleSpeed * Time.deltaTime);
    }
}
