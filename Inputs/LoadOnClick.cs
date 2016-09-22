using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour 
{
	// Loading scenes & quiting

//	public GameObject loadingImage;

	public void LoadScene (int level) 
	{
//		Application.LoadLevel(level);
		SceneManager.LoadScene(level);
	}
	
	public void ExitGame ()
	{
		Application.Quit();
	}

	public void RestartLevel()
	{
//		Application.LoadLevel(Application.loadedLevelName);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}