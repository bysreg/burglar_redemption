using UnityEngine;
using System.Collections;

public class CaughtRedHanded : MonoBehaviour {

	bool isGameOver;
	SceneFader sceneFader;
	GameOver gameOver;

	void Awake()
	{
		sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
		gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(Saw.isSawing)
		{
			GameOver();
		}
	}

	void GameOver()
	{
		gameOver.ShowGameOver();
//		if(!isGameOver)
//		{
//			isGameOver = true;
//			sceneFader.FadeOutScene(GameOverCallback);
//		}
	}

	void GameOverCallback()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

}
