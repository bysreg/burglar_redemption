using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed;

	bool simulateWithKeyboard;

	void Awake()
	{
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		simulateWithKeyboard = gameController.GetComponent<GameController>().simulateWithKeyboard;
	}

	void FixedUpdate()
	{
		if(simulateWithKeyboard)
		{
			//up
			if(Input.GetKey(KeyCode.W))
			{
				rigidbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
			}

			//right
			if(Input.GetKey(KeyCode.D))
			{
				rigidbody.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
			}

			//down
			if(Input.GetKey(KeyCode.S))
			{
				rigidbody.MovePosition(transform.position + -transform.forward * speed * Time.fixedDeltaTime);
			}

			//left
			if(Input.GetKey(KeyCode.A))
			{
				rigidbody.MovePosition(transform.position + -transform.right * speed * Time.fixedDeltaTime);
			}
		}
	}

}
