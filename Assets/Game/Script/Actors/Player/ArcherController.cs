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
    //the hit number required to get next combo level.
    public int reqComboHit;
    public float newMoveSpeed;
    //max arrow allowed to be on the field.
    public int newMaxArrowCount;
    //chance to get more loot.
    public float chanceToGetMoreLoot;
}

public class ArcherController: WarriorController {

    //when in charge, the move speed difference.
    public float charge1Speed = -75.0f;
    public float charge2Speed = -100.0f;
    public float charge3Speed = -150.0f;
    public int maxArrowCount = 3;
    //how many jump in the air can perform.
    public int maxJumpCount = 1;
    //the bone anchor for arrow launch position and direction.
    public Transform shootAnchor;
    public ComboEffectArcher[] comboEffect;
    
    [System.NonSerialized]public Spawner_Arrow arrowSpawner;
    [System.NonSerialized]public float initChargeTime1Static;
    [System.NonSerialized]public float initChargeTime2Static;
    [System.NonSerialized]public float initChargeTime3Static;
    [System.NonSerialized]public int initMaxJumpCountStatic;
    

    //current jump in the air count before player reaches ground.
    private int currentJumpCount = 0;

    void Start() {
        arrowSpawner = GetComponent<Spawner_Arrow>();
        //speed up double jump animation.
		animation["double_jump"].speed = 1.5f;
        initChargeTime1Static = FSM_Charge.FsmVariables.GetFsmFloat("chargeTime1").Value;  
        initChargeTime2Static = FSM_Charge.FsmVariables.GetFsmFloat("chargeTime2").Value; 
        initChargeTime3Static = FSM_Charge.FsmVariables.GetFsmFloat("chargeTime3").Value; 
        initMaxJumpCountStatic = maxJumpCount;
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
        moveSpeed += comboEffect[comboLevel].newMoveSpeed;
        maxArrowCount = comboEffect[comboLevel].newMaxArrowCount;
        velocity.x = moveSpeed;
        lootChanceBoostCombo = comboEffect[comboLevel].chanceToGetMoreLoot;
        //TODO add emitter effect control
        player.OnComboTrailUp(comboLevel);
    }

    public void OnComboEffectDown() {
        //restore moveSpeed
        Game.instance.OnPlayerAttributeUpdate();
        maxArrowCount = comboEffect[comboLevel].newMaxArrowCount;
        velocity.x = moveSpeed;
        lootChanceBoostCombo = 0.0f;
        //TODO add emitter effect control
        player.OnComboTrailEnd();
    }
        

    //when a button is released, check if it's charged up enough.
    public override void ReleaseCharge(BtnHoldState _upButton) {
        //if charge is not ready, stop charge.
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

            //vertical movement
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
                    //reset jump count
                    currentJumpCount = 0;
                } else {
                    //player dead
                    PlayerDead();
                }
            }
        } 
    }

    //handles player input when accepted.
	public override void TurnRight() {
        string curState = FSM_Control.ActiveStateName;
        downButton = BtnHoldState.Right;
        //if player moves toward the same direction, enter charge state.
        FSM_Charge.Fsm.Event("To_ChargePrepare");
        //if player in idle state, start moving.
        if (charMoveDir == MoveDir.Stop) {
            charMoveDir = MoveDir.Right;
            //turn the prefab toward right, and update layer order. 
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            layer.Dirty();
            //set velocity to initMoveSpeed and enter walk state.
            velocity.x = moveSpeed;
            FSM_Control.Fsm.Event("To_Walk");
            //if player moves toward left, turn around to the right.
        } else if (charMoveDir == MoveDir.Left) {
            charMoveDir = MoveDir.Right;
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            layer.Dirty();
        } else if (charMoveDir == MoveDir.Right) {
            //if player is not in the air, do shoot
            if ( curState == "Walk" ) {
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    //get into dash state
                    FSM_Control.Fsm.Event("To_Shoot");
                }
                //if player is in the air, do shoot in the air.
            } else if ( curState == "Jump" || curState == "Double_Jump" || curState == "Jump_Falling" ) {
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    FSM_Control.Fsm.Event("To_ShootInAir");
                }
            }
        }
	}
	
	public override void TurnLeft() {
        string curState = FSM_Control.ActiveStateName;
        downButton = BtnHoldState.Left;
        FSM_Charge.Fsm.Event("To_ChargePrepare");
        if (charMoveDir == MoveDir.Left) {
            if ( curState == "Walk" ) {
                //get to shoot state
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    FSM_Control.Fsm.Event("To_Shoot");
                }
            } else if ( curState == "Jump" || curState == "Double_Jump" || curState == "Jump_Falling" ) {
                if (arrowSpawner.aliveArrowCount < maxArrowCount) {
                    FSM_Control.Fsm.Event("To_ShootInAir");
                }
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
        string curState = FSM_Control.ActiveStateName;
        downButton = BtnHoldState.Jump;
        //if player is on the ground, start jump.
        if ( curState == "Walk" || curState == "Idle" ) {
            velocity.y = 0;
            OnStartJump();
            FSM_Control.Fsm.Event("To_Jump");           
            currentJumpCount += 1;
        } else if (curState == "Jump" || curState == "Jump_Falling" ) {
            //if player is in the air, do double jump if jump count allows.
            if (currentJumpCount < maxJumpCount) {
                velocity.y = 0;
                OnStartDoubleJump();
                //FSM_Control.Fsm.Event("To_Jump");
                currentJumpCount += 1;
            }
        }
	}

    //update moveSpeed in charge state.
    public void Charge1MoveSpeed() {
        velocity.x = moveSpeed + charge1Speed;
    }

    public void Charge2MoveSpeed() {
        velocity.x = moveSpeed + charge2Speed;
    }

    public void Charge3MoveSpeed() {
        velocity.x = moveSpeed + charge3Speed;
    }

    public void RestoreMoveSpeed() {
        velocity.x = moveSpeed;
    }

    //shoot arrow without gravity.
    public void HorizontalShoot() {
        ShootArrow(false);
    }

    //shoot arrow upwards, with override gravity and initial speed.
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

    //shoot arrow in parabola, with override gravity and initial speed.
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


    //shoot arrow horizontally with penetrating property and enlarged hit box.
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


    //jump in the air with magic number speed.
    public void DoubleJump() {
        velocity.y = 0;
        velocity.x = moveSpeed + 100;
    }

    //restore horizontal movespeed at the end of double jump.
    public void StopDoubleJump() {
        velocity.x = moveSpeed;
    }

    //launch an arrow from shootAnchor. you can choose to give arrow gravity or not.
    public void ShootArrow(bool _isArrowAffectedByGravity) {
        Vector2 pos = new Vector2(shootAnchor.position.x, shootAnchor.position.y);
        Arrow arrow = arrowSpawner.SpawnArrowAt(pos);
        arrow.controller = this;
        arrow.SetSpawner(arrowSpawner);
        arrow.LaunchArrowAt(shootAnchor, _isArrowAffectedByGravity);
    }   

    //when player body collide with enemy
    public override void OnDamagePlayer (bool _isHurtFromLeft, int _damageAmount) {
        if ( FSM_Hit.FsmVariables.GetFsmBool("isAcceptDamage").Value == true ) {
            Game.instance.OnPlayerHPChange(-_damageAmount);
            StartHurt(_isHurtFromLeft);
            Game.instance.theGamePanel.OnComboEnd();
            ReleaseCharge(BtnHoldState.None);
            FSM_Control.Fsm.Event("To_Stun_Ctrl");
        }
    }	

    //when player gets out of stun state, go back to a proper state according to where he was.
    public void OnStunExit() {
        string prevStateName = FSM_Control.FsmVariables.GetFsmString("PrevStateName").Value;
        if ( prevStateName == "Idle" ) {
			FSM_Control.Fsm.Event("To_Idle");
        } else if ( prevStateName == "Walk" || 
                    (prevStateName.Contains("Shoot") && prevStateName != "Shoot_InAir")
                    || prevStateName.Contains("Charge") ) {
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

    public override void OnCharacterAttributeUpdate() {
        //ATTR: att_chargeTimeReduction multiplier
        float chargeTimeReduction = 2.0f - player.charBuild.GetAttributeEffectMultiplier("att_chargeTimeReduction");
        FSM_Charge.FsmVariables.GetFsmFloat("chargeTime1").Value = initChargeTime1Static * chargeTimeReduction;
        FSM_Charge.FsmVariables.GetFsmFloat("chargeTime2").Value = initChargeTime2Static * chargeTimeReduction;
        FSM_Charge.FsmVariables.GetFsmFloat("chargeTime3").Value = initChargeTime3Static * chargeTimeReduction;

        //ATTR: att_jumpCountBoost multiplier
        int jumpCountBoost = (int)player.charBuild.GetAttributeEffectMultiplier("att_jumpCountBoost");
        maxJumpCount = jumpCountBoost;
    }

}


