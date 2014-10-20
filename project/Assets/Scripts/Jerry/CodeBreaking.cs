using UnityEngine;
using System.Collections;

public class CodeBreaking : MonoBehaviour 
{
	private int[] TheCode={0,0,0,0};
	private SpriteRenderer[] DigitLight = {null,null,null,null}; 
	private int index;


	//private int OldDirection;
	//private bool found;
	//private bool Won;
	//private bool OnlyRunTillFirstScroll;

	public AudioClip[] LockSounds;
	public LockDial lockDial;
	private int Prev;


	// Use this for initialization
	void Start () 
	{
	
		index = 0;
		Prev = lockDial.AngleToNumber ();
		//found = false;
		//Won = false;
		//OnlyRunTillFirstScroll =  true;

		audio.clip = LockSounds [0];
		

		for(int i=0; i<4; i++)
		{
			TheCode[i]= Random.Range (0,50); 
			print((TheCode[i]).ToString()); 

			DigitLight[i] = transform.GetChild(i).GetComponent <SpriteRenderer>();
			DigitLight[i].enabled =false;
		}
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		SoundEffects ();

		/*
		if(OnlyRunTillFirstScroll)
		{
			if(NowDirection() != 0)
			{
				OldDirection = NowDirection();
				OnlyRunTillFirstScroll =false;
			}
		}


		if((found)&&(!Won))
		{
			if(NowDirection () != 0)
			{
				if(OldDirection == NowDirection ())
				{
					Reset();
				}

				else
				{
					found = false;
				}
			}
		}

		*/


		if((TheCode[index] == lockDial.AngleToNumber())/*&&(found == false)&&(Won == false)*/)
		{
			DigitLight[index].enabled = true;
			index++;

			audio.clip = LockSounds[2];
			audio.volume = 1.0f;
			audio.Play ();
			//found = true;
		}

		if(index > 3)
		{
			Application.LoadLevel (Application.loadedLevel +1);
			index=0;
			//Won = true;
		}
	}

	void Reset()
	{
		index = 0;

		for(int i=0; i<4; i++)
		{
			DigitLight [i].enabled =false;
		}
	}

	int NowDirection()
	{
		if(Input.GetAxis ("Mouse ScrollWheel") < 0)
		{
			return -1;
		}

		else if (Input.GetAxis ("Mouse ScrollWheel") > 0)
		{
			return 1;
		}

		else
		{
			return 0;
		}
	}

	void SoundEffects()
	{
		int VolumeIndex = Mathf.Abs (TheCode [index] - lockDial.AngleToNumber ());

		switch (VolumeIndex) 
		{
				case 0:
						audio.volume = 1.0f;
						break;

				case 1:
						audio.volume = 0.9f;
						break;

				case 2:
						audio.volume = 0.8f;
						break;

				case 3:
						audio.volume = 0.7f;
						break;

				case 4:
						audio.volume = 0.6f;
						break;

				default :
						audio.volume = 0.5f;
						break;

		}
			



		if(Prev != lockDial.AngleToNumber())
		{
			if(audio.clip==LockSounds[1])
			{
				audio.clip = LockSounds[0];
			}
			else
			{
				audio.clip = LockSounds[1];
			}

			if(audio.isPlaying == false)
			{
				audio.Play();
			}

			Prev = lockDial.AngleToNumber();
		}


	}
	
}
