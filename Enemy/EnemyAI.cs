using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

//	http://www.attiliocarotenuto.com/articles-tutorials/unity/83-articles-tutorials/unity/292-unity-3-moving-a-npc-along-a-path
//	Basic AI with waypoints
//	Patrol, Chasing, Shooting
	
	public enum EnemyState {Patrol, Chasing, Shooting, Dead};
	public EnemyState enemyState;
	
	Animator animatori;
	private AudioSource hitAudio;
	public AudioSource explosionAudio;
	
	public Transform[] wayPoints;
	private int currentIndex;
	private Transform currentWayPoint;
	
	private GameObject PlayerTargetL;
	private GameObject PlayerTargetR;
	private GameObject playerPosition;
	
	public Transform EnemyShotLeft;
	public Transform EnemyShotRight;
	
	public GameObject shot;
	
	public float RightRange;
	public float LeftRange;
	public float DistanceBetween;
	public string EnemyshotDirection;
	
	public float moveSpeed = 5f;
	public int Health = 10;
	
	private float EnemyNextFire;
	public float EnemyFireRate = 2f;
	private float minDistance = 20f;
	
	private Vector3 playerDirection;
	public bool canShoot;
	
	void Start()
	{
		canShoot = false;
		PlayerTargetL = GameObject.FindGameObjectWithTag ("PlayerTargetL");
		PlayerTargetR = GameObject.FindGameObjectWithTag ("PlayerTargetR");
		playerPosition = GameObject.FindGameObjectWithTag ("Player");
		hitAudio = GameObject.FindWithTag("Enemy").GetComponent<AudioSource>();
		animatori = GetComponentInChildren<Animator>();
		
		currentWayPoint = wayPoints[0];
		currentIndex = 0;
		
		enemyState = EnemyState.Patrol;	
	}
	
	void Update ()
	{
		// Etäisyys pelaajaan
		DistanceBetween = Vector2.Distance(transform.position, playerPosition.transform.position);
		
		// Etäisyys ampumispaikkaan
		LeftRange = Vector2.Distance(transform.position, PlayerTargetL.transform.position);		
		RightRange = Vector2.Distance(transform.position, PlayerTargetR.transform.position);
		
		if (DistanceBetween < 60f)
		{
			enemyState = EnemyState.Chasing;
		}
		
		if (DistanceBetween > 60f)
		{
			enemyState = EnemyState.Patrol;
		}
		
		if (DistanceBetween < 27f && canShoot == true)
		{
			enemyState = EnemyState.Shooting;
		}
		
		
		switch (enemyState)
		{
			case EnemyState.Patrol:
				canShoot = false;
				MoveTowardWayPoint();
			
				if(Vector3.Distance(currentWayPoint.transform.position, transform.position) < minDistance)
				{
					++currentIndex;
				if(currentIndex > wayPoints.Length -1)
				{
					currentIndex = 0;
				}
				currentWayPoint = wayPoints[currentIndex];
				}
				break;
				
				
			case EnemyState.Chasing:
				canShoot = false;
				
				if(LeftRange < RightRange)
				{
					MoveTowardPlayer("Left");
					
					if (LeftRange < 17f) 
					{
						enemyState = EnemyState.Shooting;
						canShoot = true;
						EnemyshotDirection = "right";

					}
					else { canShoot = false; }
				}
				
				if(RightRange < LeftRange)
				{
					MoveTowardPlayer("Right");
					
					if (RightRange < 17f) 
					{
						enemyState = EnemyState.Shooting;
					 	canShoot = true;
					 	EnemyshotDirection = "left";
					}
					else { canShoot = false;}
				}


				break;
			
			
			case EnemyState.Shooting:
			
				RotateTowardPlayer();
				
				if (RightRange < 17f || LeftRange < 17f && canShoot == true)
				{
					if (Time.time > EnemyNextFire)
						ShootPlayer();
				}
				else
				{
					enemyState = EnemyState.Chasing;
					canShoot = false;
				}
				

				break;
				
			case EnemyState.Dead:
				break;
		}
	
	}

	void MoveTowardWayPoint ()
	{
		Vector3 direction = currentWayPoint.transform.position - transform.position;
		Vector3 moveVector = direction.normalized * moveSpeed * Time.deltaTime;
		transform.position += moveVector;
		
		float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) -90;
		Quaternion quaterni = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Lerp(transform.rotation, quaterni, 1f * Time.deltaTime);
		//transform.rotation = Quaternion.Slerp(transform.rotation, quaterni, 1.5f * Time.deltaTime);
	}
	
	void MoveTowardPlayer (string direction)
	{
	
		if (direction == "Left")
		{
			playerDirection = PlayerTargetL.transform.position - transform.position;
		}
		
		if (direction == "Right")
		{
			playerDirection = PlayerTargetR.transform.position - transform.position;
		}
		
		Vector3 moveVector = playerDirection.normalized * moveSpeed * Time.deltaTime;
		transform.position += moveVector;
		
		float angle = (Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg) -90;
		Quaternion quaterni = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Lerp(transform.rotation, quaterni, 1.5f * Time.deltaTime);
	}
	
	void RotateTowardPlayer ()
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, playerPosition.transform.rotation, 1f * Time.deltaTime);
	}
	
	void ShootPlayer ()
	{
		EnemyNextFire = Time.time + EnemyFireRate;
		
		if (EnemyshotDirection == "left")
		{
			Instantiate(shot, EnemyShotLeft.position, EnemyShotLeft.rotation);
		}
		
		if (EnemyshotDirection == "right")
		{
			Instantiate(shot, EnemyShotRight.position, EnemyShotRight.rotation);
		}
	}
	
	
	void OnCollisionEnter2D(Collision2D collision)
	{	
	
		if(collision.collider.CompareTag("Cannonball"))
		{
			Health--;
			hitAudio.Play();
			
			if(Health < 0)
			{
				explosionAudio.Play();	
				animatori.SetBool("Dead", true);
				GetComponent<Collider2D>().enabled = false;	
				Destroy(gameObject, 2.3f);
				enemyState = EnemyState.Dead;
			}
		}
		
		if(collision.collider.CompareTag("Player"))
		{
			Debug.Log("Hit player\t");
			
		}
	}

}
