using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Class use to manager circle pads' dialog.
/// Will be called in Chitchat class
/// </summary>

public class DialogManager : MonoBehaviour {

    public float up;
    public bool bInProcess;
    public bool bInFlush;
    public int flushIndex;

    public Text dialog;
    RectTransform rt;


	// Use this for initialization
	void Start () {
        bInProcess = false;
        bInFlush = false;
        flushIndex = 0;
        rt = dialog.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * up);
        rt.position = screenPos;
    }

    public void showDialog(string text)
    {
        dialog.text = text;
        dialog.gameObject.SetActive(true);
    }

    public void hideDialog()
    {
        dialog.gameObject.SetActive(false);
    }
}
