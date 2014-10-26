using UnityEngine;
using System.Collections;

public class PlayMovie : MonoBehaviour {

	MovieTexture movie;

	void Start()
	{
		movie = renderer.material.mainTexture as MovieTexture;
		movie.Play();
		audio.Play();
	}

	void Update()
	{
		if (!movie.isPlaying)
		{
			Application.LoadLevel (Application.loadedLevel +1);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
	}

}
