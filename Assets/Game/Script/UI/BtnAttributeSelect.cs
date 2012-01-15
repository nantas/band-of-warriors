// ======================================================================================
// File         : BtnAttributeSelect.cs
// Author       : nantas 
// Last Change  : 01/14/2012 | 21:31:02 PM | Saturday,January
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;


public class BtnAttributeSelect : MonoBehaviour {

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	public int attId;
    public int modifierLv;
    public exSpriteFont attributeName;
    public exSpriteFont attributeDesc;
    public exUIPanel attributeSelectPanel;

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
        CharacterBuild charBuild = Game.instance.thePlayer.GetComponent<CharacterBuild>();
        int targetAttributeIndex = charBuild.isAttributeLearned(attId);
        if (targetAttributeIndex == -1) {
            charBuild.LearnNewAttribute(attId);
        } else {
            charBuild.UpgradeAttributeAtIndex(targetAttributeIndex);
        }
        attributeSelectPanel.transform.position 
            = new Vector3 ( attributeSelectPanel.transform.position.x, 1500.0f, attributeSelectPanel.transform.position.z );
        attributeSelectPanel.enabled = false;
        Game.instance.AcceptInput(true);
        Time.timeScale = 1.0f;
    }

	
}


