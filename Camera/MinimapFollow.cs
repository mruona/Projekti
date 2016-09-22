using UnityEngine;
using System.Collections;

public class MinimapFollow : MonoBehaviour 
{
//	Minimap camera script
	
	private Transform cameraPos;
	Vector3 cameraOrientation = new Vector3(0, 0, -8f);
	
	void Start ()
	{
		cameraPos = GameObject.FindWithTag("MainCamera").transform;
	}
	
	void LateUpdate () 
	{
		transform.position = (cameraPos.position + cameraOrientation);
	}	
}
