using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
//	Script for GameManager

//	Variables for gems and hud
	public Text gemsText;
	private int gemsLeft = 0;
	private GameObject[] gemArray;
	public List<GameObject> gemList = new List<GameObject>();

//	End game stat screen
	public GameObject endCanvas;
	public Text end_resultTXT;
	public Text end_InfoTXT;

//	Player health for stats
	private playerHealth PH;
	private LevelTime LT;

//	Reset timeScale, check gems, GetComponents
	void Start()
	{
		Time.timeScale = 1f;
		Invoke("CheckGems", 0.2f);

		PH = GameObject.FindWithTag("Player").GetComponent<playerHealth>();
		LT = GetComponent<LevelTime>();
	}

//	For testing purposes
//	void Update()
//	{
//		if(Input.GetKeyDown(KeyCode.H))
//		   won_Level();
//		if(Input.GetKeyDown(KeyCode.J))
//			lost_Time();
//		if(Input.GetKeyDown(KeyCode.K))
//			lost_Damage();
//	}
	
//	Check map for gems
	private void CheckGems()
	{
		gemArray = GameObject.FindGameObjectsWithTag("Gems");
		
		foreach (GameObject gem in gemArray)
		{
			gemList.Add(gem);
		}
		UpdateGems();
	}
	
//	Updates HUD and list, If gems collected -> player wins
	public void UpdateGems()
	{
		gemsLeft = gemList.Count;
		gemsText.text = "Gems left: "+gemsLeft;
		
		if(gemsLeft <= 0)
			won_Level();
	}

//	Game won due all gems collected
	public void won_Level()
	{
		Time.timeScale = 0f;
		endCanvas.SetActive(true);
		end_resultTXT.text = "Level complete!";
		end_InfoTXT.text = "Total damage: \t\t" +(int)PH.totalDamage+ "\nTime remaining: \t" +LT.timeLeft;
	}

// 	 Game lost due time ran out
	public void lost_Time()
	{
		Time.timeScale = 0f;
		endCanvas.SetActive(true);
		end_resultTXT.text = "You are out of time!";
		end_InfoTXT.text = "Total damage: \t\t" +(int)PH.totalDamage+ "\nGems remaining: \t" +gemsLeft;
	}

//	 Game lost due damage taken
	public void lost_Damage()
	{
		Time.timeScale = 0f;
		endCanvas.SetActive(true);
		end_resultTXT.text = "Your ship has been wrecked!";
		end_InfoTXT.text = "Total damage: \t\t" +(int)PH.totalDamage+ "\nTime remaining: \t" +LT.timeLeft;
	}
}
