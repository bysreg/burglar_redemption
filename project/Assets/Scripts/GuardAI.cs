using UnityEngine;
using System.Collections;

public class GuardAI : MonoBehaviour {

	public float patrolSpeed = 2f;
	public Transform[] patrolWayPoints;

	NavMeshAgent nav;
	int wayPointIndex;

	void Awake()
	{
		nav = GetComponent<NavMeshAgent>();
		nav.updateRotation = false;
		//nav.destination = patrolWayPoints[wayPointIndex].position;
	}

	void Update()
	{
		//Patrolling();
	}

	void Patrolling()
	{
		nav.speed = patrolSpeed;

		if(nav.remainingDistance <= nav.stoppingDistance)
		{
			wayPointIndex = (wayPointIndex + 1) % patrolWayPoints.Length;
		}

		//print (wayPointIndex + " " + nav.remainingDistance + " " + nav.stoppingDistance);
		//print (nav.nextPosition);

		nav.destination = patrolWayPoints[wayPointIndex].position;
	}


}
