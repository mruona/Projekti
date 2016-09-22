using UnityEngine;
using System.Collections;
[RequireComponent (typeof (Rigidbody2D))]

public class HitDamage : MonoBehaviour 
{
//	Hit damage script
//	Add this to hitting object

	Rigidbody2D rigidB;
	private playerHealth PH;
	
	float kineticenergy;
	[Range(1, 30)] public float damageModifier = 15;
	[Range(0, 15)] public int kineticenergyTreshold = 1;
	
	public bool nonKineticDamage = true;

	void Start()
	{
		rigidB = GetComponent<Rigidbody2D>();
		PH = GameObject.Find("Player").GetComponent<playerHealth>();
	}
	
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Player")
		{
			if(nonKineticDamage)
			{
					PH.takeDamage(rigidB.velocity.magnitude * damageModifier);
			}
			
			else
			{
				kineticenergy = Mathf.Pow(coll.relativeVelocity.magnitude, 2) * coll.rigidbody.mass;
				if (kineticenergy > kineticenergyTreshold)
					PH.takeDamage((float)System.Math.Sqrt (kineticenergy) * damageModifier);
			}
		}
	}
}
