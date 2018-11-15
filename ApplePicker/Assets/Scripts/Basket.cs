using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Basket : MonoBehaviour {
    Text scoreText;

	// Use this for initialization
	void Start () {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        scoreText = scoreGO.GetComponentInChildren<Text>();
        scoreText.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;

        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
        
	}

    void OnCollisionEnter( Collision coll)
    {
        GameObject collidedWith = coll.gameObject;

        if( collidedWith.tag == "Apple")
        {
            Destroy(collidedWith);
        }

        int score = int.Parse(scoreText.text);
        score += 1;
        scoreText.text = score.ToString();
    }
}
