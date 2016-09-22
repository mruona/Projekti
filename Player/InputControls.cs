using UnityEngine;
using System.Collections;

public class InputControls : MonoBehaviour 
{
// 	Input Controls
	Rigidbody2D rigidB;
	
	public float turnspeed;
	public float speed;
	
	private float horizontalInput;
	
	public GameObject shot;
	public Transform shotSpawnRight;
	public Transform shotSpawnLeft;

	[HideInInspector]
	public float nextFire;
	public float fireRate;
	
	void Start ()
	{
		rigidB = GetComponent<Rigidbody2D>();
	}
			
	void FixedUpdate ()
	{
		horizontalInput = Input.GetAxis("Horizontal");
		if (horizontalInput != 0)
			TurnShip();

		if (Input.GetKey(KeyCode.W))
			MoveShip();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q) && Time.time > nextFire)
			playerShoot("vasen");

		if(Input.GetKeyDown(KeyCode.E) && Time.time > nextFire)
			playerShoot("oikea");
	}
	
	public void TurnShip(float direction)
	{
	// 	Turning for mobile
		rigidB.AddTorque((direction * -turnspeed), ForceMode2D.Force);
	//	rigidB.angularVelocity = turnHere * turnspeed;
	}
	
	void TurnShip()
	{
	// 	Turning for keyborad
		rigidB.AddTorque((horizontalInput * -turnspeed), ForceMode2D.Force);
	}
	
	public void MoveShip ()
	{
	// 	Forward movement
		float moveDirection = 1f;
		float angle = transform.rotation.z;
		float movementSpeed = speed * moveDirection;
		Vector2 movement = new Vector2 (angle,movementSpeed);	
		rigidB.AddRelativeForce(movement, ForceMode2D.Force);
	}

	public void playerShoot(string fireDirection)
	{
		nextFire = Time.time + fireRate;

		if (fireDirection == "vasen")
			Instantiate(shot, shotSpawnLeft.position, shotSpawnLeft.rotation);
		
		if (fireDirection == "oikea")
			Instantiate(shot, shotSpawnRight.position, shotSpawnRight.rotation);
	}
}
