using UnityEngine;
using System.Collections;

public class PickUpZone : MonoBehaviour 
{
	// Pickup zone for hook system, add this behind player with collider & sprite

	private SpriteRenderer TowSprite;
	private PlayerHook phook;

	void Start()
	{
		TowSprite = GetComponent<SpriteRenderer>();
		phook = GameObject.Find("Player").GetComponent<PlayerHook>();

		if(TowSprite == null)
			Debug.Log("Cannot find tow sprite Sprite on Player");
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if(other.gameObject.CompareTag("PickUp") && phook.grabbedObject == null)
			TowSprite.enabled = true;

		if(phook.grabbedObject != null)
			TowSprite.enabled = false;
	}

	void OnTriggerExit2D(Collider2D other) 
	{
		if(other.gameObject.CompareTag("PickUp"))
		{
			TowSprite.enabled = false;
		}
	}


}
