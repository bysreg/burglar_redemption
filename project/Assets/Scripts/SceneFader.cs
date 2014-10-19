using UnityEngine;
using System.Collections;

public class SceneFader : MonoBehaviour {

	public bool isFading;
	public float targetAlpha;

	void Awake()
	{
		guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
		guiTexture.enabled = false;
	}

	void Update()
	{
		if(isFading)
		{

		}
	}

	public void FadeInScene()
	{
		guiTexture.enabled = true;
		isFading = true;
		targetAlpha = 1f;
	}

	public void FadeOutScene()
	{
		guiTexture.enabled = true;
		isFading = true;
		targetAlpha = 0f;
	}

}
