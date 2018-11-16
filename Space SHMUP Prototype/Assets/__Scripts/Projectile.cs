using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 2.0f;

    public Bounds bounds;
    public Vector3 boundsCenterOffset;

    void Awake()
    {
        InvokeRepeating("CheckOffScreen", 0.0f, 2f);
    }

    void Update () {
        Vector3 pos = transform.position;
        pos.y += speed * Time.deltaTime;

        transform.position = pos;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject go = Utils.FindTaggedParent(other.gameObject);

        if (go != null && go.tag == "Enemy")
        {
            Destroy(go);
            Destroy(this.gameObject);
        }
    }

    void CheckOffScreen()
    {
        if (bounds.size == Vector3.zero)
        {
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            boundsCenterOffset = bounds.center - transform.position;
        }

        bounds.center = transform.position + boundsCenterOffset;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if (off != Vector3.zero)
        {
            if (off.y > 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
