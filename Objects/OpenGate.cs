using UnityEngine;
using System.Collections;

public class OpenGate : MonoBehaviour 
{
//	Add this to trigger zone
// 	Plays gate animation

	public string triggerName;
	public string gateName;

	SpriteRenderer sprite;
	public Color color;
	Animator animator;

	void Start () 
	{
		animator = GameObject.Find(gateName).GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}

	void OnTriggerStay2D(Collider2D other) 
	{
		if(other.gameObject.name == triggerName)
		{
			animator.SetBool("OpenGate", true);
			sprite.color = color;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.name == triggerName)
		{
			animator.SetBool("OpenGate", false);
			sprite.color = Color.red;
		}
	}

}
