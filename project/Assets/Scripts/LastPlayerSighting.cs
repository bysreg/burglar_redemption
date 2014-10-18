using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour {

	public Vector3 position = new Vector3(1000f, 1000f, 1000f);
	public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);

	AudioSource alarm;

	void Awake()
	{
		alarm = transform.Find("AlarmSound").GetComponent<AudioSource>();
	}

	void Update()
	{
		if (position != resetPosition && !alarm.isPlaying)
		{
			//sound the alarm
			alarm.Play();
		}
		else if(position == resetPosition && alarm.isPlaying)
		{
			alarm.Stop();
		}
	}
}
