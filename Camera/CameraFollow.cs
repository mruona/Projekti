using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
//	Main camera
// 	Requires player with "Player" tag

	private Transform playerPos;
	Vector3 cameraOrientation = new Vector3(0, 0, -9f);
	public bool rotateCamera = true;
	
	void Start ()
	{
		playerPos = GameObject.FindWithTag("Player").transform;
	}

	void LateUpdate () 
	{
		transform.position = (playerPos.position + cameraOrientation);
		if(rotateCamera)
			transform.rotation = (playerPos.rotation);
	}	
}