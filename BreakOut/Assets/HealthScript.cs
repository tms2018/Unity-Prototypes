using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	//Public Variables to set in the inspector
	public int health;
	// Use this for initialization
	void Start () {
		health = 1;
		
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision collision)
	{
		health -= 1;
		if ( health <= 0 )
		Destroy( gameObject );
		
	}
}
