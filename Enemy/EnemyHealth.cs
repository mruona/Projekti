using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour 
{
	private Transform shipPos;
	Vector3 healthOrientation = new Vector3(0, 12f, 0f);

	// Health variables
	public float health;
	public float maxhealth = 250f;

	//	Health HUD components
	public Image enemyHPImage;
	public RectTransform hpCanvas;

	void Start ()
	{
		shipPos = transform.GetComponentInParent<Transform>();
		health = maxhealth;
		updateHealthHUD();
	}

	void LateUpdate () 
	{
		hpCanvas.position = (shipPos.position + healthOrientation);
	}

	//	Updates HUD
	void updateHealthHUD()
	{
		enemyHPImage.fillAmount = health / maxhealth;
	}

	//	For taking damage
	public void enemyTakeDamage (float damageAmmount)
	{
		health -= damageAmmount;

		if(health <= 0f)
		{
			enemyHPImage.fillAmount = 0f / maxhealth;
			Destroy(transform.parent.parent.gameObject);

			//TODO animation? end level?
		}
		else
			updateHealthHUD();
	}
}
