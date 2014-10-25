using UnityEngine;
using System.Collections;

public class SawWithMakeyMakey : MonoBehaviour {

	float barHealth = 100f;
	float damageRate = 10f;
	SpriteRenderer kneel;
	SpriteRenderer sit;
	SpriteRenderer saw;
	AudioSource sawingSound;
	bool isSawing;
	float time;

	void Awake()
	{
		kneel = GameObject.FindGameObjectWithTag("Kneel").GetComponent<SpriteRenderer>();
		sit = GameObject.FindGameObjectWithTag("Sit").GetComponent<SpriteRenderer>();
		saw = GameObject.FindGameObjectWithTag("Saw").GetComponent<SpriteRenderer>();
		sawingSound = GameObject.FindGameObjectWithTag("Kneel").GetComponent<AudioSource>();
	}

	void Start()
	{
		kneel.enabled = false;
		sit.enabled = true;
	}

	void Update()
	{
		if(barHealth <0)
		{
			Application.LoadLevel (Application.loadedLevel +1);
		}

		if(Input.GetKey(KeyCode.S))
		{
			StartSawing();
		}


	}

	void StartSawing()
	{

		isSawing = true;
	}


	void StopSawing()
	{

		isSawing = false;
	}
}
