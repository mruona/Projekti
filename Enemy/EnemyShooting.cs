using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour 
{
	// Add this to enemy cannonballs
	Rigidbody2D rigiBall;
	public bool DestroyParent = false;
	private playerHealth PlayerHealth;
	public float Force;
	public float damageAmmount;
	
	void Awake ()
	{
		rigiBall = GetComponent<Rigidbody2D>();
	}

	void Start ()
	{
		Vector2 direction = new Vector2 (0, Force);
		rigiBall.AddRelativeForce(direction, ForceMode2D.Impulse);
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{	

		if (col.gameObject.tag == "Player")
		{
			PlayerHealth = col.gameObject.GetComponent<playerHealth>();
			PlayerHealth.takeDamage(damageAmmount);
			Destroy(gameObject, 0.3f);
		}

		if(DestroyParent)
			Destroy(transform.parent.gameObject, 1f);

		Destroy(gameObject, 1f);
	}

}
