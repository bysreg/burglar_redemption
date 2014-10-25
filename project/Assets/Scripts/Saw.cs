using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour 
{
	public float omega;
	public float A;
	public float B;
	private SpriteRenderer Image;
	private float theta;
	public float Phase;
	private Vector2 root;
	public static bool isSawing;
	public float BarHealth;
	public float DamageRate;

	ParticleSystem sparkPS;
	GameObject saw;
	GameObject sit;
	GameObject kneel;
	AudioSource sawAudio;
	bool initialized;
	SceneFader sceneFader;

	void Awake()
	{
		sparkPS = GameObject.Find("Spark").GetComponent<ParticleSystem>();
		saw = GameObject.FindGameObjectWithTag("Saw");
		sit = GameObject.FindGameObjectWithTag("Sit");
		kneel = GameObject.FindGameObjectWithTag("Kneel");
		sawAudio = saw.GetComponent<AudioSource>();
		sceneFader = GameObject.Find ("SceneFader").GetComponent<SceneFader>();
	}
	
	void Start () 
	{
		Image = gameObject.GetComponent <SpriteRenderer> ();
		theta = 0f;
		root = transform.position;
		isSawing = false;
		BarHealth = 100.0f;
		DamageRate = 10.0f;
		kneel.GetComponent<SpriteRenderer>().enabled = false;
		saw.GetComponent<SpriteRenderer>().enabled = false;

		sparkPS.enableEmission = false;
	}

	void Update () 
	{
		if(!initialized)
		{
			sceneFader.FadeInScene();
			initialized = true;
		}

		if(BarHealth <0)
		{
			Application.LoadLevel (Application.loadedLevel +1);
		}

		if(Input.GetKeyDown (KeyCode.Space))
		{
			isSawing = true;

			saw.GetComponent<SpriteRenderer>().enabled = true;
			sit.GetComponent<SpriteRenderer>().enabled = false;
			kneel.GetComponent<SpriteRenderer>().enabled = true;
			theta = Phase*Mathf.Deg2Rad;
			sawAudio.Play();

			sparkPS.enableEmission = true;
		}

		if(Input.GetKey (KeyCode.Space))
		{
			Move();
			BarHealth -= DamageRate * Time.deltaTime ;
		}

		if(Input.GetKeyUp (KeyCode.Space))
		{
			isSawing = false;
			saw.GetComponent<SpriteRenderer>().enabled = false;
			sit.GetComponent<SpriteRenderer>().enabled = true;
			kneel.GetComponent<SpriteRenderer>().enabled = false;
			sawAudio.Stop();

			sparkPS.enableEmission = false;
		}
	
	}

	void Move()
	{
		theta += omega * Time.deltaTime;
		saw.rigidbody2D.MovePosition (root + new Vector2(A * Mathf.Sin (theta), B * Mathf.Sin (theta)));
	}
}
