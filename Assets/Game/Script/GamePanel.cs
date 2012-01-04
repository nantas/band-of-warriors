// ======================================================================================
// File         : GamePanel.cs
// Author       : Wu Jie 
// Last Change  : 10/31/2011 | 20:22:27 PM | Monday,October
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// class GamePanel 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class GamePanel : MonoBehaviour {

    public exSpriteFont scoreDisplay; 
    public exSpriteFont levelUpText; 
    public exSpriteFont curCharName;
    public exSpriteFont curCharClass;
    public ComboDisplay comboDisplay;
    public exSpriteFont stageDisplay;
    public ProgressBar HPbar;
    public ProgressBar EXPbar;
    public exSprite missionTargetIcon;
    public exSpriteFont missionInfoText;
    public exSpriteFont playerLvlDisplay;
    public exUIPanel panelGameOver;
    public exSprite[] charSlots;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void Awake () {
        //ScoreCounter scoreCounter = Game.instance.scoreCounter;
        scoreDisplay.text = "$" + "0";
        playerLvlDisplay.text = "lv" + (Game.instance.playerLvl+1);
        //hide combo display
        comboDisplay.transform.Translate(0, 800, 0);
        comboDisplay.enabled = false;
        HPbar.ratio = 1.0f;
        EXPbar.ratio = 0.0f;
    }

    public void ShowGameOver () {
        panelGameOver.enabled = true;
        Transform trans = panelGameOver.transform;
        trans.position = new Vector3 (Camera.main.transform.position.x, 
                                      0.0f, trans.position.z);
    }

    public void ChangeNameDisplay() {
        curCharName.text = Game.instance.thePlayer.playerController.charName;
        curCharClass.text = Game.instance.thePlayer.playerController.charClass;
    }


    public void ShowLevelUpText() {
        levelUpText.animation.Play("txt_levelUp");
    }

    public void OnStageUpdate(int _section) {
        stageDisplay.text = "stage 1 - " + (_section + 1);
    }
    
    public void OnComboUpdate() {
        if (comboDisplay.enabled == false) {
            comboDisplay.enabled = true;
            comboDisplay.transform.Translate(0, -800, 0);
        }
        comboDisplay.HitAnEnemy();
    }

    public void OnComboEnd() {
        if (comboDisplay.enabled == true) {
            comboDisplay.transform.Translate(0, 800, 0);
            comboDisplay.enabled = false;
            Game.instance.thePlayer.playerController.SendMessage("OnComboHitUpdate", 0, 
                                                                 SendMessageOptions.DontRequireReceiver);
        }
    }

    public void SlotUpdate(int _charIndex) {
        foreach (exSprite slot in charSlots) {
            slot.spanim.Play("slot_inactive");
        }
        charSlots[_charIndex-1].spanim.Play("slot_active");
    }

    public void OnMissionUpdate(EnemyClass _targetType, int _numToKill) {
        if (_numToKill < 0) _numToKill = 0;
        string iconName = _targetType.ToString().ToLower();
        missionTargetIcon.spanim.Play(iconName);
        missionInfoText.text = "to kill: " + _numToKill;
    }



}
