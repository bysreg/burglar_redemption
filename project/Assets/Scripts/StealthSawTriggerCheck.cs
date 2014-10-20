using UnityEngine;
using System.Collections;

public class StealthSawTriggerCheck : MonoBehaviour {

	Player player;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag != "Player")
			return;

		player.SetHasSaw();
		this.gameObject.SetActive(false);
	}
}
