using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour 
{
	public float CurrentAngle;
	public float TargetAngle;
	float Omega;
	bool RotationLock=false;

	// Use this for initialization
	void Start () 
	{
		Omega = 60;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CurrentAngle = rigidbody2D.rotation ;
		if(Input.GetKeyDown(KeyCode.W))
		{
			if(RotationLock ==false)
			{
				RotationLock = true;
				CurrentAngle = rigidbody2D.rotation ;
				TargetAngle = CurrentAngle + 120f;
				TargetAngle = Normalize (TargetAngle);
				Omega = 60;
			}

		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			if(RotationLock ==false)
			{
				RotationLock = true;
				CurrentAngle = rigidbody2D.rotation ;
				TargetAngle = CurrentAngle - 120f;
				TargetAngle = Normalize (TargetAngle);
				Omega = -60;
			}
			
		}


		StartCoroutine (RotateThing ());
	
	}

	IEnumerator RotateThing()
	{
		rigidbody2D.rotation = Normalize (rigidbody2D.rotation);

		if(RotationLock == true)
		{
			rigidbody2D.MoveRotation(rigidbody2D.rotation + Time.deltaTime * Omega);
			yield return new WaitForSeconds (Time.deltaTime);
		}

		if(Mathf.Abs(rigidbody2D.rotation - TargetAngle ) < Mathf.Abs(2*Omega*Time.deltaTime) )
		{
			RotationLock =false;
			rigidbody2D.MoveRotation(TargetAngle);
		}

		yield return new WaitForSeconds (0f);
	}

	float Normalize( float angle)
	{
		if(angle< 0)
		{
			angle += 360f;
		}

		if(angle>360)
		{
			angle -= 360f;
		}

		return angle;
	}
}
