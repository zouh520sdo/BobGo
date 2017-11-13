using UnityEngine;
using System.Collections;

/// <summary>
/// Purly rotation controlling script
/// </summary>

public class Rotate : MonoBehaviour {

    public float angluarSpeed = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * angluarSpeed * Time.deltaTime);
    }
}
