using UnityEngine;
using System.Collections;

public class NormalPad : PadBase {

    protected override void OnAwake()
    {
        base.OnAwake();
        speed = 8f;
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isInvulnerable && col.collider.tag == "Laser")
        {
            GameManager.RemovePad(this);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isInvulnerable && col.tag == "Laser")
        {
            GameManager.RemovePad(this);
        }
    }


    public override void Move(Vector2 pos)
    {
        base.Move(pos);
        Vector2 dir = pos - (Vector2)transform.position;
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
