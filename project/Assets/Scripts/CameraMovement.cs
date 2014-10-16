using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	GameObject player;
	Vector3 relativePosition;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		relativePosition = transform.position - player.transform.position;
	}

	void FixedUpdate()
	{
		transform.position = relativePosition + player.transform.position;
	}
}
