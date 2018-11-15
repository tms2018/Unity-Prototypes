using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
    [Header("Set in inspector:")]
    public static Hero S;
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public Bounds bounds;
    public float gameRestartDelay = 2f;

    [Header("Set in script:")]
    [Space(10)]
    [SerializeField]
    private float _shieldLevel = 1f;
    public GameObject lastTriggerGo = null;

    void Awake()
    {
        S = this;
        bounds = Utils.CombineBoundsOfChildren(gameObject);
    }

	
	// Update is called once per frame
	void Update () {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;

        bounds.center = transform.position;

        Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
        if(off != Vector3.zero)
        {
            pos -= off;
        }
        transform.position = pos;
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
	}

    void OnTriggerEnter(Collider other)
    {
        //Find the tag of the collided game object
        GameObject go = Utils.FindTaggedParent(other.gameObject);

        if (go != null)
        {
            if(go == lastTriggerGo)
            {
                return;
            }
            lastTriggerGo = go;

            if( go.tag == "Enemy")
            {
                shieldLevel--;
                Destroy(go);
            }
            else
            {
                print("Triggered: " + go.name);
            }
        }
        else
        {
            print("Triggered: " + other.gameObject.name);
        }

    }

    public float shieldLevel
    {
        get
        {
            return _shieldLevel;
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);
            if(value < 0)
            {
                Destroy(this.gameObject);
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}