// ======================================================================================
// File         : ComboDisplay.cs
// Author       : nantas 
// Last Change  : 12/25/2011 | 15:19:25 PM | Sunday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;


///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class ComboDisplay : MonoBehaviour {

    public exSprite progressBar;
    public exSpriteFont comboText;
    public float addPerHit = 0.1f;
    public float subPerSec = 0.2f;
    [System.NonSerialized]public int comboHit;
    private float ratio;

    void Awake () {
        ratio = 0.0f;
        progressBar.scale = new Vector2 (ratio, 1);
        comboText.text = "";
        comboHit = 0;
    }

    void OnEnable () {
        ratio = 0.0f;
        progressBar.scale = new Vector2 (ratio, 1);
    }

    void OnDisable() {
        comboHit = 0;
    }

    public void HitAnEnemy() {
        ratio += addPerHit;
        if (ratio > 1.0f) ratio = 1.0f;
        comboHit += 1;
        Game.instance.thePlayer.playerController.SendMessage("OnComboHitUpdate",comboHit);
        comboText.text = "combo x" + comboHit;
    }

    void LateUpdate () {
        float subPerFrame = subPerSec * Time.deltaTime;
        ratio -= subPerFrame;
        if (ratio < 0.0f) ratio = 0.0f;
        progressBar.scale = new Vector2 (ratio, 1);
        //combo bar disappear
        if (ratio == 0.0f) {
            Game.instance.theGamePanel.OnComboEnd();
        }
    }

}



