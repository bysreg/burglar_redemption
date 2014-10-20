using UnityEngine;
using System.Collections;

public class VolumeRolloff : MonoBehaviour 
{
	public GameObject Listener;
	public AudioSource Source;
	public float Dist;
	public float k;

	// Use this for initialization
	void Start () 
	{
		//Listener = GameObject.FindGameObjectsWithTag ("PsuedoListener");
		Source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Dist = Vector3.Magnitude(transform.position - Listener.transform.position);
		Source.volume = Mathf.Pow (0.95f, k * Dist*Dist);
	
	}
}
