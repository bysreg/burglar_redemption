using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	
	public int health = 10;

	public void Hit(int amount)
	{
		health -= amount;
	}
}
