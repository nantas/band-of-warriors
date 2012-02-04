// ======================================================================================
// File         : WarriorController.cs
// Author       : nantas 
// Last Change  : 12/26/2011 | 17:05:35 PM | Monday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

//check which button was previously pressed.
public enum BtnHoldState {
    Left,
    Right,
    Jump,
    None
}

public class WarriorController : MonoBehaviour {

    //horizontal move speed for character.
	public float moveSpeed = 175.0f;
    //vertical jump speed 
    public float jumpSpeed = 1200.0f;
    //vertical jump height
    public float jumpHeight = 200.0f;
    //display name for character.
    public string charName;
    //display class name for character.
    public string charClass;
    //attack damage for each time player weapon touches an enemy. 
    public int attackPower = 10;

    //the visual part of the player.
    [System.NonSerialized]public PlayerBase player; 
    //which direction the player is going to.
	[System.NonSerialized]public MoveDir charMoveDir;
    //playmaker state machine reference.
    [System.NonSerialized]public PlayMakerFSM FSM_Control;
    [System.NonSerialized]public PlayMakerFSM FSM_Hit;
    [System.NonSerialized]public PlayMakerFSM FSM_Charge;
    //current combo level for the character.
    [System.NonSerialized]public int comboLevel;
    //variable to store initial value of character attributes
    [System.NonSerialized]public float initInvincibleDuration;
    [System.NonSerialized]public float initJumpSpeedStatic;
    [System.NonSerialized]public float initMoveSpeedStatic;
    [System.NonSerialized]public int initAttackPowerStatic;
    //helper variable to compute loot drop rate easier.
    [System.NonSerialized]public float lootChanceBoostCombo;
    [System.NonSerialized]public float lootChanceBoostAttribute;
    //reference to the prefab exLayer component, to fix layer order when prefab flips.
    protected exLayer layer;
    //the last pressed button. to check charge states.
    protected BtnHoldState downButton;
    
    //velocity of the player. this is the main thing that handles movement.
    protected Vector2 velocity;


    protected virtual void Awake () {
        //get the reference of playmaker fsms.
        PlayMakerFSM[] fsms = GetComponents<PlayMakerFSM>();
        foreach (PlayMakerFSM fsm in fsms) {
            if (fsm.FsmName == "FSM_Control") {
                FSM_Control = fsm;
            }
            if (fsm.FsmName == "FSM_Hit") {
                FSM_Hit = fsm;
            }
            if (fsm.FsmName == "FSM_Charge") {
                FSM_Charge = fsm;
            }
        }
        //initialize key variables.
        player = transform.GetComponent<PlayerBase>();
        layer = GetComponent<exLayer>();
        velocity = new Vector2(0, 0);
        comboLevel = 0;
        charMoveDir = MoveDir.Stop;
        downButton = BtnHoldState.None;
        initJumpSpeedStatic = jumpSpeed;
        initMoveSpeedStatic = moveSpeed;
        initAttackPowerStatic = attackPower;
        initInvincibleDuration = FSM_Hit.FsmVariables.GetFsmFloat("varInvincibleDuration").Value;
 
    }

    //check if player is able to accept input, depending on fsm variable set.
    public bool isAcceptInput() {
        return FSM_Control.FsmVariables.GetFsmBool("isAcceptInput").Value;
    }

    //flip player without a button input. this is used in menu actions.
    public void GetFaceDirection() {
        if (Game.instance.thePlayer.playerController.charMoveDir == MoveDir.Left){
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            velocity.x = moveSpeed;
        } else {
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            velocity.x = moveSpeed;
        }
    }


       
    //virtual functions that will be override by child class.
    public virtual void OnComboHitUpdate(){
    }

    public virtual void OnDamagePlayer(bool _isHurtFromLeft, int _amount) {
    }

    public virtual void StartJump(){
    }

    public virtual void TurnLeft(){
    }

    public virtual void TurnRight(){
    }

    public virtual void ReleaseCharge(BtnHoldState _upButton) {
    }

    public virtual void OnCharacterAttributeUpdate() {
    }

    protected void OnStartJump() {
        Debug.Log("jump up.");
        Vector3 moveAmount = new Vector3(0, jumpHeight, 0);
        float moveTime = jumpHeight/jumpSpeed;
        iTween.Stop(gameObject);
        gameObject.MoveBy(moveAmount, moveTime, 0, 
                          EaseType.easeOutQuad, "StartFalling", gameObject);
        Debug.Log("jump up sent.");
    }

    protected void OnStartDoubleJump() {
        FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value = false;  
        Vector3 moveAmount = new Vector3(0, jumpHeight/1.5f, 0);
        float moveTime = jumpHeight/1.5f/jumpSpeed;
        iTween.Stop(gameObject);
        gameObject.MoveBy(moveAmount, moveTime, 0, EaseType.easeOutQuad, "StartFalling",
                          gameObject);
    }

    protected void StartFalling() {
        Debug.Log("start falling.");
        FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value = true;  
    }

    //push player back
    public void StartHurt(bool _isHurtFromLeft) {
        float pushAmount = 50.0f;
        Vector3 moveAmount = new Vector3(pushAmount, 0, 0);
        float moveTime = 0.05f;
        if (_isHurtFromLeft) {
            if (transform.position.x + pushAmount > Game.instance.rightBoundary.position.x) {
                float dist = Game.instance.rightBoundary.position.x - transform.position.x;
                moveAmount.x = dist;
            }
        } else {
            if (transform.position.x - pushAmount < Game.instance.leftBoundary.position.x) {
                float dist = Game.instance.leftBoundary.position.x - transform.position.x;
                moveAmount.x = dist;
            } else {
                moveAmount.x = -pushAmount;
            }
        }
        gameObject.MoveBy(moveAmount, moveTime, 0, EaseType.linear);
    }

}
