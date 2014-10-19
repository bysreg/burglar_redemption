using UnityEngine;
using System.Collections;

public class DebugText : MonoBehaviour {

	LastPlayerSighting lastPlayerSighting;
	GUIText lastSightingText;
	int guardCount;
	GuardAI[] guardAIs;

	void Awake()
	{
		lastSightingText = transform.Find("LastSightingText").GetComponent<GUIText>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
		guardCount = GameObject.Find("Guards").transform.childCount;
		GameObject guardParent = GameObject.Find("Guards");
		guardAIs = new GuardAI[guardCount];

		for(int i=0; i<guardCount; i++)
		{
			guardAIs[i] = guardParent.transform.GetChild(i).GetComponent<GuardAI>();
		}///gggggggggg
	}

	void Update()
	{
		lastSightingText.text = "Last Sighting : " + lastPlayerSighting.position;
	}

	void OnGUI()
	{
		for(int i=0; i<guardCount; i++)
		{
			GuardAI guard = guardAIs[i];

			GUI.Label(new Rect(20, i * 45 + 45, 100, 35), "Guard " + i + " : " + guard.GetGuardState());
		}
	}
}
