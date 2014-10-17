using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public bool simulateWithKeyboard;

	private bool isGameOver;

	public void GameOver()
	{
		//fade the screen to black
		isGameOver = false;
	}
}
