using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float speed;
	public Texture2D standTexture;
	public Texture2D[] walkHorTextures;
	public float walkHorFrameTime;
	public Texture2D[] walkUpTextures;
	public float walkUpFrameTime;
	public Texture2D[] walkDownTextures;
	public float walkDownFrameTime;
	
	bool simulateWithKeyboard;
	Material playerMat;
	float animTime;
	int walkingDir;
	int prevWalkingDir;
	int frameIndex;
	float frameTime;
	Texture2D[] walkTextures;
	GameObject graphic;

	void Awake()
	{
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		simulateWithKeyboard = gameController.GetComponent<GameController>().simulateWithKeyboard;
		playerMat = GetComponentInChildren<MeshRenderer>().material;
		graphic = transform.Find("Graphic").gameObject;
		prevWalkingDir = -1;
		walkingDir = -1;
		walkTextures = walkHorTextures;
		frameTime = walkHorFrameTime;
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
				playerMat.mainTexture = walkHorTextures[0];
			}
			else if(walkingDir == 0)
			{
				frameTime = walkUpFrameTime;
				walkTextures = walkUpTextures;
				playerMat.mainTexture = walkUpTextures[0];
			}
			else if(walkingDir == 1)
			{
				frameTime = walkDownFrameTime;
				walkTextures = walkDownTextures;
				playerMat.mainTexture = walkDownTextures[0];
			}
			else if(walkingDir == -1)
			{
				playerMat.mainTexture = standTexture;
			}
		}
		
		if(walkingDir != -1)
		{
			animTime += Time.fixedDeltaTime;
			if(animTime >= frameTime)
			{
				animTime -= frameTime;
				frameIndex = (frameIndex + 1) % walkTextures.Length;
				
				playerMat.mainTexture = walkTextures[frameIndex];
			}
		}
	}

	void FixedUpdate()
	{
		prevWalkingDir = walkingDir;
		walkingDir = -1;
		Vector3 newPos = transform.position;
		Vector3 direction = Vector3.zero;

		if(simulateWithKeyboard)
		{
			//up
			if(Input.GetKey(KeyCode.W))
			{
				MoveUp(ref direction);
			}
			//down
			else if(Input.GetKey(KeyCode.S))
			{
				MoveDown(ref direction);
			}

			//right
			if(Input.GetKey(KeyCode.D))
			{
				MoveRight(ref direction);
			}
			//left
			else if(Input.GetKey(KeyCode.A))
			{
				MoveLeft(ref direction);
			}
		}

		if(walkingDir != -1)
		{
			rigidbody.MovePosition(transform.position + direction.normalized * speed * Time.fixedDeltaTime);
		}

		AnimWalk();
	}

	void MoveUp(ref Vector3 direction)
	{
		direction.z = 1;
		walkingDir = 0;
	}

	void MoveDown(ref Vector3 direction)
	{
		direction.z = -1;
		walkingDir = 1;
	}

	void MoveRight(ref Vector3 direction)
	{
		direction.x = 1;
		walkingDir = 2;
		graphic.transform.localScale = new Vector3(1, 1, 1);
	}

	void MoveLeft(ref Vector3 direction)
	{
		direction.x = -1;
		walkingDir = 3;
		graphic.transform.localScale = new Vector3(-1, 1, 1);
	}
	
}
