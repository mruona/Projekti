using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour 
{
// 	Treasure spawner
//	Add this to empty GameObject

	public GameObject managerObject;
	public GameManager gameManagerScript;
	
	void Start()
	{
		managerObject = GameObject.FindGameObjectWithTag("GameManager");
		gameManagerScript = managerObject.GetComponent<GameManager>();

		if (gameManagerScript == null)
			Debug.Log ("Cannot find 'GameManager' script");
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{		
		if(other.gameObject.tag == "Player")
		{
			gameManagerScript.gemList.Remove(this.gameObject);
			Destroy(gameObject, 0f);
			gameManagerScript.UpdateGems();	
		}
	}
}
