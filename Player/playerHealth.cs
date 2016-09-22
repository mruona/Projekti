using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour 
{
//	Add this script for player

// Health variables
	private float health;
	public float maxhealth = 100f;

//	Health HUD components
	public Image healthBarImage;
	public Text healthText;

//	Total damage taken for stats
	public float totalDamage = 0f;

// Immune to damage, Sprite indicator
	public bool isImmune = false;
	public SpriteRenderer boostSprite;

//	GameManager component
	private GameManager gameManager;

	void Start () 
	{
		gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

		health = maxhealth;
		updateHealthHUD();
	}

//	Updates HUD, return nearest integer
	void updateHealthHUD()
	{
		healthBarImage.fillAmount = health / maxhealth;
		healthText.text = "Health: "+Mathf.Round(health);
	}

//	For taking damage, returns if immune to damage, checks if player has health, calls update for HUD, calculates total damage for stats
	public void takeDamage (float damageAmmount)
	{
		if(isImmune)
			return;

		totalDamage += damageAmmount;
		health -= damageAmmount;

		if(health <= 0f)
		{
			healthText.text = "Health: 0";
			healthBarImage.fillAmount = 0f / maxhealth;
			gameManager.lost_Damage();
		}
		else
			updateHealthHUD();
	}

//	For healing, player health is capped to max health
	public void healDamage (float healthAmmount)
	{
		health += healthAmmount;
		health = Mathf.Min(health, maxhealth);
		updateHealthHUD();
	}

//	During this player is invulnerable, enables sprite to indicate
	public IEnumerator Invulnerable(float time) 
	{
		isImmune = true;
		boostSprite.enabled = true;
		
		yield return new WaitForSeconds(time);
		
		boostSprite.enabled = false;
		isImmune = false;
	}
}
