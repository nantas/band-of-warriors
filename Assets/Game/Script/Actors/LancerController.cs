// ======================================================================================
// File         : LancerController.cs
// Author       : nantas 
// Last Change  : 12/19/2011 | 22:04:38 PM | Monday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

[System.Serializable]
public class ComboEffect {
    public int reqComboHit;
    public float newMoveSpeed;
    public float newDashSpeed;
    public float chanceToGetMoreLoot;
}

public class LancerController: MonoBehaviour {

	public float initMoveSpeed = 250.0f;
    public float initJumpSpeed = 1250.0f;
    public float dashSpeed = 450.0f;
    public ComboEffect[] comboEffect;

    [System.NonSerialized]public PlayerBase player; 
	[System.NonSerialized]public MoveDir charMoveDir;
    [System.NonSerialized]public PlayMakerFSM FSM_Control;
    [System.NonSerialized]public PlayMakerFSM FSM_Hit;
    [System.NonSerialized]public int comboLevel;
    public int curAddLootChance;

    private Vector2 velocity;
    

    void Awake () {
        PlayMakerFSM[] fsms = GetComponents<PlayMakerFSM>();
        foreach (PlayMakerFSM fsm in fsms) {
            if (fsm.FsmName == "FSM_Control") {
                FSM_Control = fsm;
            }
            if (fsm.FsmName == "FSM_Hit") {
                FSM_Hit = fsm;
            }
        }
        player = transform.GetComponent<PlayerBase>();
        velocity = new Vector2(0, 0);
        comboLevel = 0;
        charMoveDir = MoveDir.Stop;
    }

    public bool isAcceptInput() {
        return FSM_Control.FsmVariables.GetFsmBool("isAcceptInput").Value;
    }

    public void OnComboHitUpdate(int _comboHit) {
        if (_comboHit >= comboEffect[comboLevel].reqComboHit) {
            comboLevel += 1;
            OnComboEffectUp();
        }
        if (_comboHit == 0) {
            comboLevel = 0;
            OnComboEffectDown();
        }
    }

    public void OnComboEffectUp() {
        initMoveSpeed = comboEffect[comboLevel].newMoveSpeed;
        dashSpeed = comboEffect[comboLevel].newDashSpeed;
        velocity.x = initMoveSpeed;
        curAddLootChance = (int) (comboEffect[comboLevel].chanceToGetMoreLoot * 100);
        //TODO add emitter effect control
        player.OnComboTrailUp(comboLevel);
    }

    public void OnComboEffectDown() {
        initMoveSpeed = comboEffect[comboLevel].newMoveSpeed;
        dashSpeed = comboEffect[comboLevel].newDashSpeed;
        velocity.x = initMoveSpeed;
        curAddLootChance = 0;
        //TODO add emitter effect control
        player.OnComboTrailEnd();
    }
        


	void Update () {
        //check if player can give input
        if ( FSM_Control.FsmVariables.GetFsmBool("isAcceptInput").Value == true ) {
            //handle Input
            if ( Input.GetButtonDown("Right") ) {
                TurnRight();
            }
            if ( Input.GetButtonDown("Left") ) {
                TurnLeft();
            }
            if ( Input.GetButtonDown("Jump") ) {
                StartJump();
            }
        }

		//handle movement
		float horizonDist = Time.deltaTime * velocity.x;
        float verticalDist = 0;
        //check movement direction
		switch (charMoveDir) {
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
        //prepare vertical movement
        if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == true ) {
            velocity.y += Game.instance.gravity;
            verticalDist = Time.deltaTime * velocity.y;
        }
		//move character
        if ( FSM_Hit.FsmVariables.GetFsmBool("isPlayerStun").Value == false ){
    		//horizontal
		    transform.Translate (horizonDist, 0, 0, Space.World);
            //make sure player will not get out of bounds
            if ( transform.position.x + horizonDist > Game.instance.rightBoundary.position.x ) {
                transform.position = new Vector3 ( Game.instance.rightBoundary.position.x,
                                                   transform.position.y, transform.position.z);
            }
            if ( transform.position.x + horizonDist < Game.instance.leftBoundary.position.x ) {
	    		transform.position = new Vector3 ( Game.instance.leftBoundary.position.x,
                                                   transform.position.y, transform.position.z);
		    }

            //vertical
            if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == true  ) {
                transform.Translate (0, verticalDist, 0);
            }
        }

        //update air to ground state
        if (FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == true) {
            if ( transform.position.y <= Game.instance.groundPosY ) {
                transform.position = new Vector3 (transform.position.x, 
                                              Game.instance.groundPosY, transform.position.z);
                //handle gameover
                if (FSM_Control.FsmVariables.GetFsmBool("isPlayerNoHP").Value == false) {
                    FSM_Control.Fsm.Event("To_Walk");
                } else {
                    //player dead
                    PlayerDead();
                }
            }
        } 

    }

	public void TurnRight() {
        if (charMoveDir == MoveDir.Stop) {
            charMoveDir = MoveDir.Right;
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            velocity.x = initMoveSpeed;
            FSM_Control.Fsm.Event("To_Walk");
        } else if (charMoveDir == MoveDir.Left) {
            charMoveDir = MoveDir.Right;
            transform.localEulerAngles = new Vector3 (0, 0, 0);
        } else if (charMoveDir == MoveDir.Right) {
            if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
            //get into dash state
            velocity.x = dashSpeed;
            FSM_Control.Fsm.Event("To_Dash");
            }
        }
	}
	
	public void TurnLeft() {
        if (charMoveDir == MoveDir.Left) {
            if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
                //get into dash state
                velocity.x = dashSpeed;
                FSM_Control.Fsm.Event("To_Dash");
            }
        }
        if (charMoveDir == MoveDir.Stop) {
            charMoveDir = MoveDir.Left;
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            velocity.x = initMoveSpeed;
            FSM_Control.Fsm.Event("To_Walk");
        } 
        if (charMoveDir == MoveDir.Right) {
            charMoveDir = MoveDir.Left;
            transform.localEulerAngles = new Vector3 (0, 180, 0);
        }
	}

	public void StartJump() {
        if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
            velocity.y = initJumpSpeed;
            FSM_Control.Fsm.Event("To_Jump");           
        } else  {
            velocity.x = dashSpeed;
            FSM_Control.Fsm.Event("To_AirDash");
        }
	}

    public void StopDash() {
        velocity.x = initMoveSpeed;
    }

    public void OnDamagePlayer (bool _isHurtFromLeft, int _damageAmount) {
        if ( FSM_Hit.FsmVariables.GetFsmBool("isAcceptDamage").Value == true ) {
            Game.instance.OnPlayerHPChange(-_damageAmount);
            StartHurt(_isHurtFromLeft);
            Game.instance.theGamePanel.OnComboEnd();
            FSM_Control.Fsm.Event("To_Stun_Ctrl");
        }
    }	

    //push player back
    public void StartHurt(bool _isHurtFromLeft) {
        if (_isHurtFromLeft) {
            transform.Translate(30.0f, 0, 0, Space.World);
            if (transform.position.x > Game.instance.rightBoundary.position.x) {
                transform.position = new Vector3(Game.instance.rightBoundary.position.x,
                                                 transform.position.y, transform.position.z);
            }
        } else {
            transform.Translate(-30.0f, 0, 0, Space.World);
            if (transform.position.x < Game.instance.leftBoundary.position.x) {
                transform.position = new Vector3(Game.instance.leftBoundary.position.x,
                                                 transform.position.y, transform.position.z);
            }
        }
    }

    public void OnStunExit() {
        string prevStateName = FSM_Control.FsmVariables.GetFsmString("PrevStateName").Value;
        if ( prevStateName == "Idle" ) {
			FSM_Control.Fsm.Event("To_Idle");
        } else if ( prevStateName == "Walk" || prevStateName == "Dash" || prevStateName == "Dash_Recover" ) {
            FSM_Control.Fsm.Event("To_Walk");
        } else if ( prevStateName == "Jump" || prevStateName == "AirDash" || prevStateName == "Jump_NoMove" ) {	
            FSM_Control.Fsm.Event("To_JumpNoMove");
		}
    }

    public void OnPlayerNoHP() {
        Debug.Log("player has no hp. go to dead state.");
        if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == true ) {
            Debug.Log("goto dead in air state");
            FSM_Control.Fsm.Event("To_DeadDrop");
        } else {
            Debug.Log("goto dead on ground state");
            FSM_Control.Fsm.Event("To_DeadGround");
        }
    }

    public void PlayerDead() {
        velocity = new Vector2 (0, 0);
        Game.instance.AcceptInput(false);
        Game.instance.theGamePanel.ShowGameOver();
        this.enabled = false;
    }

}

