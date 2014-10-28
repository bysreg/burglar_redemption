using UnityEngine;
using System.Collections;
using Phidgets ;

public class Saw : MonoBehaviour 
{
	public float omega;
	public float A;
	public float B;
	//public float Phase; //unused ?

	bool isSawing;
	bool isSitting;
	float BarHealth;
	float DamageRate;
	float theta;
	Vector2 root;
	InterfaceKit ifk;
	ParticleSystem sparkPS;
	GameObject saw;
	GameObject sit;
	GameObject kneel;
	AudioSource sawAudio;
	bool initialized;
	SceneFader sceneFader;
	bool usePhidgets; // automatically initialized if there is phidgets connected
	float oldRotationSensorVal;
	float rotationSensorVal;
	float deltaRotationSensor = 2; // a delta value of rotation sensor to be considered there is a rotation

	void Awake()
	{
		sparkPS = GameObject.Find("Spark").GetComponent<ParticleSystem>();
		saw = GameObject.FindGameObjectWithTag("Saw");
		sit = GameObject.FindGameObjectWithTag("Sit");
		kneel = GameObject.FindGameObjectWithTag("Kneel");
		sawAudio = saw.GetComponent<AudioSource>();
		sceneFader = GameObject.Find ("SceneFader").GetComponent<SceneFader>();
	}
	
	void Start () 
	{
		theta = 0f;
		root = transform.position;
		isSawing = false;
		BarHealth = 400.0f;
		DamageRate = 10.0f;
		kneel.GetComponent<SpriteRenderer>().enabled = false;
		saw.GetComponent<SpriteRenderer>().enabled = false;

		sparkPS.enableEmission = false;

		ifk = new InterfaceKit();
		ifk.open ();
		try
		{
			ifk.waitForAttachment(2000);
			usePhidgets = true;
			rotationSensorVal = oldRotationSensorVal = ifk.sensors[0].Value;
		}
		catch(Phidgets.PhidgetException)
		{
			//no phidgets attached
		}
		print ("use Phidgets for sawing ? " + usePhidgets);
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.G))
		{
			sceneFader.FadeOutScene(1, () => {Application.LoadLevel(Application.loadedLevel + 1);});
		}

		if(!initialized)
		{
			sceneFader.FadeInScene();
			initialized = true;
			return;
		}

		if(!isSitting)
		{
			if (usePhidgets) 
			{
				oldRotationSensorVal = rotationSensorVal;
				rotationSensorVal = ifk.sensors[0].Value;

				if(Mathf.Abs(rotationSensorVal - oldRotationSensorVal) >= deltaRotationSensor)
				{
					if(!isSawing)
					{
						StartSawing();
					}
					else
					{
						ContinueSawing();
					}
				}
				else
				{
					if(isSawing)
					{
						StopSawing();
					}
				}
			}
			else
			{
				if(Input.GetKeyDown (KeyCode.Space))
				{
					StartSawing();
				}
				
				if(Input.GetKey (KeyCode.Space))
				{
					ContinueSawing();
				}
				
				if(Input.GetKeyUp (KeyCode.Space))
				{
					StopSawing();
				}
			}
		}

		if(Input.GetKey(KeyCode.F))
		{
			Sit();
		}
		else
		{
			Kneel();
		}

		if(BarHealth <0)
		{
			sceneFader.FadeOutScene(1, () => {Application.LoadLevel(Application.loadedLevel + 1);});
		}
	}

	void Move()
	{
		theta += omega * Time.deltaTime;
		saw.rigidbody2D.MovePosition (root + new Vector2(A * Mathf.Sin (theta), B * Mathf.Sin (theta)));
	}

	void StartSawing()
	{
		isSawing = true;
		
		saw.GetComponent<SpriteRenderer>().enabled = true;
		sit.GetComponent<SpriteRenderer>().enabled = false;
		kneel.GetComponent<SpriteRenderer>().enabled = true;
		//theta = Phase * Mathf.Deg2Rad;
		sawAudio.Play();
		
		sparkPS.enableEmission = true;
	}

	void ContinueSawing()
	{
		Move();
		BarHealth -= DamageRate * Time.deltaTime ;
	}

	void StopSawing()
	{
		isSawing = false;
		//fixme ? 
		//saw.GetComponent<SpriteRenderer>().enabled = false;
		//sit.GetComponent<SpriteRenderer>().enabled = true;
		//kneel.GetComponent<SpriteRenderer>().enabled = false;
		sawAudio.Stop();
		sparkPS.enableEmission = false;
	}

	void Sit()
	{
		StopSawing ();

		isSitting = true;
		saw.GetComponent<SpriteRenderer>().enabled = false;
		sit.GetComponent<SpriteRenderer>().enabled = true;
		kneel.GetComponent<SpriteRenderer>().enabled = false;
	}

	void Kneel()
	{
		isSitting = false;
		saw.GetComponent<SpriteRenderer>().enabled = true;
		sit.GetComponent<SpriteRenderer>().enabled = false;
		kneel.GetComponent<SpriteRenderer>().enabled = true;
	}

	public bool IsSawing()
	{
		return isSawing;
	}

	public bool IsSitting()
	{
		return isSitting;
	}
}
