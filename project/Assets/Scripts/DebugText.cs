using UnityEngine;
using System.Collections;

public class DebugText : MonoBehaviour {

	LastPlayerSighting lastPlayerSighting;
	GUIText lastSightingText;

	void Awake()
	{
		lastSightingText = transform.Find("LastSightingText").GetComponent<GUIText>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
	}

	void Update()
	{
		lastSightingText.text = "Last Sighting : " + lastPlayerSighting.position;
	}


}
