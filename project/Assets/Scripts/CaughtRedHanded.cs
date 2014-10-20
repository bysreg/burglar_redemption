using UnityEngine;
using System.Collections;

public class CaughtRedHanded : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(Saw.isSawing)
		{
			print ("Caught");
			Application.LoadLevel(Application.loadedLevel);
		}
	}

}
