using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public Transform shotPrefab;
	public bool isEnemy;

	public void DelayedAttack(float timeInSeconds)
	{
		Invoke ("Attack",timeInSeconds);
	}
	
	public void Attack(){

			//create new shot at shooter's position
			var shotTransform = Instantiate (shotPrefab) as Transform;
			shotTransform.position = transform.position;

			ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
			if (shot != null){
				shot.isEnemyShot = isEnemy;
			}		
	}
}
