using UnityEngine;
using System.Collections;

public class CaughtRedHanded : MonoBehaviour {

	bool isGameOver;
	SceneFader sceneFader;
	GameOver gameOver;
	Saw saw;

	void Awake()
	{
		sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
		gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
		saw = GameObject.Find ("saw").GetComponent<Saw> ();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(saw.IsSawing())
		{
			GameOver();
		}
	}

	void GameOver()
	{
		gameOver.ShowGameOver();
	}
}
