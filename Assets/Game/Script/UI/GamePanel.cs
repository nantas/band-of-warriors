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
    public LevelUpPanel panelLevelUp;
    public exSprite charPortrait;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void Awake () {
        //ScoreCounter scoreCounter = Game.instance.scoreCounter;
        scoreDisplay.text = "$" + "0";
        playerLvlDisplay.text = "lv" + 
             Game.instance.thePlayer.GetComponent<CharacterBuild>().charLevel;
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

    public void UpdateCharacterInfo() {
        curCharName.text = Game.instance.thePlayer.playerController.charName;
        curCharClass.text = Game.instance.thePlayer.playerController.charClass;
        playerLvlDisplay.text = "lv" + Game.instance.thePlayer.charBuild.charLevel;
    }


    public void ShowLevelUpText() {
        levelUpText.animation.Play("txt_levelUp");
    }

    public void ShowLevelCompleteText() {
        levelUpText.text = "mission complete!";
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
        string animName = "icon_cp" + _charIndex;
        charPortrait.spanim.Play(animName);
    }
    

    public void OnMissionUpdate(EnemyClass _targetType, int _numToKill) {
        if (_numToKill < 0) _numToKill = 0;
        string iconName = _targetType.ToString().ToLower();
        missionTargetIcon.spanim.Play(iconName);
        missionInfoText.text = "to kill: " + _numToKill;
    }



}
