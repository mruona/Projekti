using UnityEngine;
using System.Collections;

public class PlayerHook : MonoBehaviour 
{
	// Player hook for Level3

	private Rigidbody2D grabRB;
	private Transform endPos;

	public GameObject grabbedObject;
	public LayerMask lineCastMask;

	private HingeJoint2D hinge;
	private SpriteRenderer TowSprite;
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			if(grabbedObject == null)
				TryHook(GetHookedObject());
			else
				DropObject();
		}
	}

	void Start()
	{
		endPos = GameObject.Find("endPos").GetComponent<Transform>();
		hinge = GetComponent<HingeJoint2D>();
	}

//	Checks if pick up object is behind player, returns gameobject or null
	public GameObject GetHookedObject()
	{
		Vector2 target = new Vector2(endPos.position.x, endPos.position.y);
		RaycastHit2D hit2D = Physics2D.Linecast(transform.position, target, lineCastMask);		
		if(hit2D.collider != null && hit2D.collider.CompareTag("PickUp"))
			return hit2D.rigidbody.gameObject;
		else
			return null;
	}
	
	public void TryHook(GameObject objectToGrab)
	{
		if (objectToGrab == null)
			return;

		else
		{
			grabbedObject = objectToGrab;
			grabRB = objectToGrab.GetComponent<Rigidbody2D>();
			hinge.enabled = true;
			hinge.connectedBody = grabRB;
		}
	}
	
// 	To drop hooked object
	public void DropObject()
	{
		if (grabbedObject == null)
			return;

		else
		{
			hinge.enabled = false;
			hinge.connectedBody = null;
			grabRB.velocity = Vector2.zero;
			grabbedObject = null;
		}
	}
}
