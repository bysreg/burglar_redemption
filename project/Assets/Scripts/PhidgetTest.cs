using UnityEngine;
using System.Collections;
using Phidgets;

public class PhidgetTest : MonoBehaviour {
	
	InterfaceKit ifk;
	
	void Start()
	{
		ifk = new InterfaceKit();
		ifk.open();
		ifk.waitForAttachment();
		print ("done");
	}
	
	void Update()
	{
		int sensor1 = ifk.sensors[0].Value;
		int sensor2 = ifk.sensors[6].Value;
		
		print (sensor1 + " " + sensor2);
	}
}
