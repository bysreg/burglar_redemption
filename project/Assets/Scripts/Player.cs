using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    public float speed;
    public Texture2D standTexture;
    public Texture2D[] walkHorTextures;
    public float walkHorFrameTime;
    public Texture2D[] walkUpTextures;
    public float walkUpFrameTime;
    public Texture2D[] walkDownTextures;
    public float walkDownFrameTime;

    bool simulateWithKeyboard;
    GameController gameController;
    Material playerMat;
    float animTime;
    int walkingDir;
    int prevWalkingDir;
    int frameIndex;
    float frameTime;
    Texture2D[] walkTextures;
    GameObject graphic;

    AudioSource footstepsSound;

	bool hasSaw;
	bool canMove = true;

    void Awake()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        this.gameController = gameController.GetComponent<GameController>();
        playerMat = GetComponentInChildren<MeshRenderer>().material;
        graphic = transform.Find("Graphic").gameObject;
        prevWalkingDir = -1;
        walkingDir = -1;
        walkTextures = walkHorTextures;
        frameTime = walkHorFrameTime;

        footstepsSound = gameController.transform.Find("FootstepsSound").GetComponent<AudioSource>();
    }

    void AnimWalk()
    {
        if (prevWalkingDir != walkingDir)
        {
            animTime = 0;
            frameIndex = 0;
            if (walkingDir == 2 || walkingDir == 3)
            {
                frameTime = walkHorFrameTime;
                walkTextures = walkHorTextures;
                playerMat.mainTexture = walkHorTextures[0];
            }
            else if (walkingDir == 0)
            {
                frameTime = walkUpFrameTime;
                walkTextures = walkUpTextures;
                playerMat.mainTexture = walkUpTextures[0];
            }
            else if (walkingDir == 1)
            {
                frameTime = walkDownFrameTime;
                walkTextures = walkDownTextures;
                playerMat.mainTexture = walkDownTextures[0];
            }
            else if (walkingDir == -1)
            {
                playerMat.mainTexture = standTexture;
            }
        }

        if (walkingDir != -1)
        {
            //play footsteps sound
            if (!footstepsSound.isPlaying)
            {
                footstepsSound.Play();
            }

            animTime += Time.fixedDeltaTime;
            if (animTime >= frameTime)
            {
                animTime -= frameTime;
                frameIndex = (frameIndex + 1) % walkTextures.Length;

                playerMat.mainTexture = walkTextures[frameIndex];
            }
        }
        else
        {
            //stop footsteps sound
            if (footstepsSound.isPlaying)
            {
                footstepsSound.Stop();
            }
        }
    }

    void FixedUpdate()
    {
		if(!canMove)
			return;

        prevWalkingDir = walkingDir;
        walkingDir = -1;
        Vector3 direction = Vector3.zero;

        if (gameController.simulateWithKeyboard)
        {
            //up
            if (Input.GetKey(KeyCode.W))
            {
                MoveUp(ref direction);
            }
            //down
            else if (Input.GetKey(KeyCode.S))
            {
                MoveDown(ref direction);
            }

            //right
            if (Input.GetKey(KeyCode.D))
            {
                MoveRight(ref direction);
            }
            //left
            else if (Input.GetKey(KeyCode.A))
            {
                MoveLeft(ref direction);
            }

            if (walkingDir != -1)
            {
                rigidbody.MovePosition(transform.position + direction.normalized * speed * Time.fixedDeltaTime);
            }
        }
        else
        {
            print(PSMoveInput.MoveControllers[0].Connected + " " + PSMoveInput.MoveControllers[1].Connected);
            if ((PSMoveInput.MoveControllers[0].Connected))
            {
                Vector3 gemPos, handlePos;
                MoveData moveData = PSMoveInput.MoveControllers[0].Data;
                gemPos = moveData.Position;
                handlePos = moveData.HandlePosition;

                Vector3 psmoveDir = gemPos - handlePos;
                print(psmoveDir.x + " " + psmoveDir.y);

                float delta = 0.2f;

                if (psmoveDir.y >= delta)
                {
                    MoveUp(ref direction);
                }
                else if (psmoveDir.y <= -delta)
                {
                    MoveDown(ref direction);
                }

                if (psmoveDir.x >= delta)
                {
                    MoveRight(ref direction);
                }
                else if (psmoveDir.x <= -delta)
                {
                    MoveLeft(ref direction);
                }

                psmoveDir.z = psmoveDir.y;
                psmoveDir.y = 0f;

                if (walkingDir != -1)
                {
                    rigidbody.MovePosition(transform.position + psmoveDir.normalized * speed * Time.fixedDeltaTime);
                }
            }
        }

        AnimWalk();
    }

    void MoveUp(ref Vector3 direction)
    {
        direction.z = 1;
        walkingDir = 0;
    }

    void MoveDown(ref Vector3 direction)
    {
        direction.z = -1;
        walkingDir = 1;
    }

    void MoveRight(ref Vector3 direction)
    {
        direction.x = 1;
        walkingDir = 2;
        graphic.transform.localScale = new Vector3(1, 1, 1);
    }

    void MoveLeft(ref Vector3 direction)
    {
        direction.x = -1;
        walkingDir = 3;
        graphic.transform.localScale = new Vector3(-1, 1, 1);
    }

	public void SetHasSaw()
	{
		hasSaw = true;
	}

	public void SetFreeze()
	{
		canMove = false;
		prevWalkingDir = 0; // force the prevwalkingdir and walkingdir to be different
		walkingDir = -1;
		AnimWalk();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.isTrigger == false && other.tag == "Guard")
		{
			//stop that guard from moving close in order to avoid collision
			other.GetComponent<GuardAI>().CatchPlayer();
		}
	}
}
