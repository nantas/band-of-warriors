// ======================================================================================
// File         : BtnLoadPuzzle.cs
// Author       : Wu Jie 
// Last Change  : 10/31/2011 | 18:01:05 PM | Monday,October
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
///
/// Button Start Game
///
///////////////////////////////////////////////////////////////////////////////

public class BtnCharSelect : MonoBehaviour {

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	public int charIndex;
    public PlayerBase nextChar;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	void Awake () {
        exUIButton uiButton = GetComponent<exUIButton>();
        uiButton.OnButtonPress += OnButtonPress;
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonPress () {
        if (charIndex != Game.instance.curCharIndex && 
            Game.instance.thePlayer.playerController.FSM_Control.ActiveStateName == "Walk") {
            //switch character index
            Game.instance.curCharIndex = charIndex;
            Vector3 ringOutPos = nextChar.transform.position;
            nextChar.transform.position = Game.instance.thePlayer.transform.position;
            Game.instance.thePlayer.transform.position = ringOutPos;
            MoveDir curMoveDir = Game.instance.thePlayer.playerController.charMoveDir;
            Game.instance.thePlayer.playerController.FSM_Control.Fsm.Event("To_Idle");
            Game.instance.thePlayer.playerController.enabled = false;
            //switch
            Game.instance.thePlayer = nextChar;
            Camera.main.GetComponent<CameraFollow>().target = nextChar.transform;
            Game.instance.thePlayer.playerController.enabled = true;
            Game.instance.thePlayer.playerController.charMoveDir = curMoveDir;
            Game.instance.thePlayer.playerController.GetFaceDirection();
            Game.instance.theGamePanel.ChangeNameDisplay();
            Game.instance.thePlayer.playerController.FSM_Control.Fsm.Event("To_Walk");
         }
    }
	
}

