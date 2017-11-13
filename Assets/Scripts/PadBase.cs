using UnityEngine;
using System.Collections;

/// <summary>
/// Circle pad's base class
/// </summary>

public class PadBase : MonoBehaviour {

    public bool isSelected;
    public bool isInvulnerable;
    protected DialogManager dm;

    [SerializeField]
    protected float speed;

    void Awake()
    {
        OnAwake();
        dm = GetComponent<DialogManager>();
    }

    // Use this for initialization
    void Start()
    {
        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnAwake()
    {
        tag = "Pad";
        gameObject.layer = LayerMask.NameToLayer("Pad");
        isSelected = false;
        isInvulnerable = false;
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    public virtual void Move(Vector2 pos)
    {

    }
}
