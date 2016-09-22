using UnityEngine;
using System.Collections;

public class RotationTurret : MonoBehaviour 
{
//	Rotates turret and then shoots cannonball with delay

	[Range(5, 270)] public int rotationAngle = 45;
	[Range(0f, 5f)] public float waitTime = 2f;
	private int spins;

	public Transform[] shotSpawn;
	public GameObject cannonBall;

	void Start() 
	{
		StartCoroutine(RotateAndShoot(waitTime));
		spins = rotationAngle;
	}

	IEnumerator RotateAndShoot(float waitTime) 
	{
		Shoot();
		yield return new WaitForSeconds(waitTime);

		while(spins > 0)
		{
			transform.Rotate(Vector3.forward, -2f);
			yield return new WaitForSeconds (0.01f);
			spins--;
		}

		spins = rotationAngle;
		StartCoroutine(RotateAndShoot(waitTime));
	}

	void Shoot()
	{
		// Shoot here
		foreach(Transform spawns in shotSpawn)
		{
			Instantiate(cannonBall, spawns.position, spawns.rotation);
		}

	}
}
