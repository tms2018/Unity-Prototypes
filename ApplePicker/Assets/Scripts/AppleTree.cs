﻿using UnityEngine;
using System.Collections;

public class AppleTree : MonoBehaviour {
    public GameObject applePrefab;
    public float speed = 1f;
    public float leftAndRightEdge = 10f;
    public float chanceToChangeDirections = 0.1f;
    public float secondsBetweenAppleDrops = 1f;
	
	void Start ()
    {
        InvokeRepeating("DropApple", 2, secondsBetweenAppleDrops);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x += (speed * Time.deltaTime);
        transform.position = pos;

        if( transform.position.x > leftAndRightEdge )
        {
            speed = -Mathf.Abs(speed);
        }
        if( transform.position.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed);
        }
    }

    void FixedUpdate()
    {
        if (Random.value <= chanceToChangeDirections)
        {
            speed *= -1;
        }
    }

    void DropApple()
    {
        GameObject apple = Instantiate(applePrefab) as GameObject;
        apple.transform.position = transform.position;
    }
}
