using UnityEngine;
using System.Collections;

public class SpawnTreasure : MonoBehaviour 
{
//	Add this to GameObject to spawn treasure
//	Spawn GameObjects randomly inside circle radius

	public GameObject treasureObject;
	public float radius = 35f;
	public int numberOfItems = 1;
	
	void Start () 
	{
		for (int i = 1; i <= numberOfItems; i++)
		{
			Vector2 randomi = new Vector2(	Random.insideUnitCircle.x * radius+ transform.position.x, 
			                            	Random.insideUnitCircle.y * radius+ transform.position.y);
			
			Instantiate (treasureObject, randomi, Quaternion.identity);
		}
	}

}
