using UnityEngine;
using System.Collections;

public class Parenting : MonoBehaviour {

	public static string target;

	// Use this for initialization
	void Start () 
	{
		target="Dummy";
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnTriggerStay2D (Collider2D other)
	{
		if(target == gameObject.tag )
		{
			if(other.gameObject.tag=="Leaf")
			{
				other.transform.parent = null;
				other.transform.parent = gameObject.transform ;
			}
		}

	}
}
