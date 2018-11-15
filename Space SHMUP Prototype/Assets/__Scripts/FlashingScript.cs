using UnityEngine;
using System.Collections;

public class FlashingScript : MonoBehaviour {
    IEnumerator Flashing()
    {
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        Sprite sprite = playerSprite.sprite;
        while (true)
        {
            playerSprite.sprite = null;
            yield return new WaitForSeconds(0.2f);
            playerSprite.sprite = sprite;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(Flashing());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
