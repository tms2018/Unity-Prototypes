using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    public static bool goalMet = false;
	// Use this for initialization
    void OnTriggerEnter( Collider other)
    {
        if ( other.gameObject.tag == "Projectile" )
        {
            Goal.goalMet = true;
            Renderer renderer = gameObject.GetComponent<Renderer>();
            Color c = renderer.material.color;
            c.a = 1;
            renderer.material.color = c;
        }
    }
}