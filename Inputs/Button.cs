using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour 
{
//	Use this script for mobile buttons

	public Color defaultColour;
	public Color selectedColour;
	private Material mat;
	private InputControls inputControls;
	private PlayerHook playerHook;
	
	void Start() 
	{
		inputControls = GameObject.FindWithTag("Player").GetComponent<InputControls>();
		playerHook = GameObject.FindWithTag("Player").GetComponent<PlayerHook>();

		mat = GetComponent<Renderer>().material;
	}
	
	void LateUpdate()
	{
		if(Time.time > inputControls.nextFire)
			mat.color = defaultColour;
		else
			mat.color = selectedColour;
	}

	void OnTouchDown()
	{ 
		if(gameObject.tag == "Pick") 
		{			
			if(playerHook.grabbedObject == null)
				playerHook.TryHook(playerHook.GetHookedObject());
			else
				playerHook.DropObject();
		}
	}
	void OnTouchUp()
	{ }
	void OnTouchExit()
	{ }
	
	void OnTouchStay()
	{
// 		Mikäli tagilla olevaa nappia painetaan pohjassa -> suoritetaan inputcontrollien komentoja
		if(gameObject.tag == "Left")
			inputControls.TurnShip(-1f);
		
		if(gameObject.tag == "Right")
			inputControls.TurnShip(1f);
		
		if(gameObject.tag == "Gas") {
			inputControls.MoveShip();
		}
		
		if(gameObject.tag == "ShootLeft" && Time.time > inputControls.nextFire) {
			inputControls.playerShoot("vasen");
		}
		
		if(gameObject.tag == "ShootRight" && Time.time > inputControls.nextFire) {
			inputControls.playerShoot("oikea");
		}
		
		if(gameObject.tag == "Quit") {
			ExitGame();
		}
		
		if(gameObject.tag == "Restart") {
			reStart();
		}
		
	}

	void ExitGame ()
	{
		Application.Quit();
	}
	
	void reStart ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
