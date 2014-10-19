using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public bool simulateWithKeyboard;

	bool isGameOver;
	bool initialized;
	SceneFader sceneFader;

	void Awake()
	{
		sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
	}

	public void GameOver()
	{
		//fade the screen to black
		isGameOver = false;
		sceneFader.FadeInScene();
	}
	
	void Update()
	{
		if(!initialized)
		{
			sceneFader.FadeOutScene();

			initialized = true;
		}
	}
}
