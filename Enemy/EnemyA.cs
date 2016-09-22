using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyA : MonoBehaviour 
{
//	Enemy script with CharacterController & A*
	CharacterController characterController;

	public enum enTurnType
	{
		PathMode,
		PlayerRotationStep,
		LineOfSightMode,
		NeedsTurning,
	}
	public string mode;

	private Transform playerLocation;
	private Transform currentTarget;
	[Header("Targets:")]
	public Transform targetL;
	public Transform targetR;
	private float LeftRange;
	private float RightRange;
	public float distanceToPlayer;

	Seeker seeker;
	Path path;
	[Header("Path settings")]
	public float updateRate;
	public float visualRate;
	private int currentWaypoint;
	private float maxWpDistance = 1f;

	[Header("Variables")]
	public float movementSpeed = 10f;
	private float currentSpeed;
	public float turnSpeed = 20f;
	public float pathTurnSpeed = 1f;

	private bool spottedLeft = false;
	private bool spottedRight = false;
	[Header("LOS")]
	public bool LineOfSight = false;
	public LayerMask spottingLayer;
	public Transform spot1;
	public Transform spot2;

	[HideInInspector]
	public float nextFire;
	[Header("Shoot settings:")]
	public float fireRate;
	public GameObject EnemyShot;
	public Transform shot1;
	public Transform shot2;
	[Header("Ramming mode:")]
	public bool ramming = false;
	[Header("Smoke particles:")]
	public ParticleSystem particle1;
	public ParticleSystem particle2;
	public ParticleSystem particle3;

	void Start () 
	{
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		playerLocation = GameObject.FindWithTag("Player").GetComponent<Transform>();

		currentTarget = targetR;
		StartCoroutine (UpdatePath());
		StartCoroutine (CheckPlayer());
	}

	void FixedUpdate() 
	{
//		Debug.DrawLine(transform.position, spot1.position, Color.magenta);
//		Debug.DrawLine(transform.position, spot3.position, Color.magenta);

	//	If path not found
		if(path == null) {
			return;
		}

	//	If this final waypoint?
		if(currentWaypoint >= path.vectorPath.Count) 
		{
			if(ramming && !LineOfSight)
			{
				ramming = false;
				CheckVisual();
				StartCoroutine (UpdatePath());
			}
			return;
		}

	//	Enemy rotation to waypoint / player
		if(distanceToPlayer > 25f && !LineOfSight)
			followPath();
		else if(!ramming)
			RotateShip();
		
		if(LineOfSight && !ramming)
			currentSpeed = 0f;
		else if(ramming)
			currentSpeed = 20f;
		else
			currentSpeed = movementSpeed;

	//	Enemy movement
		Vector3 movement = (path.vectorPath[currentWaypoint] - transform.position).normalized * currentSpeed * Time.fixedDeltaTime;
		characterController.Move(movement);

	//	When close enough current waypoint -> jump to next one
		if(Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < maxWpDistance) {
			currentWaypoint++;
		}

	}

	void RotateShip()
	{
		if(LineOfSight)
		{
			mode = enTurnType.LineOfSightMode.ToString();
			Quaternion angle = Quaternion.LookRotation(playerLocation.position - transform.position, transform.TransformDirection(Vector3.up));
			transform.rotation = new Quaternion(0, 0, angle.z, angle.w);
		}

		else
		{
			mode = enTurnType.PlayerRotationStep.ToString();
			float step = turnSpeed * Time.fixedDeltaTime;
			float angle = Vector3.Angle(playerLocation.up , transform.position - playerLocation.position);
			if(angle < 90f)
				transform.rotation = Quaternion.RotateTowards(transform.rotation, playerLocation.rotation, -step);
			else
				transform.rotation = Quaternion.RotateTowards(transform.rotation, playerLocation.rotation, step);
		}
	}

	void followPath()
	{
		mode = enTurnType.PathMode.ToString();

		Quaternion rotation = Quaternion.LookRotation
			(path.vectorPath[currentWaypoint] - transform.position, transform.TransformDirection(Vector3.back));
		
		if(rotation == new Quaternion(0, 0, 0, 0))
			return;

		else
		{
			Quaternion filterRotation = new Quaternion(0f, 0f, rotation.z, rotation.w);
			transform.rotation = Quaternion.Lerp(transform.rotation, filterRotation, pathTurnSpeed * Time.deltaTime);
		}
	}

	IEnumerator CheckPlayer() 
	{
		distanceToPlayer = Vector2.Distance(transform.position, playerLocation.position);
		CheckVisual();
		yield return new WaitForSeconds(1f / visualRate);
		StartCoroutine(CheckPlayer());
	}
		
	IEnumerator UpdatePath () 
	{	
		if(ramming)
			yield break;
		
	//	Stop updating when close
		while(Vector2.Distance(transform.position, currentTarget.position) < 2f) 
		{
			yield return null;
		}

	//	Check wich is closer and assing target
		LeftRange = Vector2.Distance(transform.position, targetL.transform.position);	
		RightRange = Vector2.Distance(transform.position, targetR.transform.position);

		if(LeftRange < RightRange)
			currentTarget = targetL;
		else
			currentTarget = targetR;
		
	//	Update target location, wait and start Coroutine again
		seeker.StartPath(transform.position, currentTarget.position, OnPathComplete);
		yield return new WaitForSeconds ( 1f / updateRate );
		StartCoroutine(UpdatePath());
	}

	// When Path is complete
	void OnPathComplete(Path p)
	{
		if(!p.error) {
			path = p;
			currentWaypoint = 0;
		}
		
		else {
			Debug.Log(p.error);
		}
	}

	void CheckVisual()
	{
		spottedLeft = (Physics2D.Linecast(transform.position, spot1.position, spottingLayer));
		spottedRight = (Physics2D.Linecast(transform.position, spot2.position, spottingLayer));

		if(spottedLeft || spottedRight)
			LineOfSight = true;
		else
			LineOfSight = false;

		if(Time.time > nextFire && spottedLeft)
			ShootPlayer("left");

		if(Time.time > nextFire && spottedRight)
			ShootPlayer("right");

		if(!ramming)
			CheckRamming();
	}

	void CheckRamming()
	{
		float angle2 = Vector3.Angle(playerLocation.up, transform.position - playerLocation.position);
		if(angle2 <120f && angle2 > 60f && distanceToPlayer < 60f && distanceToPlayer > 40f && !LineOfSight)
		{
			if(!ramming)
			{
				Vector3 offset = transform.up * 6f + playerLocation.up * 5f;
				Vector3 ramTarget = playerLocation.position + offset;
				seeker.StartPath(transform.position, ramTarget, OnPathComplete);
				ramming = true;
				StopCoroutine("UpdatePath");
			}
		}
	}
		
	void ShootPlayer(string FireDirection)
	{
		nextFire = Time.time + fireRate;
		int change = Random.Range(1, 11);

		if (FireDirection == "left")
		{
			int pos = Random.Range(-4, 4);
			Vector3 dir = shot1.position + shot1.right * pos;

			if(change >= 9)
				StartCoroutine(RapidFire(shot1.position, shot1.right, shot1.rotation));
			else
			{
				particle1.transform.position = dir;
				Instantiate(EnemyShot, dir, shot1.rotation);
				particle1.Emit(20);
			}
		}

		if (FireDirection == "right")
		{
			int pos = Random.Range(-4, 4);
			Vector3 dir = shot2.position + shot2.right * pos;

			if(change >= 1)
				StartCoroutine(RapidFire(shot2.position, shot2.right, shot2.rotation));
			else
			{
				particle1.transform.position = dir;
				Instantiate(EnemyShot, dir, shot2.rotation);
				particle1.Emit(20);
			}
		}
	}

	IEnumerator RapidFire(Vector3 location, Vector3 direction, Quaternion rot) 
	{			
		Vector3 offSet = location - direction * 4f;
		particle1.transform.position = offSet;
		particle1.Emit(30);
		Instantiate(EnemyShot, offSet, rot);

		yield return new WaitForSeconds(0.3f);

		particle2.transform.position = location;
		particle2.Emit(30);
		Instantiate(EnemyShot, location, rot);

		yield return new WaitForSeconds(0.3f);

		offSet = location + direction * 4f;
		particle3.transform.position = offSet;
		particle3.Emit(30);
		Instantiate(EnemyShot, offSet, rot);
	}
}
