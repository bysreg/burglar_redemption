using UnityEngine;
using System.Collections;
using Phidgets ;

public class LockController : MonoBehaviour 
{
	public bool Emulator;
	private bool RotationLock;
	private float TargetRotation;
	public int Sensor, OldSensorValue;
	InterfaceKit ifk;

	public float RotationSpeed=1.0f;
	public GameObject SelectedCog;

	Vector3 Clockwise= new Vector3(0,0,-120);
	Vector3 AntiClockwise= new Vector3(0,0,-120);
	

	// Use this for initialization
	void Start () 
	{
		RotationLock = false;
		TargetRotation = 0;
		Parenting.target = "None";
		if(!Emulator)
		{
			ifk = new InterfaceKit();
			ifk.open ();
			ifk.waitForAttachment(5000);
		}

		Sensor = OldSensorValue = 500;

		SelectedCog = GameObject.FindGameObjectWithTag("Dummy");
	}
	
	// Update is called once per frame
	void Update () 
	{
		OldSensorValue = Sensor;
		//This part of the code modifies the value of the Sensor variable depending on how the emulator is used
		if(Emulator == false)
		{
			Sensor = ifk.sensors[6].Value;
		}

		else 
		{
			Sensor += Mathf.FloorToInt((Input.GetAxis ("Mouse ScrollWheel")*10));
		}


		//This part of the code controls which cog is selected at any given point in time

		if(Input.GetKeyDown(KeyCode.W))
		{
			Parenting.target = "TopCog";
		}

		if(Input.GetKeyDown (KeyCode.A))
		{
			Parenting.target = "LeftCog"; 
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			Parenting.target = "CentralCog";
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			Parenting.target = "RightCog";
		}


		if((Input.GetKeyUp(KeyCode.W))||(Input.GetKeyUp(KeyCode.A))||(Input.GetKeyUp(KeyCode.S))||(Input.GetKeyUp(KeyCode.D)))
		{
			Parenting.target = "Dummy";
		}

		SelectedCog = GameObject.FindGameObjectWithTag (Parenting.target);


		//Rotate ();


	}
	/*
	void Rotate()
	{
		if (Mathf.Abs(OldSensorValue- Sensor)>5)
		{
			SelectedCog.transform.Rotate (Vector3.Lerp ())
		}

	}
	*/
}
