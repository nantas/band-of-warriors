// ======================================================================================
// File         : ArcherController.cs
// Author       : nantas 
// Last Change  : 12/30/2011 | 22:04:38 PM | Monday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;


[System.Serializable]
public class ComboEffectArcher {
    public int reqComboHit;
    public float newMoveSpeed;
    public int newMaxArrowCount;
    public float chanceToGetMoreLoot;
}

public class ArcherController: WarriorController {

    public float charge1Speed = 180.0f;
    public float charge2Speed = 130.0f;
    public float charge3Speed = 100.0f;
    public int maxArrowCount = 3;
    public Transform shootAnchor;
    [System.NonSerialized]public Spawner_Arrow arrowSpawner;
    public ComboEffectArcher[] comboEffect;

    void Start() {
        arrowSpawner = GetComponent<Spawner_Arrow>();
		animation["double_jump"].speed = 1.5f;
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
        maxArrowCount = comboEffect[comboLevel].newMaxArrowCount;
        velocity.x = initMoveSpeed;
        curAddLootChance = (int) (comboEffect[comboLevel].chanceToGetMoreLoot * 100);
        //TODO add emitter effect control
        player.OnComboTrailUp(comboLevel);
    }

    public void OnComboEffectDown() {
        initMoveSpeed = comboEffect[comboLevel].newMoveSpeed;
        maxArrowCount = comboEffect[comboLevel].newMaxArrowCount;
        velocity.x = initMoveSpeed;
        curAddLootChance = 0;
        //TODO add emitter effect control
        player.OnComboTrailEnd();
    }
        

    public override void ReleaseCharge(BtnHoldState _upButton) {
        if (FSM_Charge.ActiveStateName == "Charge_Prepare") {
            FSM_Charge.Fsm.Event("To_StopCharge");
            downButton = BtnHoldState.None;
            return;
        }
        if (FSM_Charge.ActiveStateName == "Charge_Up") {
            if (_upButton == downButton) {
                FSM_Control.Fsm.Event("To_ShootUp");
            } 
            FSM_Charge.Fsm.Event("To_StopCharge");
            downButton = BtnHoldState.None;
            return;
        }
        if (FSM_Charge.ActiveStateName == "Charge_Para") {
            if (_upButton == downButton) {
                FSM_Control.Fsm.Event("To_ShootPara");
            }
            FSM_Charge.Fsm.Event("To_StopCharge");
            downButton = BtnHoldState.None;
            return;
        }
        if (FSM_Charge.ActiveStateName == "Charge_Power") {
            if (_upButton == downButton) {
                FSM_Control.Fsm.Event("To_ShootPower");
            }
            FSM_Charge.Fsm.Event("To_StopCharge");
            downButton = BtnHoldState.None;
            return;
        }

    
    }

	void Update () {
        //check if player can give input
        if ( FSM_Control.FsmVariables.GetFsmBool("isAcceptInput").Value == true ) {
            //handle Input button down
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

	public override void TurnRight() {
        downButton = BtnHoldState.Right;
        FSM_Charge.Fsm.Event("To_ChargePrepare");
        lastBtnDownTime = Time.time;
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
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    //get into dash state
                    FSM_Control.Fsm.Event("To_Shoot");
                }
            } else if (FSM_Control.ActiveStateName == "Jump" ) {
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    FSM_Control.Fsm.Event("To_ShootInAir");
                }
            }
        }
	}
	
	public override void TurnLeft() {
        downButton = BtnHoldState.Left;
        FSM_Charge.Fsm.Event("To_ChargePrepare");
        lastBtnDownTime = Time.time;
        if (charMoveDir == MoveDir.Left) {
            if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
                //get to shoot state
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    FSM_Control.Fsm.Event("To_Shoot");
                }
            } else if (FSM_Control.ActiveStateName == "Jump" ) {
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    FSM_Control.Fsm.Event("To_ShootInAir");
                }
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

	public override void StartJump() {
        downButton = BtnHoldState.Jump;
        if ( FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == false ) {
            velocity.y = initJumpSpeed;
            FSM_Control.Fsm.Event("To_Jump");           
        } else  {
            FSM_Control.Fsm.Event("To_DoubleJump");
        }
	}

    public void Charge1MoveSpeed() {
        initMoveSpeed = charge1Speed;
        velocity.x = initMoveSpeed;
    }

    public void Charge2MoveSpeed() {
        initMoveSpeed = charge2Speed;
        velocity.x = initMoveSpeed;
    }

    public void Charge3MoveSpeed() {
        initMoveSpeed = charge3Speed;
        velocity.x = initMoveSpeed;
    }

    public void RestoreMoveSpeed() {
        initMoveSpeed = 200.0f;
        velocity.x = initMoveSpeed;
    }

    public void HorizontalShoot() {
        ShootArrow(false);
    }

    public void UpShoot() {
        if (arrowSpawner.aliveArrowCount < maxArrowCount) {
            Vector2 pos = new Vector2(shootAnchor.position.x, shootAnchor.position.y);
            Arrow arrow = arrowSpawner.SpawnArrowAt(pos);
            arrow.controller = this;
            arrow.SetSpawner(arrowSpawner);
            arrow.fxArrow.emit = true;
            arrow.initSpeed = 1050.0f;
            arrow.arrowGravity = -50f;
            arrow.LaunchArrowAt(shootAnchor, true);
        }
    }

    public void ParaShoot() {
        if (arrowSpawner.aliveArrowCount < maxArrowCount) {
            Vector2 pos = new Vector2(shootAnchor.position.x, shootAnchor.position.y);
            Arrow arrow = arrowSpawner.SpawnArrowAt(pos);
            arrow.controller = this;
            arrow.SetSpawner(arrowSpawner);
            arrow.fxArrow.emit = true;
            arrow.initSpeed = 900.0f;
            arrow.arrowGravity = -25.0f;
            arrow.LaunchArrowAt(shootAnchor, true);
        }
    }


    public void PowerShoot() {
        Vector2 pos = new Vector2(shootAnchor.position.x, shootAnchor.position.y);
        Arrow arrow = arrowSpawner.SpawnArrowAt(pos);
        arrow.controller = this;
        arrow.SetSpawner(arrowSpawner);
        arrow.spCollider.radius = 30.0f;
        arrow.spCollider.height = 100.0f;
        arrow.spArrow.scale = new Vector2 (2, 2);
        arrow.fxArrow.emit = true;
        arrow.initSpeed = 1000.0f;
        arrow.isPenetrating = true;
        arrow.LaunchArrowAt(shootAnchor, false);
    }


    public void DoubleJump() {
        velocity.y = initJumpSpeed+100;
        velocity.x = initMoveSpeed + 100;
    }

    public void StopDoubleJump() {
        velocity.x = initMoveSpeed;
    }

    public void ShootArrow(bool _isArrowAffectedByGravity) {
        Vector2 pos = new Vector2(shootAnchor.position.x, shootAnchor.position.y);
        Arrow arrow = arrowSpawner.SpawnArrowAt(pos);
        arrow.controller = this;
        arrow.SetSpawner(arrowSpawner);
        arrow.LaunchArrowAt(shootAnchor, _isArrowAffectedByGravity);
    }   



    public override void OnDamagePlayer (bool _isHurtFromLeft, int _damageAmount) {
        if ( FSM_Hit.FsmVariables.GetFsmBool("isAcceptDamage").Value == true ) {
            Game.instance.OnPlayerHPChange(-_damageAmount);
            StartHurt(_isHurtFromLeft);
            Game.instance.theGamePanel.OnComboEnd();
            ReleaseCharge(BtnHoldState.None);
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
        } else if ( prevStateName == "Walk" || prevStateName.Contains("Shoot") || prevStateName.Contains("Charge") ) {
            FSM_Control.Fsm.Event("To_Walk");
        } else if ( prevStateName.Contains("Jump")) {	
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


