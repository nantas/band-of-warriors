using UnityEngine;
using System.Collections;

public class WarriorControl : MonoBehaviour {

	public float initMoveSpeed = 120.0f;
    public float initJumpSpeed = 500.0f;
    public float dashDuration = 0.5f;
	public exSprite[] allBodyParts;
    public exSprite spFX;
    public PlayMakerFSM lancerFSM;

	[System.NonSerialized] public MoveDir charMoveState;
    //[System.NonSerialized] public JumpState charJumpState;
    //[System.NonSerialized] public HurtState charHurtState;
    //[System.NonSerialized] public ActionState charActionState;
    private ActionState charActionState;
    private HurtState charHurtState;
    private JumpState charJumpState;
	private float flashTime = 0.0f;
    private float stunTime = 0.1f;
    private Vector2 velocity;
	  
	void Awake () {
		flashTime = allBodyParts[0].spanim.animations[0].length;
        lancerFSM = transform.GetComponent<PlayMakerFSM>();
	}
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(1,1,1);
		charMoveState = MoveDir.Right;
        charJumpState = JumpState.Ground;
        charHurtState = HurtState.Hitable;
        charActionState = ActionState.Free;
		animation["walk"].speed = initMoveSpeed/120.0f;
        velocity = new Vector2 (initMoveSpeed, 0);
		//StartWalk();
		
	}

    /*
    void Update () {
        if (Input.GetButtonDown("Right")) {
            lancerFSM.FsmVariables.GetFsmFloat("varHVelocity").Value = initMoveSpeed;
        }
        if (Input.GetButtonDown("Left")) {
            lancerFSM.FsmVariables.GetFsmFloat("varHVelocity").Value = -initMoveSpeed;
        }
        if (Input.GetButtonDown("Jump")) {
            lancerFSM.FsmVariables.GetFsmFloat("varVVelocity").Value = initJumpSpeed;
        }
        
    }

	// Update is called once per frame
    
	void Update () {
		//handle input
		if ( Input.GetButtonDown("Right") ) {
			TurnRight();
		}
		if ( Input.GetButtonDown("Left") ) {
			TurnLeft();
		}
		if ( Input.GetButtonDown("Jump") ) {
			StartJump();
		}
            
		
        //handle Jump
        if (charJumpState == JumpState.Ready2Jump) {
            if (!animation.IsPlaying("jump"))
                animation.Play("jump");
            charJumpState = JumpState.InAir;
            velocity.y = initJumpSpeed;
        }
		//handle horizontal movement
		float horizonDist = Time.deltaTime * velocity.x;
        float verticalDist = 0;
		switch (charMoveState) {
			case MoveDir.Right:
				//don't need to change
				break;
			case MoveDir.Left:
				horizonDist = -horizonDist;
				break;
			case MoveDir.Stop:
				horizonDist = 0;
				break;
			default:
				horizonDist = 0;
				break;
		}
        //handle vertical movement
        if (charJumpState == JumpState.InAir) {
            velocity.y += Game.instance.gravity;
            verticalDist = Time.deltaTime * velocity.y;
        }
		//move character
        if (charHurtState != HurtState.Stun){
    		//horizontal
            if ((transform.position.x + horizonDist < Game.instance.rightBoundary.position.x) 
	    		&& (transform.position.x + horizonDist > Game.instance.leftBoundary.position.x) ) {
		    	transform.Translate (horizonDist, 0, 0, Space.World);
		    }
            //vertical
            if ( charJumpState == JumpState.InAir ) {
                transform.Translate (0, verticalDist, 0);
            }
        }
        //update air to ground state
        if (charJumpState != JumpState.Ground) {
            if ( transform.position.y <= Game.instance.groundPosY ) {
                transform.position = new Vector3 (transform.position.x, 
                                              Game.instance.groundPosY, transform.position.z);
                charJumpState = JumpState.Ground;
                StartWalk();
            }
        }
	
	}
*/
	
	public void TurnRight() {
/*		if (charActionState == ActionState.Free || charActionState == ActionState.Jump) {
            if (charMoveState != MoveDir.Right) {
                Debug.Log("turning right!");
	    		charMoveState = MoveDir.Right;
//		    	transform.localScale = new Vector3(1,1,1);
                transform.localEulerAngles = new Vector3 (0, 0, 0);
		    } else {
                if (charJumpState == JumpState.Ground) {
                    //get into dash state
                    Debug.Log("dashing right!");
                    charActionState = ActionState.Dash;
                    GoDash(charMoveState);
                }
            }
        }
        */

	}
	
	public void TurnLeft() {
/*        if (charActionState == ActionState.Free || charActionState == ActionState.Jump) {
    		if (charMoveState != MoveDir.Left) {
                Debug.Log("turning left!");
	    		charMoveState = MoveDir.Left;
//		    	transform.localScale = new Vector3(-1,1,1);
                transform.localEulerAngles = new Vector3(0, 180, 0);
		    } else {
                if (charJumpState == JumpState.Ground) {
                    //get into dash state
                    Debug.Log("dashing left!");
                    GoDash(charMoveState);
                }
            }
        }
        */
	}

    public void GoDash(MoveDir _moveDir) {
        charActionState = ActionState.Dash;
        velocity.x = velocity.x + 275.0f;
        if (!animation.IsPlaying("lancer_dash")) {
            animation.Play("lancer_dash");
        }
        spFX.spanim.Play("speedline");
        Invoke("StopDash", dashDuration);
    }

    public void StopDash() {
        charActionState = ActionState.Recover;
        velocity.x = initMoveSpeed;
        if (!animation.IsPlaying("lancer_dash_recover")) {
            animation.Play("lancer_dash_recover");
        }
        float waitTime = animation["lancer_dash_recover"].length;
        spFX.spanim.Stop();
        Invoke("StartWalk", waitTime);
    }
 
    
    public void OnDamagePlayer (bool _isHurtFromLeft, int _damageAmount) {
        if (charHurtState == HurtState.Hitable) {
            Game.instance.OnPlayerHPChange(-_damageAmount);
            StartCoroutine(OnPlayerHurt(_isHurtFromLeft));
        }
    }	

	public IEnumerator OnPlayerHurt(bool _isHurtFromLeft) {
		Debug.Log("Calling OnPlayerHurt");
		if (charHurtState == HurtState.Hitable) {
			charHurtState = HurtState.Stun;
			//playing hurt flash effect
			foreach (exSprite sprite in allBodyParts) {
				sprite.spanim.Play("flash");
			}

            if (charJumpState == JumpState.Ground) {
                if (_isHurtFromLeft) {
                    //push player away a little bit: magic number
                    transform.Translate(40.0f, 0, 0);
                } else {
                    transform.Translate(-40.0f, 0, 0);
                }
            }
			//Debug.Log("playing hurt animation");
            yield return new WaitForSeconds(stunTime);
            charHurtState = HurtState.Invincible;
            if (flashTime > stunTime) {
    			yield return new WaitForSeconds(flashTime - stunTime);
            }
			charHurtState = HurtState.Hitable;	
		}
	}

    
	public void StartWalk() {
        /*
        charActionState = ActionState.Free;
		if (!animation.IsPlaying("walk")) {
			animation.Play("walk");
		}
        */
	}
	
	public void StartJump() {
        /*
        if (charActionState == ActionState.Free) {
            charActionState = ActionState.Jump;
            if (charJumpState == JumpState.Ground) {
                charJumpState = JumpState.Ready2Jump;
            }
        }
        */
	}


	public void OnJumpFinish() {
       // animation.Stop();
       // charJumpState = JumpState.Ground;
       // transform.position = new Vector3(transform.position.x, Game.instance.groundPosY,
       //                                  transform.position.z);
	   // StartWalk();
	}
}
