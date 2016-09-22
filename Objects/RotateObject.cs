using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour 
{
//	Simple rotation to object

	public float spinSpeed;

	void Update () 
	{
		transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
	}
}
