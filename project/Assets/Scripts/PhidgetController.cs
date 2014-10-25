using UnityEngine;
using System.Collections;
using Phidgets;

public class PhidgetController : MonoBehaviour {

	InterfaceKit ifk;

	public static int LockSensor;

	void Start()
	{
		ifk = new InterfaceKit();
		ifk.open();
		ifk.waitForAttachment(5000);
	}

	void Update()
	{

		int sensor1 = ifk.sensors[0].Value;
		LockSensor = ifk.sensors[6].Value;

		//print (sensor1 + " " + sensor2);
	}
}
