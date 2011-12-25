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
    public ComboDisplay comboDisplay;
    public ProgressBar HPbar;
    public ProgressBar EXPbar;
    public exSpriteFont playerLvlDisplay;
    public exUIPanel panelGameOver;

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

    public void ShowLevelUpText() {
        levelUpText.animation.Play("txt_levelUp");
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
        }
    }



}
