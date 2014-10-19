using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	public Object nextScene;

	MovieTexture movie;

	void Start()
	{
		movie = renderer.material.mainTexture as MovieTexture;
		movie.Play();
	}

	void Update()
	{
		if(!movie.isPlaying)
		{
			if(nextScene != null)
				Application.LoadLevel(nextScene.name);
		}
	}

}
