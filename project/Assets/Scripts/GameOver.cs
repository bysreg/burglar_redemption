using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	GameObject jailBar;
	bool isGameOver;
	Vector3 jailRelativeToCamPos;
	Vector3 fromPos;
	Vector3 targetDiffPos = new Vector3(0, -83f, 0);
	Vector3 targetPos;
	float time;
	float speed = 0.5f;
	SceneFader sceneFader;

	void Awake()
	{
		jailBar = transform.Find("JailBar").gameObject;
		GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
		jailRelativeToCamPos = jailBar.transform.position - mainCam.transform.position;
		jailBar.transform.position = mainCam.transform.position + jailRelativeToCamPos;
		fromPos = jailBar.transform.position;
		targetPos = jailBar.transform.position + targetDiffPos;
		jailBar.SetActive(false);
		sceneFader = GameObject.Find("SceneFader").GetComponent<SceneFader>();
		//ShowGameOver();
	}

	public void ShowGameOver()
	{
		isGameOver = true;
		jailBar.SetActive(true);
	}

	void Update()
	{
		if(isGameOver)
		{
			time += Time.deltaTime * speed;
			jailBar.transform.position = new Vector3(targetPos.x, Mathf.SmoothStep(fromPos.y, targetPos.y, time), targetPos.z);

			if(time >= 1)
			{
				sceneFader.FadeOutScene(1, () => {Application.LoadLevel(Application.loadedLevel);});
			}
		}
	}
}
