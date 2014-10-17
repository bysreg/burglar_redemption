﻿using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {
	
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
	int frameIndex;
	float frameTime;
	Texture2D[] walkTextures;
	GameObject graphic;
	NavMeshAgent nav;

	void Awake()
	{
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

	void Update()
	{
		prevWalkingDir = walkingDir;
		walkingDir = -1;

		Vector3 direction = nav.velocity;

		if(direction == Vector3.zero)
		{
			walkingDir = -1;
		}
		else if(Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
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

	public int GetFaceDir()
	{
		return walkingDir;
	}
}
