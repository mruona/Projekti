using UnityEngine;
using System.Collections;

public class shotFired : MonoBehaviour 
{
	// Add this to player cannonballs

	Rigidbody2D rigiBall;
	public bool DestroyParent = false;
	private EnemyHealth enemyHealth;
	public float HitDamage;
	
	void Awake ()
	{
		rigiBall = GetComponent<Rigidbody2D>();
	}
	
	void Start ()
	{
		Vector2 force = new Vector2 (0, 30f);
		rigiBall.AddRelativeForce(force, ForceMode2D.Impulse);
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{	

		if (col.gameObject.tag == "Enemy")
		{
			col.gameObject.SendMessage("enemyTakeDamage", HitDamage);
			Destroy(gameObject, 0.5f);
		}
		else
			Destroy(gameObject, 1f);
		
		if(DestroyParent)
			Destroy(transform.parent.gameObject, 1f);
	}
}
