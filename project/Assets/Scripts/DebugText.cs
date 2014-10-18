using UnityEngine;
using System.Collections;

public class DebugText : MonoBehaviour {

	LastPlayerSighting lastPlayerSighting;
	GUIText lastSightingText;
	int guardCount;

	void Awake()
	{
		lastSightingText = transform.Find("LastSightingText").GetComponent<GUIText>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
		guardCount = GameObject.Find("Guards").transform.childCount;
	}

	void Update()
	{
		lastSightingText.text = "Last Sighting : " + lastPlayerSighting.position;
	}

	void OnGUI()
	{
		for(int i=0; i<guardCount; i++)
		{
			GUI.Label(new Rect(20, i * 45 + 45, 100, 35), "Guard " + i + " : ");
		}
	}
}
