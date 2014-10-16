using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float speed;
	public Texture2D standTexture;
	public Texture2D[] walkHorTextures;
	public float walkHorFrameTime;
	public Texture2D[] walkVerTextures;
	public float walkVerFrameTime;

	bool simulateWithKeyboard;
	Material playerMat;
	float animTime;
	int walkingDir;
	int prevWalkingDir;
	int frameIndex;
	float frameTime;
	
	void Awake()
	{
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		simulateWithKeyboard = gameController.GetComponent<GameController>().simulateWithKeyboard;
		playerMat = GetComponentInChildren<MeshRenderer>().material;
		prevWalkingDir = -1;
		walkingDir = -1;
	}

	void Update()
	{
		if(prevWalkingDir != walkingDir)
		{
			animTime = 0;
			if(walkingDir == 2 || walkingDir == 3)
			{
				frameTime = walkHorFrameTime;
			}
			else if(walkingDir == -1)
			{
				playerMat.mainTexture = standTexture;
			}
		}

		if(walkingDir != -1)
		{
			animTime += Time.deltaTime;
			if(animTime >= frameTime)
			{
				animTime -= frameTime;
				frameIndex = (frameIndex + 1) % walkHorTextures.Length;

				playerMat.mainTexture = walkHorTextures[frameIndex];
			}
		}
	}

	void FixedUpdate()
	{
		prevWalkingDir = walkingDir;
		walkingDir = -1;

		if(simulateWithKeyboard)
		{
			//up
			if(Input.GetKey(KeyCode.W))
			{
				rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
				walkingDir = 0;
			}
			//down
			else if(Input.GetKey(KeyCode.S))
			{
				rigidbody.MovePosition(transform.position + -transform.forward * speed * Time.fixedDeltaTime);
				walkingDir = 1;
			}

			//right
			if(Input.GetKey(KeyCode.D))
			{
				rigidbody.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
				walkingDir = 2;
			}
			//left
			else if(Input.GetKey(KeyCode.A))
			{
				rigidbody.MovePosition(transform.position + -transform.right * speed * Time.fixedDeltaTime);
				walkingDir = 3;
			}

		}
	}
	
}
