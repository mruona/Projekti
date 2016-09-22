using UnityEngine;
using System.Collections;

public class shotFiredEnemy : MonoBehaviour 
{
//	Use this for turret ammo

	Rigidbody2D rigiBall;
	public bool destroyParent = false;
	public Vector2 forceDirection = new Vector2 (0, 10f);	

	void Awake ()
	{
		rigiBall = GetComponent<Rigidbody2D>();
	}
	
	void Start ()
	{
			rigiBall.AddRelativeForce(forceDirection, ForceMode2D.Impulse);
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{	
			Destroy(gameObject, 0f);
			if(destroyParent)
				Destroy(transform.parent.gameObject, 2f);
	}
}
