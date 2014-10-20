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

	// Use this for initialization
	void Start () 
	{
		Image = gameObject.GetComponent <SpriteRenderer> ();
		theta = 0f;
		root = transform.position;
		isSawing = false;
		BarHealth = 100.0f;
		DamageRate = 10.0f;

		if((gameObject.tag=="Kneel")||(gameObject.tag=="Saw"))
		{
			Image.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(BarHealth <0)
		{
			Application.LoadLevel (Application.loadedLevel +1);
		}

		if(Input.GetKeyDown (KeyCode.Space))
		{
			if(gameObject.tag=="Saw")
			{
				Image.enabled=true;
				isSawing = true;

			}

			if(gameObject.tag=="Sit")
			{
				Image.enabled=false;
			}

			if(gameObject.tag=="Kneel")
			{
				Image.enabled=true;
				theta=Phase*Mathf.Deg2Rad;
				audio.Play();
			}
		}

		if(Input.GetKey (KeyCode.Space))
		{
			if(gameObject.tag=="Saw")
			{
				Move();
				BarHealth -= DamageRate * Time.deltaTime ;
			}
		}

		if(Input.GetKeyUp (KeyCode.Space))
		{
			if(gameObject.tag=="Saw")
			{
				Image.enabled=false;
				isSawing = false;
			}

			if(gameObject.tag=="Sit")
			{
				Image.enabled=true;
			}

			if(gameObject.tag=="Kneel")
			{
				Image.enabled=false;
				audio.Stop();
			}
		}
	
	}

	void Move()
	{
		theta += omega * Time.deltaTime;
		rigidbody2D.MovePosition (root + new Vector2(A * Mathf.Sin (theta), B * Mathf.Sin (theta)));
	}
}
