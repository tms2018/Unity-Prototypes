using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {
	//Variables to set in the inspector
	public Vector3 moveDirection;
	public float speed = 10;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Rigidbody>().AddForce( moveDirection.normalized * speed );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
