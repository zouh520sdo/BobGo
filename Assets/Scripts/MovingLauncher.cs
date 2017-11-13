using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingLauncher : LauncherBase {

    public float moveSpeed = 3f;
    private int direction = 1;

    static List<GameObject> boundObjects;
    static Dictionary<string, float> bounds;

    protected override void OnAwake()
    {
        base.OnAwake();
        if (boundObjects == null)
        {
            boundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Bound"));
        }
        if (bounds == null)
        {
            float[] boundsX = new float[boundObjects.Count];
            float[] boundsY = new float[boundObjects.Count];
            for (int i=0; i < boundObjects.Count; i++)
            {
                boundsX[i] = boundObjects[i].transform.position.x;
                boundsY[i] = boundObjects[i].transform.position.y;
            }
            bounds = new Dictionary<string, float>();
            bounds["left"] = Mathf.Min(boundsX);
            bounds["right"] = Mathf.Max(boundsX);
            bounds["up"] = Mathf.Max(boundsY);
            bounds["down"] = Mathf.Min(boundsY);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        moveHorizontal();
        /*
        if (lasersFire != null)
        {
            lasersFire();
        }*/
    }

    public void moveHorizontal()
    {
        Vector3 position = transform.position;
        if (position.x <= bounds["left"] || position.x >= bounds["right"]
            || position.y >= bounds["up"] || position.y <= bounds["down"])
        {
            direction *= -1;
        }
        transform.Translate(direction * moveSpeed * Time.deltaTime * transform.right, Space.World);
    }
}