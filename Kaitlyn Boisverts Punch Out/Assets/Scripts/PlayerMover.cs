using UnityEngine;
using System.Collections;

public class PlayerMover : MonoBehaviour {
	
	private WeaponScript fist;
	public Animator anim;
	private float attackTimer = 0.0f;
	public int health = 10;
	public GameObject menu;
	
	void Start()
	{
		fist = GetComponentInChildren<WeaponScript>();
	}
	// Update is called once per frame
	void Update () {
		if( GameObject.Find("Enemy") == null || health == 0 ) {
			anim.Play("Win");
			menu.SetActive(true);
		}
		if( health == 0 ){
			SpecialEffectsHelper.Instance.Explosion(transform.position);
			Destroy(gameObject);
		}
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
		
		attackTimer -= Time.deltaTime;
		if( info.IsName("Base.Idle") )
		{	
			anim.SetFloat("horizontalMotion", Input.GetAxis("Horizontal"));
			anim.SetBool("isLeftPunchPressed", Input.GetButtonDown("Fire1"));
		}
		else if ( info.IsName("Base.PunchHighLeft"))
		{
			anim.SetBool("isLeftPunchPressed", false);
			if(attackTimer < 0.0f)
			{
				fist.DelayedAttack(0.0f);
				attackTimer = 0.37f;			
			}
		}
		else
		{
			anim.SetBool("isPunched", false);
		}
	}
	
	
	void OnTriggerExit2D(Collider2D otherCollider)
	{	
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null){
			if( shot.isEnemyShot == true){
				anim.SetBool("isPunched", true);
				health--;
			}
		}
	}
}
