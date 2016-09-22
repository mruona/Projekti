using UnityEngine;
using System.Collections;

public class AreaDamage : MonoBehaviour 
{
// 	Area damage script
// 	Add this to collider object

	private float nextDamage;
	[Range(0, 5)] public float damageRate = 1f;
	[Range(1, 50)] public float damageValue = 5f;
	
	private playerHealth PH;
	
	void Start()
	{
		PH = GameObject.Find("Player").GetComponent<playerHealth>();
	}

//	On trigger collider
	void OnTriggerStay2D(Collider2D other) 
	{
		if(Time.time > nextDamage && other.gameObject.tag == "Player")
		{
			nextDamage = Time.time + damageRate;
			PH.takeDamage(damageValue);
		}
	}

//	On normal collision 
	void OnCollisionStay2D(Collision2D other) 
	{
		if(Time.time > nextDamage && other.gameObject.tag == "Player")
		{
			nextDamage = Time.time + damageRate;
			PH.takeDamage(damageValue);
		}
	}
}
