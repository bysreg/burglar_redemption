using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour 
{
	public float omega;
	public float A;
	public float B;
	private SpriteRenderer Image;
	private float theta;
	private Vector2 root;
	public static bool isSawing;

	// Use this for initialization
	void Start () 
	{
		Image = gameObject.GetComponent <SpriteRenderer> ();
		Image.enabled = false;
		theta = 0f;
		root = transform.position;
		isSawing = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Space))
		{
			Image.enabled=true;
			isSawing = true;
		}

		if(Input.GetKey (KeyCode.Space))
		{
			Move();
		}

		if(Input.GetKeyUp (KeyCode.Space))
		{
			Image.enabled=false;
			isSawing = false;
		}
	
	}

	void Move()
	{
		theta += omega * Time.deltaTime;
		rigidbody2D.MovePosition (root + new Vector2(A * Mathf.Sin (theta), B * Mathf.Sin (theta)));
	}
}
