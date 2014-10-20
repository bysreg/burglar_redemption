using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {

	public int initialWalkDir = -1;
	public Texture2D standTexture;
	public Texture2D[] walkHorTextures;
	public float walkHorFrameTime;
	public Texture2D[] walkUpTextures;
	public float walkUpFrameTime;
	public Texture2D[] walkDownTextures;
	public float walkDownFrameTime;

	Material guardMat;
	float animTime;
	int walkingDir;
	int prevWalkingDir;
	Vector3 prevWalkingV;
	Vector3 walkingV;
	int frameIndex;
	float frameTime;
	Texture2D[] walkTextures;
	GameObject graphic;
	NavMeshAgent nav;
	GameController gameController;
	GameObject player;

	AudioSource footstepsSound;

	void Awake()
	{
		graphic = transform.Find("Graphic").gameObject;
		guardMat = graphic.GetComponent<MeshRenderer>().material;
		prevWalkingDir = -1;
		walkingDir = initialWalkDir;

		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag("Player");
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

		footstepsSound = GetComponent<AudioSource>();
	}

	void Start()
	{
		AnimWalk();
	}

	void AnimWalk()
	{
		if(prevWalkingDir != walkingDir)
		{
			animTime = 0;
			frameIndex = 0;
			if(walkingDir == 2)
			{
				frameTime = walkHorFrameTime;
				walkTextures = walkHorTextures;
				guardMat.mainTexture = walkHorTextures[0];
				graphic.transform.localScale = new Vector3(graphic.transform.localScale.y, graphic.transform.localScale.y, graphic.transform.localScale.z);
			}
			else if(walkingDir == 3)
			{
				frameTime = walkHorFrameTime;
				walkTextures = walkHorTextures;
				guardMat.mainTexture = walkHorTextures[0];
				graphic.transform.localScale = new Vector3(-graphic.transform.localScale.y, graphic.transform.localScale.y, graphic.transform.localScale.z);
			}
			else if(walkingDir == 0)
			{
				frameTime = walkUpFrameTime;
				walkTextures = walkUpTextures;
				guardMat.mainTexture = walkUpTextures[0];
				graphic.transform.localScale = new Vector3(graphic.transform.localScale.y, graphic.transform.localScale.y, graphic.transform.localScale.z);
			}
			else if(walkingDir == 1)
			{
				frameTime = walkDownFrameTime;
				walkTextures = walkDownTextures;
				guardMat.mainTexture = walkDownTextures[0];
				graphic.transform.localScale = new Vector3(graphic.transform.localScale.y, graphic.transform.localScale.y, graphic.transform.localScale.z);
			}
			else if(walkingDir == -1)
			{
				guardMat.mainTexture = standTexture;
			}
		}
		
		if(nav.velocity != Vector3.zero)
		{
			//play footsteps sound
			if(!footstepsSound.isPlaying)
			{
				footstepsSound.Play();
			}

			animTime += Time.fixedDeltaTime;
			if(animTime >= frameTime)
			{
				animTime -= frameTime;
				frameIndex = (frameIndex + 1) % walkTextures.Length;
				
				guardMat.mainTexture = walkTextures[frameIndex];
			}
		}
		else
		{
			//stop footsteps sound
			if(footstepsSound.isPlaying)
			{
				footstepsSound.Stop();
			}
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject == player)
		{
			//game over
			gameController.GameOver();
		}
	}

	void Update()
	{
		prevWalkingDir = walkingDir;
		prevWalkingV = walkingV;
		walkingV = nav.velocity;

		if(walkingV != Vector3.zero)
		{
			if(Mathf.Abs(walkingV.x) > Mathf.Abs(walkingV.z))
			{
				if(walkingV.x > 0) // right
				{
					walkingDir = 2;
				}
				else // left
				{
					walkingDir = 3;
				}
			}
			else
			{
				if(walkingV.z > 0) // up
				{
					walkingDir = 0;
				}
				else // down
				{
					walkingDir = 1;
				}
			}
		}

		AnimWalk();
	}

	public int GetFaceDir()
	{
		return walkingDir == -1 ? 1 : walkingDir;
	}
}
