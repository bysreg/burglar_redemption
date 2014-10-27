using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public bool simulateWithKeyboard;

	bool isGameOver;
	bool initialized;
	SceneFader sceneFader;
	GameObject caughtText;

	void Awake()
	{
		sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
		caughtText = GameObject.Find("CaughtText");
	}

	public void GameOver()
	{
		//fade the screen to black
		if(!isGameOver)
		{
			isGameOver = true;
			iTween.MoveBy(caughtText, iTween.Hash("y", -2, "easeType", "easeInOutExpo", "time", 0.5f));
			sceneFader.FadeOutScene(2, 
				() => {
					Application.LoadLevel(Application.loadedLevel);				
				}
			);
		}
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.G))
		{
			sceneFader.FadeOutScene(1, () => {Application.LoadLevel(Application.loadedLevel + 1);});
		}

		if(!initialized)
		{
			sceneFader.FadeInScene();
			initialized = true;
		}
	}
}
