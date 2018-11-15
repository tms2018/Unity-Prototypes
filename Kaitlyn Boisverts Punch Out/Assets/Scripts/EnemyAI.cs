using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	
	private WeaponScript fist;
	public Animator anim;
	public int health = 30;
	
	private AnimatorStateInfo info;
	private float timeBetweenAttacks = 3.0f;
	private int timesHit = 0;
	// Update is called once per frame
	void Start()
	{
		fist = GetComponentInChildren<WeaponScript>();
	}
	
	void Update () {
		info = anim.GetCurrentAnimatorStateInfo(0);
		Debug.Log(health);
		timeBetweenAttacks -= Time.deltaTime;
		if( health == 0 )
		{
			SpecialEffectsHelper.Instance.Explosion(transform.position);
			Destroy(gameObject);
		}
		if( info.IsName("Base.Idle"))
		{	
			timesHit = 0;
			anim.SetInteger("TimesHit", 0);
			if(timeBetweenAttacks < 0)
			{
				Attack();
			}
		}
		else
		{
			anim.SetBool("attack", false);
		}
		
	}
	
	void Attack()
	{
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

		if( info.IsName("Base.Idle"))
		{
			anim.SetBool("attack",true);
			fist.DelayedAttack(0.7f);
			timeBetweenAttacks = 3 * Random.value + 2;
		}
	}
	
	void OnTriggerExit2D( Collider2D other )
	{
		ShotScript shot = other.gameObject.GetComponent<ShotScript>();
		if (shot != null){
			if(shot.isEnemyShot == false && !info.IsName("Base.Attack")){
				anim.SetBool("isPunched", true);
				anim.SetInteger("TimesHit", ++timesHit);
				health--;
			}
		}	
	}
}
