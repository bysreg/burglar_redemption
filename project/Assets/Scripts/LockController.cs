﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Phidgets ;

public class LockController : MonoBehaviour 
{
	private bool RotationLock;
	private bool ButtonPressed;
	public int Sensor, OldSensorValue;
	InterfaceKit ifk;
	public GameObject SelectedCog;
	public GameObject[] Leaves;
	private float Omega=60f;
	public float AngularSpeed=60f;
	public float TargetAngle=0f;
	public float CurrentAngle=0f;

	Vector3 Clockwise= new Vector3(0,0,-120);
	Vector3 AntiClockwise= new Vector3(0,0,-120);
	float deltaRotationSensor = 2; // a delta value of rotation sensor to be considered there is a rotation
	bool usePhidgets;
	SceneFader sceneFader;
	bool initialized;
	AudioSource lockTickSound;	

	void Start () 
	{
		sceneFader = GameObject.Find ("SceneFader").GetComponent<SceneFader>();
		RotationLock = false;
		ButtonPressed = false;
		Parenting.target = "None";
		ifk = new InterfaceKit();
		ifk.open ();

		try 
		{
			ifk.waitForAttachment(2000);
			usePhidgets = true;
			Sensor = OldSensorValue = ifk.sensors[6].Value;
		}
		catch(Phidgets.PhidgetException)
		{
			// no phidgets attached
		}

		if(!usePhidgets)
		{
			deltaRotationSensor = 1; // for mouse, 1 point difference is enough
			Sensor = OldSensorValue = 500;
		}


		GameObject.Find ("/Lock/CogT/BackLight").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find ("/Lock/CogC/BackLight").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find ("/Lock/CogR/BackLight").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find ("/Lock/CogL/BackLight").GetComponent<SpriteRenderer>().enabled = false;

		SelectedCog = GameObject.FindGameObjectWithTag("Dummy");
		print ("use Phidgets for lock-picking ? " + usePhidgets);

		//background
		GameObject bg = GameObject.Find("bg");
		float distance = bg.transform.position.z - GameObject.FindGameObjectWithTag("MainCamera").transform.position.z; // for orthographic camera
		float height = Camera.main.orthographicSize * 2f;
		float width = height * Screen.width * 1.0f / Screen.height;
		bg.transform.localScale = new Vector3(width, height, 1);
		print (width + " " + height);

		lockTickSound = this.gameObject.GetComponentInChildren<AudioSource>();

		RandomizeLock();
	}

	void SelectCog(string parentName)
	{
		ButtonPressed = true;
		ResetParents();
		Parenting.target = parentName;
		SelectedCog = GameObject.FindGameObjectWithTag (Parenting.target);

		//make all other's cog disabled
		GameObject.Find ("/Lock/CogT/BackLight").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find ("/Lock/CogC/BackLight").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find ("/Lock/CogR/BackLight").GetComponent<SpriteRenderer>().enabled = false;
		GameObject.Find ("/Lock/CogL/BackLight").GetComponent<SpriteRenderer>().enabled = false;

		SelectedCog.transform.Find ("BackLight").GetComponent<SpriteRenderer> ().enabled = true;
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.G))
		{
			sceneFader.FadeOutScene(1, () => {Application.LoadLevel(Application.loadedLevel + 1);});
		}

		if(!initialized)
		{
			sceneFader.FadeInScene();
			initialized = true;
			return;
		}

		OldSensorValue = Sensor;

		if(usePhidgets)
		{
			Sensor = ifk.sensors[6].Value;
		}
		else 
		{
			Sensor += Mathf.FloorToInt((Input.GetAxis ("Mouse ScrollWheel")*10));
		}
		
		if(Input.GetKeyDown(KeyCode.W))
		{
			SelectCog("TopCog");
		}

		if(Input.GetKeyDown (KeyCode.A))
		{
			SelectCog("LeftCog");
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			SelectCog("CentralCog");
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			SelectCog("RightCog");
		}

		if((Input.GetKeyUp(KeyCode.W))||(Input.GetKeyUp(KeyCode.A))||(Input.GetKeyUp(KeyCode.S))||(Input.GetKeyUp(KeyCode.D)))
		{
			ButtonPressed = false;
			SelectedCog.transform.Find("BackLight").GetComponent<SpriteRenderer>().enabled = false;
			//Parenting.target = "Dummy";
		}

		if( (ButtonPressed ==true) && (  Mathf.Abs(OldSensorValue - Sensor) >= deltaRotationSensor) && RotationLock == false )                                   //Edit here if you want to switch direction of rotation
		{
			if(OldSensorValue > Sensor)
			{
				StartRotatingLock(false);
				//print ("counter clockwise");

			}
			else if(OldSensorValue < Sensor)
			{
				StartRotatingLock(true);
				//print ("clockwise");
			}
		}

		StartCoroutine (RotateThing ());
	}

	void StartRotatingLock(bool isClockwise)
	{
		RotationLock = true;
		CurrentAngle = SelectedCog.rigidbody2D.rotation;

		if(isClockwise)
		{
			TargetAngle = Normalize(CurrentAngle - 120f);
			Omega = -AngularSpeed;
		}
		else
		{
			TargetAngle = Normalize(CurrentAngle + 120f);
			Omega = AngularSpeed;
		}

		lockTickSound.Play();
	}

	void InstantRotatingLock(string lockName, bool isClockwise, int count)
	{
		GameObject cog;
		if(lockName == "top")
		{
			cog = GameObject.FindGameObjectWithTag ("TopCog");
		}
		else if(lockName == "center")
		{
			cog = GameObject.FindGameObjectWithTag ("CentralCog");
		}
		else if(lockName == "left")
		{
			cog = GameObject.FindGameObjectWithTag ("LeftCog");
		}
		else if(lockName == "right")
		{
			cog = GameObject.FindGameObjectWithTag ("RightCog");
		}
		else
		{
			return;
		}

		GameObject[] leaves = FindLeavesInsideCog(cog);
		for(int i=0; i<3; i++)
		{
			leaves[i].transform.parent = cog.transform;
		}

		if(isClockwise)
		{
			cog.transform.Rotate(new Vector3(0, 0, -120 * count));
		}
		else
		{
			cog.transform.Rotate(new Vector3(0, 0, 120 * count));
		}
	}
	
	void ResetParents()
	{
		for(int i=0; i<9; i++)
		{
			Leaves[i].transform.parent = null;
		}
	}

	IEnumerator RotateThing()
	{
		SelectedCog.rigidbody2D.rotation = Normalize (SelectedCog.rigidbody2D.rotation);
		
		if(RotationLock)
		{
			SelectedCog.rigidbody2D.MoveRotation(SelectedCog.rigidbody2D.rotation + Time.deltaTime * Omega);
			yield return new WaitForSeconds (Time.deltaTime);
		}
		
		if(RotationLock && Mathf.Abs(SelectedCog.rigidbody2D.rotation - TargetAngle ) < Mathf.Abs(2*Omega*Time.deltaTime) )
		{
			RotationLock = false;
			lockTickSound.Stop();
			SelectedCog.rigidbody2D.MoveRotation(TargetAngle);
			if(CheckIsWin())
			{
				sceneFader.FadeOutScene(1, () => {Application.LoadLevel(Application.loadedLevel + 1);});
			}
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
	
	bool CheckIsWin()
	{
		return CheckCog(GameObject.FindGameObjectWithTag("TopCog"), "LeafR") &&
			CheckCog(GameObject.FindGameObjectWithTag("RightCog"), "LeafG") &&
			CheckCog(GameObject.FindGameObjectWithTag("LeftCog"), "LeafB");
	}

	bool CheckCog(GameObject cog, string leafName)
	{
		GameObject[] leavesInside = FindLeavesInsideCog(cog);
		foreach(var leaf in leavesInside)
		{
			if(leaf.name != leafName)
				return false;
		}

		return true;
	}

	void RandomizeLock()
	{
		InstantRotatingLock("center", true, 1);
		InstantRotatingLock("left", true, 2);
		InstantRotatingLock("right", true, 2);
		InstantRotatingLock("top", true, 2);
	}

	GameObject[] FindLeavesInsideCog(GameObject cog)
	{
		GameObject[] ret = new GameObject[3];
		CircleCollider2D col = cog.GetComponent<CircleCollider2D>();
		float sqrRadius = col.radius * col.radius;
		int count = 0;

		for(int i=0; i<Leaves.Length; i++)
		{
			if((Leaves[i].transform.position - cog.transform.position).sqrMagnitude <= sqrRadius)
			{
				ret[count++] = Leaves[i];
			}
		}

		return ret;
	}
}
