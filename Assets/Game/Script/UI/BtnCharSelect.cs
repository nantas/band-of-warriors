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
    public exUIPanel charSelectPanel;

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
        if (charIndex != Game.instance.curCharIndex) {
            //handle portrait change
            Game.instance.theGamePanel.SlotUpdate(charIndex);
            //switch character index
            Game.instance.curCharIndex = charIndex;
            //start switching character
            SwitchCharacter();

        }
        charSelectPanel.transform.position 
            = new Vector3 ( charSelectPanel.transform.position.x, 1500.0f, charSelectPanel.transform.position.z );
        charSelectPanel.enabled = false;
        Game.instance.AcceptInput(true);
        Time.timeScale = 1.0f;
    }

    void SwitchCharacter() {
        //switch position
        Vector3 ringOutPos = nextChar.transform.position;
        nextChar.transform.position = new Vector3(Game.instance.thePlayer.transform.position.x,
                                                  Game.instance.groundPosY, 
                                                  Game.instance.thePlayer.transform.position.z);
        Game.instance.thePlayer.transform.position = ringOutPos;
        //get the move direction.
        MoveDir curMoveDir = Game.instance.thePlayer.playerController.charMoveDir;
        //send the old character state to idle and disable the controller.
        Game.instance.thePlayer.playerController.FSM_Control.Fsm.Event("To_Idle");
        Game.instance.thePlayer.playerController.FSM_Control.enabled = false;
        Game.instance.thePlayer.playerController.FSM_Hit.enabled = false;
        Game.instance.thePlayer.playerController.FSM_Charge.enabled = false;
        Game.instance.thePlayer.playerController.enabled = false;
        //switch reference in game class to the new character
        Game.instance.thePlayer = nextChar;
        //switch camera target
        Camera.main.GetComponent<CameraFollow>().target = nextChar.transform;
        //enable control for the new character, and set its state to idle.
        Game.instance.thePlayer.playerController.enabled = true;
        Game.instance.thePlayer.playerController.FSM_Control.enabled = true;
        Game.instance.thePlayer.playerController.FSM_Hit.enabled = true;
        Game.instance.thePlayer.playerController.FSM_Charge.enabled = true;
        Game.instance.thePlayer.playerController.charMoveDir = curMoveDir;
        Game.instance.thePlayer.playerController.GetFaceDirection();
        Game.instance.thePlayer.playerController.charMoveDir = MoveDir.Stop;
        Game.instance.thePlayer.playerController.FSM_Control.Fsm.Event("To_Idle");
        //update name display and exp bar display
        Game.instance.theGamePanel.UpdateCharacterInfo();
        Game.instance.OnExpDisplayUpdate();
        //update attribute and related variable
        Game.instance.OnPlayerAttributeUpdate();
    }
	
}

