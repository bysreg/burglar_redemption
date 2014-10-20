using UnityEngine;
using System.Collections;

public class GuardAI : MonoBehaviour {
	
	public float patrolSpeed = 1.5f;
	public float chaseSpeed = 4f;
	public float chaseWaitTime = 5f;
	public float patrolWaitTime = 10f;
	public Transform[] patrolWayPoints;
	public bool stopAfterFinishingPatrol = false;

	public static bool guardActive = false;

	bool patrol = true; // does this guard able to patrol ?

	NavMeshAgent nav;
	GuardSight guardSight;
	int wayPointIndex;
	float chaseTimer;
	float patrolTimer;

	GameObject player;
	LastPlayerSighting lastPlayerSighting;

	//debugging
	string patrolling = "patrolling";
	string chasing = "chasing";
	string waitingAfterChase = "waiting after chase";
	string waitingAfterPatrol = "waiting after patrol";
	string successCatchPlayer = "success catch player";
	string guardState;
	bool isSuccessCatchPlayer;

	void Awake()
	{
		nav = GetComponent<NavMeshAgent>();
		nav.updateRotation = false;
		guardSight = GetComponent<GuardSight>();
		player = GameObject.FindGameObjectWithTag("Player");
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();
	}

	void Update()
	{
		if(isSuccessCatchPlayer)
		{
			nav.Stop ();
		}
		else if(guardSight.personalLastSighting != lastPlayerSighting.resetPosition)
		{
			Chasing ();
		}
		else
		{
			Patrolling();
		}
	}

	void Patrolling()
	{
		if(!patrol || !guardActive)
			return;

		guardState = patrolling;
		nav.speed = patrolSpeed;

		if(nav.remainingDistance <= nav.stoppingDistance)
		{
			//print (patrolTimer);
			patrolTimer += Time.deltaTime;
			guardState = waitingAfterPatrol;

			if(patrolTimer >= patrolWaitTime)
			{
				if(stopAfterFinishingPatrol && wayPointIndex + 1 == patrolWayPoints.Length)
				{
					patrol = false;
					nav.Stop();
					return;
				}

				wayPointIndex = (wayPointIndex + 1) % patrolWayPoints.Length;

				patrolTimer -= patrolWaitTime;
			}
		}

		//print (wayPointIndex + " " + nav.remainingDistance + " " + nav.stoppingDistance);

		nav.destination = patrolWayPoints[wayPointIndex].position;
	}

	void Chasing()
	{
		guardState = chasing;
		Vector3 sightingDir = guardSight.personalLastSighting - transform.position;

		if(sightingDir.sqrMagnitude > 4f)
		{
			nav.destination = guardSight.personalLastSighting;
		}

		nav.speed = chaseSpeed;

		if(nav.remainingDistance < nav.stoppingDistance)
		{
			chaseTimer += Time.deltaTime;
			guardState = waitingAfterChase;

			if(chaseTimer >= chaseWaitTime)
			{
				chaseTimer -= chaseWaitTime;

				guardSight.personalLastSighting = lastPlayerSighting.resetPosition;
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
			}
		}
	}

	public string GetGuardState()
	{
		return guardState;
	}

	public void CatchPlayer()
	{
		isSuccessCatchPlayer = true;
		guardState = successCatchPlayer;
	}

}
