using UnityEngine;
using System.Collections;

public class WarriorControl : MonoBehaviour {

	public float movementSpeed = 120.0f;
	public exSprite[] allBodyParts;
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

	[System.NonSerialized] public MoveDir charMoveState;
    [System.NonSerialized] public JumpState charJumpState;
    [System.NonSerialized] public HurtState charHurtState;
	private float flashTime = 0.0f;
    private float stunTime = 0.1f;
	  
	void Awake () {
		flashTime = allBodyParts[0].spanim.animations[0].length;
	}
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(1,1,1);
		charMoveState = MoveDir.Right;
        charJumpState = JumpState.Ground;
        charHurtState = HurtState.Hitable;
		animation["walk"].speed = movementSpeed/120.0f;
		StartWalk();
		
	}
	
	// Update is called once per frame
	void Update () {
		//handle input
		if ( Input.GetButton("Right")) {
			TurnRight();
		}
		if ( Input.GetButton("Left")) {
			TurnLeft();
		}
		if ( Input.GetButton("Jump") ) {
			StartJump();
		}
		
        //handle Jump
        if (charJumpState == JumpState.Ready2Jump) {
            if (!animation.IsPlaying("jump"))
                animation.Play("jump");
            charJumpState = JumpState.InAir;
        }
		//handle movement
		float horizonDist = Time.deltaTime * movementSpeed;		
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
		//move character
        if (charHurtState != HurtState.Stun){
    		if ((transform.position.x + horizonDist < Game.instance.rightBoundary.position.x) 
	    		&& (transform.position.x + horizonDist > Game.instance.leftBoundary.position.x) ) {
		    	transform.Translate (horizonDist, 0, 0);
		    }
        }
	
	}
	
	public void TurnRight() {
		if (charMoveState != MoveDir.Right) {
			charMoveState = MoveDir.Right;
			transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void TurnLeft() {
		if (charMoveState != MoveDir.Left) {
			charMoveState = MoveDir.Left;
			transform.localScale = new Vector3(-1,1,1);
		}
	}
	
	public IEnumerator OnPlayerHurt(bool _isHurtFromLeft) {
		//Debug.Log("Calling OnPlayerHurt");
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
		if (!animation.IsPlaying("walk")) {
			animation.Play("walk");
		}
	}
	
	public void StartJump() {
        if (charJumpState == JumpState.Ground) {
            charJumpState = JumpState.Ready2Jump;
        }
    
	}
	
	public void OnJumpFinish() {
        animation.Stop();
        charJumpState = JumpState.Ground;
        transform.position = new Vector3(transform.position.x, Game.instance.groundPosY,
                                         transform.position.z);
		StartWalk();
	}
}
