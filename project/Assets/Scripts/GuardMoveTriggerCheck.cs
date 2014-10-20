using UnityEngine;
using System.Collections;

public class GuardMoveTriggerCheck : MonoBehaviour {

	GuardAI[] guards;

	void Awake()
	{
		GameObject guardsLayer = GameObject.Find("Guards");
		int guardCount = guardsLayer.transform.childCount;
		guards = new GuardAI[guardCount];

		for(int i=0; i<guardCount; i++)
		{
			guards[i] = guardsLayer.transform.GetChild(i).GetComponent<GuardAI>();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Player")
			return;

		//let all the guards come out. mwahahahah
		GuardAI.guardActive = true;
	}

		
}
