using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelTime : MonoBehaviour 
{
//	Time to complete level, remaining time
	public int levelTime;
	public int timeLeft;

//	Components
	public Text timerText;
	private GameManager gameManager;

	void Start () 
	{
		gameManager = gameObject.GetComponent<GameManager>();
		timeLeft = levelTime;
		StartCoroutine ("Countdown", levelTime);
	}

//	Counter for time, updates HUD, If time ticks to zero -> player loses
	private IEnumerator Countdown(int time)
	{
		while(time > 0)
		{
			timerText.text = "Time: " +time;
			yield return new WaitForSeconds(1);
			timeLeft--;
			time--;
		}
		timerText.text = "Time: " +time;
		gameManager.lost_Time();
	}
}
