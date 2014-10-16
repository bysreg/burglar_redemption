using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {

	public float fovAngle = 110f; //degrees that the guard can see

	GameObject player;
	bool playerInSight;
	SphereCollider sphereCol;
	LastPlayerSighting lastPlayerSighting;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		sphereCol = GetComponent<SphereCollider>();
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == player)
		{
			playerInSight = false;

			Vector3 direction = player.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			if(angle < fovAngle * 0.5f)
			{
				RaycastHit hit;

				if(Physics.Raycast(transform.position, direction.normalized, out hit, sphereCol.radius))
				{
					if(hit.collider.gameObject == player)
					{
						playerInSight = true;

						lastPlayerSighting.position = player.transform.position;
					}
				}
			}
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == player)
		{
			playerInSight = false;
		}
	}

	void FixedUpdate()
	{

	}
}
