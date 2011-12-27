// ======================================================================================
// File         : WarriorController.cs
// Author       : nantas 
// Last Change  : 12/26/2011 | 17:05:35 PM | Monday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class WarriorController : MonoBehaviour {

	public float initMoveSpeed = 175.0f;
    public float initJumpSpeed = 1200.0f;
    public string charName;
    public string charClass;
    [System.NonSerialized]public PlayerBase player; 
	[System.NonSerialized]public MoveDir charMoveDir;
    [System.NonSerialized]public PlayMakerFSM FSM_Control;
    [System.NonSerialized]public PlayMakerFSM FSM_Hit;
    [System.NonSerialized]public int comboLevel;
    public int curAddLootChance;

    protected Vector2 velocity;


    protected virtual void Awake () {
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

    public void GetFaceDirection() {
        if (Game.instance.thePlayer.playerController.charMoveDir == MoveDir.Left){
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            velocity.x = initMoveSpeed;
        } else {
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            velocity.x = initMoveSpeed;
        }
    }


       

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


}
