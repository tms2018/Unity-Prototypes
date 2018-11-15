﻿using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    [Header("ADJUSTABLE IN INSPECTOR")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;

    [Header("SET IN SCRIPT")]
    [Space(10)]
    public Bounds bounds;
    public Vector3 boundsCenterOffset;
	
    // Use this for initialization
	void Awake () {
        InvokeRepeating("CheckOffScreen", 0f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void CheckOffScreen()
    {
        if(bounds.size == Vector3.zero)
        {
            bounds = Utils.CombineBoundsOfChildren(this.gameObject);
            boundsCenterOffset = bounds.center - transform.position;
        }

        bounds.center = transform.position + boundsCenterOffset;
        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.offScreen);
        if( off != Vector3.zero)
        {
            if (off.y < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}