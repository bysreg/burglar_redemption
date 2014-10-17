using UnityEngine;
using System.Collections;

public class GuardSight : MonoBehaviour {

	public float fovAngle = 110f; //degrees that the guard can see
	public bool debugSight;

	GameObject player;
	bool playerInSight;
	SphereCollider sphereCol;
	LastPlayerSighting lastPlayerSighting;
	Guard guard;

	//debug
	Vector3 debugLeftSightDir;
	Vector3 debugRightSightDir;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		sphereCol = GetComponent<SphereCollider>();
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();
		guard = GetComponent<Guard>();
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == player)
		{
			playerInSight = false;
			
			Vector3 direction = player.transform.position - transform.position;
			float angle = Vector3.Angle(direction, -transform.forward);
			
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

	void Update()
	{
		if(debugSight)
		{
			DebugSight();
		}
	}

	void DebugSight()
	{	
		int dir = guard.GetFaceDir();

		switch(dir)
		{
		case 0:
			debugLeftSightDir = Quaternion.Euler(0, fovAngle / 2, 0) * transform.forward;
			debugRightSightDir = Quaternion.Euler(0, -fovAngle / 2, 0) * transform.forward;
			break;

		case 1:
			debugLeftSightDir = Quaternion.Euler(0, -fovAngle / 2, 0) * -transform.forward;
			debugRightSightDir = Quaternion.Euler(0, fovAngle / 2, 0) * -transform.forward;
			break;

		case 2:
			debugLeftSightDir = Quaternion.Euler(0, fovAngle / 2, 0) * transform.right;
			debugRightSightDir = Quaternion.Euler(0, fovAngle / 2, 0) * transform.right;
			break;

		case 3:
		default:
			debugLeftSightDir = Quaternion.Euler(0, -fovAngle / 2, 0) * -transform.right;
			debugRightSightDir = Quaternion.Euler(0, fovAngle / 2, 0) * -transform.right;
			break;
		}


	}
}
