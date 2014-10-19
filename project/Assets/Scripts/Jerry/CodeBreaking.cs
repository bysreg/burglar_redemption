using UnityEngine;
using System.Collections;

public class CodeBreaking : MonoBehaviour 
{
	private int[] TheCode={0,0,0,0};
	private SpriteRenderer[] DigitLight = {null,null,null,null}; 
	private int index;
	private int OldDirection;
	private bool found;
	private bool Won;
	private bool OnlyRunTillFirstScroll;

	public LockDial lockDial;

	// Use this for initialization
	void Start () 
	{
		index = 0;
		found = false;
		Won = false;
		OnlyRunTillFirstScroll =  true;
		

		for(int i=0; i<4; i++)
		{
			TheCode[i]= Random.Range (0,50); 
			print((TheCode[i]*2).ToString()); 

			DigitLight[i] = transform.GetChild(i).GetComponent <SpriteRenderer>();
			DigitLight[i].enabled =false;
		}
	
	}
	
	// Update is called once per frame
	void Update () 
	{
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


		if((TheCode[index] == lockDial.AngleToNumber())&&(found == false)&&(Won == false))
		{
			DigitLight[index].enabled = true;
			index++;
			found = true;
		}

		if(index > 3)
		{
			index=0;
			Won = true;
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
	
}
