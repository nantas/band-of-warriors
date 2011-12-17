// ======================================================================================
// File         : playerController.cs
// Author       : nantas 
// Last Change  : 12/17/2011 | 14:41:16 PM | Saturday,December
// Description  : 
// ======================================================================================


///////////////////////////////////////////////////////////////////////////////
// class PlayerController 
// 
// Purpose: Handle input, control player speed and acceleration
// 
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public enum JumpState {
    Ready2Jump,
    Ground,
    InAir,
    StunInAir
}

public enum HurtState {
    Invincible,
    Stun,
    Hitable
}

public enum ActionState {
    Free,
    Jump,
    Dash,
    AirDash,
    Recover
}


public class PlayerController: MonoBehaviour {
    //temp variable from Game.instance
    public float gravity = -90f;
    public float leftBoundary = -300f;
    public float rightBoundary = 300f;
    public float groundPosY = -130f;

	public float initMoveSpeed = 120.0f;
    public float initJumpSpeed = 500.0f;
    private PlayMakerFSM playerFSM;
    public Vector2 velocity;


	[System.NonSerialized] public MoveDir charMoveState;
    //[System.NonSerialized] public JumpState charJumpState;
    //[System.NonSerialized] public HurtState charHurtState;
    //[System.NonSerialized] public ActionState charActionState;
    public ActionState charActionState;
    public HurtState charHurtState;
    public JumpState charJumpState;

    void Awake () {
        playerFSM = GetComponent<PlayMakerFSM>();
        velocity = new Vector2(0, 0);
    }
    
    void Start () {
        charMoveState = MoveDir.Stop;
        playerFSM.Fsm.Event("To_Walk");
    }

	void Update () {
		//handle input
        Debug.Log("update.");
		if ( Input.GetButtonDown("Right") ) {
            Debug.Log("press right.");
			TurnRight();
		}
		if ( Input.GetButtonDown("Left") ) {
            Debug.Log("press left.");
			TurnLeft();
		}
		if ( Input.GetButtonDown("Jump") ) {
			StartJump();
		}
            
		
        //handle Jump
        if (charJumpState == JumpState.Ready2Jump) {
            velocity.y = initJumpSpeed;
            charJumpState = JumpState.InAir;
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
            velocity.y += gravity;
            verticalDist = Time.deltaTime * velocity.y;
        }
		//move character
        if (charHurtState != HurtState.Stun){
    		//horizontal
            if ((transform.position.x + horizonDist < rightBoundary) 
	    		&& (transform.position.x + horizonDist > leftBoundary) ) {
		    	transform.Translate (horizonDist, 0, 0, Space.World);
		    }
            //vertical
            if ( charJumpState == JumpState.InAir ) {
                transform.Translate (0, verticalDist, 0);
            }
        }
        //update air to ground state
        if (charJumpState != JumpState.Ground) {
            if ( transform.position.y <= groundPosY ) {
                transform.position = new Vector3 (transform.position.x, 
                                              groundPosY, transform.position.z);
                charJumpState = JumpState.Ground;
                playerFSM.Fsm.Event("To_Walk");
            }
        }
	
	}


	public void TurnRight() {
		if (charActionState == ActionState.Free || charActionState == ActionState.Jump) {
            if (charMoveState != MoveDir.Right) {
                Debug.Log("turning right!");
	    		charMoveState = MoveDir.Right;
                transform.localEulerAngles = new Vector3 (0, 0, 0);
		    } else {
                if (charJumpState == JumpState.Ground) {
                    //get into dash state
                    Debug.Log("dashing right!");
                    playerFSM.Fsm.Event("To_Dash");
                }
            }
        }

	}
	
	public void TurnLeft() {
        if (charActionState == ActionState.Free || charActionState == ActionState.Jump) {
    		if (charMoveState != MoveDir.Left) {
                Debug.Log("turning left!");
	    		charMoveState = MoveDir.Left;
//		    	transform.localScale = new Vector3(-1,1,1);
                transform.localEulerAngles = new Vector3(0, 180, 0);
		    } else {
                if (charJumpState == JumpState.Ground) {
                    //get into dash state
                    Debug.Log("dashing left!");
                    playerFSM.Fsm.Event("To_Dash");
                }
            }
        }
	}

    public void StartWalk() {
        velocity.x = initMoveSpeed;
        playerFSM.Fsm.Event("To_Walk");
    }

	public void StartJump() {
        if (charActionState == ActionState.Free) {
            if (charJumpState == JumpState.Ground) {
                playerFSM.Fsm.Event("To_Jump");
            }
        }
	}

}


