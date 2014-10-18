using UnityEngine;
using System.Collections;

public class ExitZoneDetection : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			//player reaches the goal
			print ("goal reached");
		}
	}

}
