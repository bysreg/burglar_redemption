using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	GameObject player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}
}
