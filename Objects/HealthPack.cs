using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour 
{
//	Add this to health pack Object

//	Player health component
	private playerHealth PH;
	
	public float healthAmmount;

	void Start()
	{
		PH = GameObject.FindWithTag("Player").GetComponent<playerHealth>();
	}

//	If player enter trigger zone -> heals and destroys health pack
	void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.gameObject.tag == "Player")
		{
			PH.healDamage(healthAmmount);
			Destroy(gameObject, 0f);
		}
	}
}
