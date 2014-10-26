using UnityEngine;
using System.Collections;
using Phidgets ;

public class LockController : MonoBehaviour 
{
	public bool Emulator;
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

	// Use this for initialization
	void Start () 
	{
		RotationLock = false;
		ButtonPressed = false;
		Parenting.target = "None";
		if(!Emulator)
		{
			ifk = new InterfaceKit();
			ifk.open ();
			ifk.waitForAttachment(5000);
		}
		else
		{
			deltaRotationSensor = 1; // for mouse, 1 point difference is enough
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
		
		if(Input.GetKeyDown(KeyCode.W))
		{
			ButtonPressed = true;
			ResetParents();
			Parenting.target = "TopCog";
		}

		if(Input.GetKeyDown (KeyCode.A))
		{
			ButtonPressed = true;
			ResetParents();
			Parenting.target = "LeftCog"; 
		}

		if(Input.GetKeyDown(KeyCode.S))
		{
			ButtonPressed = true;
			ResetParents();
			Parenting.target = "CentralCog";
		}

		if(Input.GetKeyDown(KeyCode.D))
		{
			ButtonPressed = true;
			ResetParents();
			Parenting.target = "RightCog";
		}


		if((Input.GetKeyUp(KeyCode.W))||(Input.GetKeyUp(KeyCode.A))||(Input.GetKeyUp(KeyCode.S))||(Input.GetKeyUp(KeyCode.D)))
		{
			ButtonPressed = false;
			//Parenting.target = "Dummy";
		}

		if( (ButtonPressed ==true) && (  Mathf.Abs(OldSensorValue - Sensor) >= deltaRotationSensor) && RotationLock == false )                                   //Edit here if you want to switch direction of rotation
		{
			if(OldSensorValue > Sensor)
			{
				RotationLock = true;
				CurrentAngle = SelectedCog.rigidbody2D.rotation ;
				TargetAngle = CurrentAngle + 120f;
				TargetAngle = Normalize (TargetAngle);
				Omega = AngularSpeed ;

			}
			else if(OldSensorValue < Sensor)
			{
				RotationLock = true;
				CurrentAngle = SelectedCog.rigidbody2D.rotation ;
				TargetAngle = CurrentAngle - 120f;
				TargetAngle = Normalize (TargetAngle);
				Omega = -AngularSpeed;
			}
		}

		SelectedCog = GameObject.FindGameObjectWithTag (Parenting.target);
		StartCoroutine (RotateThing ());
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
		
		if(RotationLock == true)
		{
			SelectedCog.rigidbody2D.MoveRotation(SelectedCog.rigidbody2D.rotation + Time.deltaTime * Omega);
			yield return new WaitForSeconds (Time.deltaTime);
		}
		
		if(Mathf.Abs(SelectedCog.rigidbody2D.rotation - TargetAngle ) < Mathf.Abs(2*Omega*Time.deltaTime) )
		{
			RotationLock =false;
			SelectedCog.rigidbody2D.MoveRotation(TargetAngle);
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

	bool HasWon()
	{
		bool verdict = true; 

		if(RotationLock == false)
		{
			GameObject temp;
			Transform temp2;

			temp = GameObject.FindGameObjectWithTag("LeftCog");
			Parenting.target = "LeftCog";

			for (int i=0; i<3; i++)
			{
				temp2 =temp.transform.GetChild(i);
				if(temp2.name != "LeafB")
				{
					verdict= false;
				}
			}	

			temp = GameObject.FindGameObjectWithTag("TopCog");
			Parenting.target = "TopCog";
			
			for (int i=0; i<3; i++)
			{
				temp2 =temp.transform.GetChild(i);
				if(temp2.name != "LeafR")
				{
					verdict= false;
				}
			}	

			temp = GameObject.FindGameObjectWithTag("RightCog");
			Parenting.target = "RightCog";
			
			for (int i=0; i<3; i++)
			{
				temp2 =temp.transform.GetChild(i);
				if(temp2.name != "LeafG")
				{
					verdict= false;
				}
			}	
		}

		else
		{
			verdict = false;
		}

		return verdict;
	}
}
