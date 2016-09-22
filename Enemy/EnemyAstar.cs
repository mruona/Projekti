using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]

public class EnemyAstar : MonoBehaviour 
{
//	Script for A* enemy

// Components
	private Seeker seeker;
	private Rigidbody2D rigid;

// Enemy target, path to follow, update rate for path
	public Transform target;
	public Path path;
	public float updateRate = 0.5f;
	
// Moments speed and force mode for rigidbody
	public float speed = 1000f;
	public ForceMode2D fmode;
	
//	[HideInInspector]
	public bool pathIsEnded = false;
	public float nextWaypointDistance = 3f;
	private int currentWaypoint = 0;
	
//	public float distanceToPlayer;

	void Start()
	{
		seeker = GetComponent<Seeker>();
		rigid = GetComponent<Rigidbody2D>();
		
		if(target == null) {
			Debug.Log("No player found");
			return;
		}
		
		// Starting path (Enemy location to target location)
		seeker.StartPath(transform.position, target.position, OnPathComplete);
		
		// Starting coroutine for Updatepath at start
		StartCoroutine (UpdatePath());
	}
	
	void FixedUpdate() 
	{		
		// If no path is available
		if(path == null) {
			return;
		}

		if(currentWaypoint >= path.vectorPath.Count) {
			return;
		}

		// Direction to next waypoint & speed added
		Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position ).normalized;
		dir *= speed * Time.fixedDeltaTime;

		// Move AI with direction
		rigid.AddForce(dir, fmode);
		transform.up = rigid.velocity.normalized;
		
		float dist = Vector2.Distance (transform.position, path.vectorPath[currentWaypoint]);
		if (dist < nextWaypointDistance) {
			currentWaypoint++;
		}

	}
	
// 	Updating path
	IEnumerator UpdatePath () 
	{
		if(target == null) {
			return false;
		}
		
		while(Vector2.Distance(transform.position, target.position) < 15f)
		{
			rigid.drag = 50f;
			yield return null;
		}
		rigid.drag = 0.2f;
		
		seeker.StartPath(transform.position, target.position, OnPathComplete);
		yield return new WaitForSeconds ( 1f / updateRate );

		StartCoroutine(UpdatePath());
	}
	
// 	When path is Complete
	public void OnPathComplete (Path p) 
	{
		//Debug.Log("We got path");
		if(!p.error) 
		{
			path = p;
			currentWaypoint = 0;
		}

		else 
		{
			Debug.Log(p.error);
		}

	}
}
