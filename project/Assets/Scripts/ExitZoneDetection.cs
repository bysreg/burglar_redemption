using UnityEngine;
using System.Collections;

public class ExitZoneDetection : MonoBehaviour {

	public GameObject[] supriseGuards; 

	Player player;
	LastPlayerSighting lastPlayerSighting;
	SceneFader sceneFader;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
		sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			//player reaches the goal
			print ("goal reached");

			sceneFader.FadeOutScene(1, 
			                        () => {
					Application.LoadLevel(Application.loadedLevel + 1);				
				}
			);

			//now freeze the player, sound the alarm, and call the guards that are somehow near the exit zone
//			player.SetFreeze();
//			lastPlayerSighting.position = player.gameObject.transform.position;
//			foreach(var guard in supriseGuards)
//			{
//				guard.SetActive(true);
//			}
		}
	}

}
