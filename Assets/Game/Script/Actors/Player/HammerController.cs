// ======================================================================================
// File         : HammerController.cs
// Author       : nantas 
// Last Change  : 12/19/2011 | 22:04:38 PM | Monday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

[System.Serializable]
public class ComboEffectHam {
    public int reqComboHit;
    public float newMoveSpeed;
    //increase attack damage
    public int newAttackPower;
    public float chanceToGetMoreLoot;
}

public class HammerController: WarriorController {

    //initial attack damage. not in use yet.
    public int attackPower = 2;
    public ComboEffectHam[] comboEffect;


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

    void Start () {
        //changed uppercut animation speed.
		animation["uppercut"].speed = 2.0f;
    }

    public void OnComboEffectUp() {
        moveSpeed += comboEffect[comboLevel].newMoveSpeed;
        attackPower = comboEffect[comboLevel].newAttackPower;
        velocity.x = moveSpeed;
        curAddLootChance = (int) (comboEffect[comboLevel].chanceToGetMoreLoot * 100);
        //TODO add emitter effect control
        player.OnComboTrailUp(comboLevel);
    }

    public void OnComboEffectDown() {
        //restore moveSpeed
        Game.instance.OnPlayerAttributeUpdate();
        attackPower = comboEffect[comboLevel].newAttackPower;
        velocity.x = moveSpeed;
        curAddLootChance = 0;
        //TODO add emitter effect control
        player.OnComboTrailEnd();
    }
        
    public override void ReleaseCharge(BtnHoldState _upButton) {
        //only enters charge release state when it's already in charge loop state.
        if (FSM_Charge.ActiveStateName == "Charge_Loop_Armor") {
            if (_upButton == downButton) {
                FSM_Control.Fsm.Event("To_ChargeRelease");
                FSM_Charge.Fsm.Event("To_ChargeRelease");
            } 
            FSM_Charge.Fsm.Event("To_StopCharge");
            downButton = BtnHoldState.None;
            return;
        } else {
            downButton = BtnHoldState.None;
            FSM_Charge.Fsm.Event("To_StopCharge");
        }
    }

	void LateUpdate () {
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
              //handle input button up
        if ( Input.GetButtonUp("Right")) {
            ReleaseCharge(BtnHoldState.Right);
        }
        if ( Input.GetButtonUp("Left")) {
            ReleaseCharge(BtnHoldState.Left);
        }
        if ( Input.GetButtonUp("Jump")) {
            ReleaseCharge(BtnHoldState.Jump);
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
            if ( FSM_Control.ActiveStateName == "AirAttack") {
                velocity.y += Game.instance.gravity * 2;
            } else {
                velocity.y += Game.instance.gravity;
            }
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
                if (FSM_Control.FsmVariables.GetFsmBool("isPlayerNoHP").Value == true ) {
                    //handle gameover
                    PlayerDead();
                } else if (FSM_Control.ActiveStateName == "AirAttack") {
                    //handles AirAttack landing specificaly
                    velocity.y = jumpSpeed/2 + 350;
                    transform.Translate(0, velocity.y * Time.deltaTime, 0);
                    FSM_Control.Fsm.Event("To_AirAttack_Recover");
                } else {
                    //Debug.Log("air recover to walk?");
                    FSM_Control.Fsm.Event("To_Walk");
                } 
            }
        } 

    }

	public override void TurnRight() {
        downButton = BtnHoldState.Right;
        FSM_Charge.Fsm.Event("To_ChargePrepare");   
        if (charMoveDir == MoveDir.Stop) {
            charMoveDir = MoveDir.Right;
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            layer.Dirty();
            velocity.x = moveSpeed;
            FSM_Control.Fsm.Event("To_Walk");
        } else if (charMoveDir == MoveDir.Left) {
            charMoveDir = MoveDir.Right;
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            layer.Dirty();
        } else if (charMoveDir == MoveDir.Right) {
            if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
            //get into uppercut
            velocity.x = moveSpeed;
            FSM_Control.Fsm.Event("To_Uppercut");
            }
        }
	}
	
	public override void TurnLeft() {
        downButton = BtnHoldState.Left;
        FSM_Charge.Fsm.Event("To_ChargePrepare");   
        if (charMoveDir == MoveDir.Left) {
            if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
                //get into dash state
                velocity.x = moveSpeed;
                FSM_Control.Fsm.Event("To_Uppercut");
            }
        }
        if (charMoveDir == MoveDir.Stop) {
            charMoveDir = MoveDir.Left;
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            layer.Dirty();
            velocity.x = moveSpeed;
            FSM_Control.Fsm.Event("To_Walk");
        } 
        if (charMoveDir == MoveDir.Right) {
            charMoveDir = MoveDir.Left;
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            layer.Dirty();
        }
	}

	public override void StartJump() {
        downButton = BtnHoldState.Jump;
        if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
            velocity.y = jumpSpeed;
            FSM_Control.Fsm.Event("To_Jump");           
        } else  {
            velocity.y = jumpSpeed;
            FSM_Control.Fsm.Event("To_AirAttack");
        }
	}

    public override void OnDamagePlayer (bool _isHurtFromLeft, int _damageAmount) {
        if ( FSM_Hit.FsmVariables.GetFsmBool("isAcceptDamage").Value == true ) {
            if (FSM_Charge.ActiveStateName.Contains("Armor") || FSM_Control.ActiveStateName.Contains("Armor")) {
                Game.instance.OnPlayerHPChange(-_damageAmount);
                player.OnHurtStart();
            } else {
                Game.instance.OnPlayerHPChange(-_damageAmount);
                StartHurt(_isHurtFromLeft);
                Game.instance.theGamePanel.OnComboEnd();
                FSM_Control.Fsm.Event("To_Stun_Ctrl");
            }
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
        } else if ( prevStateName == "Walk" || prevStateName == "Charge_Start" ) {
            FSM_Control.Fsm.Event("To_Walk");
        } else if ( prevStateName == "Uppercut" ) {
            FSM_Control.Fsm.Event("To_Walk" );
        } else if ( prevStateName == "Jump" || prevStateName == "AirAttack" || 
                    prevStateName == "AirAttack_Recover" || prevStateName == "Jump_NoMove" ) {	
            FSM_Control.Fsm.Event("To_JumpNoMove");
		}
    }

    public void OnPlayerNoHP() {
        //Debug.Log("player has no hp. go to dead state.");
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


