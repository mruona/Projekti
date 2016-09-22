using UnityEngine;
using System.Collections;

public class InvulnerableBoost : MonoBehaviour 
{
//	Invulnerable time
	public float boostTime;

//	Player component
	private playerHealth PH;
	
	void Start()
	{
		PH = GameObject.Find("Player").GetComponent<playerHealth>();
	}

//	When plyer enters trigger zone, Starts coroutine in player health script, destroys boost object
	void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.gameObject.tag == "Player")
		{
			PH.StartCoroutine("Invulnerable", boostTime);
			Destroy(gameObject, 0f);
		}
	}
}
