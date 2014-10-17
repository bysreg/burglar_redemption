using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {

	public float fovAngle = 110f; //degrees that the guard can see
	public Texture2D standTexture;
	public Texture2D[] walkHorTextures;
	public float walkHorFrameTime;
	public Texture2D[] walkUpTextures;
	public float walkUpFrameTime;
	public Texture2D[] walkDownTextures;
	public float walkDownFrameTime;

	GameObject player;
	bool playerInSight;
	SphereCollider sphereCol;
	LastPlayerSighting lastPlayerSighting;

	Material guardMat;
	float animTime;
	int walkingDir;
	int prevWalkingDir;
	int frameIndex;
	float frameTime;
	Texture2D[] walkTextures;
	GameObject graphic;
	NavMeshAgent nav;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		sphereCol = GetComponent<SphereCollider>();
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		lastPlayerSighting = gameController.GetComponent<LastPlayerSighting>();

		graphic = transform.Find("Graphic").gameObject;
		guardMat = graphic.GetComponent<MeshRenderer>().material;
		prevWalkingDir = -1;
		walkingDir = -1;
		walkTextures = walkHorTextures;
		nav = GetComponent<NavMeshAgent>();
	}

	void AnimWalk()
	{
		if(prevWalkingDir != walkingDir)
		{
			animTime = 0;
			frameIndex = 0;
			if(walkingDir == 2 || walkingDir == 3)
			{
				frameTime = walkHorFrameTime;
				walkTextures = walkHorTextures;
				guardMat.mainTexture = walkHorTextures[0];
			}
			else if(walkingDir == 0)
			{
				frameTime = walkUpFrameTime;
				walkTextures = walkUpTextures;
				guardMat.mainTexture = walkUpTextures[0];
			}
			else if(walkingDir == 1)
			{
				frameTime = walkDownFrameTime;
				walkTextures = walkDownTextures;
				guardMat.mainTexture = walkDownTextures[0];
			}
			else if(walkingDir == -1)
			{
				guardMat.mainTexture = standTexture;
			}
		}
		
		if(walkingDir != -1)
		{
			animTime += Time.fixedDeltaTime;
			if(animTime >= frameTime)
			{
				animTime -= frameTime;
				frameIndex = (frameIndex + 1) % walkTextures.Length;
				
				guardMat.mainTexture = walkTextures[frameIndex];
			}
		}
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
		prevWalkingDir = walkingDir;
		walkingDir = -1;

		Vector3 direction = nav.velocity;
		print (direction);

		if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
		{
			if(direction.x > 0) // right
			{
				walkingDir = 2;
				graphic.transform.localScale = new Vector3(1, 1, 1);
			}
			else // left
			{
				walkingDir = 3;
				graphic.transform.localScale = new Vector3(-1, 1, 1);
			}
		}
		else
		{
			if(direction.z > 0) // up
			{
				walkingDir = 0;
			}
			else // down
			{
				walkingDir = 1;
			}
		}

		AnimWalk();
	}
}
