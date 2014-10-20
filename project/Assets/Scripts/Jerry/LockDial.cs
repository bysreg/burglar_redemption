using UnityEngine;
using System.Collections;

public class LockDial : MonoBehaviour {

	private float Rotation;
	private Rigidbody2D TheDial;
	public float K;
	int num0, num1;

	// Use this for initialization
	void Start () 
	{
		Rotation = 0.0f;
		TheDial = GetComponent <Rigidbody2D> ();


	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateRotation ();
		TheDial.MoveRotation (Rotation);
		num1 = AngleToNumber ();
	}
	

	public int AngleToNumber()
	{
		int number = Mathf .FloorToInt(Rotation / 3.6f);
		return number;
	}

	void UpdateRotation()
	{
		Rotation += K*(Input.GetAxis ("Mouse ScrollWheel"));

		if(Rotation >= 360)
		{
			Rotation -= 360;
		}

		if(Rotation < 0)
		{
			Rotation += 360;
		}
	}
}
