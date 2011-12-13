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
    public ProgressBar HPbar;
    public ProgressBar EXPbar;
    public exSpriteFont playerLvlDisplay;


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
        HPbar.ratio = 1.0f;
        EXPbar.ratio = 0.0f;
    }

}
