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

    //id and modifierLv for the attribute to learn
	public int attId;
    public int modifierLv;
    //name and description display 
    public exSpriteFont attributeName;
    public exSpriteFont attributeDesc;
    //LevelUpPanel
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
        //check if it's a new attribute or existing one.
        int targetAttributeIndex = charBuild.isAttributeLearned(attId);
        //if it's new
        if (targetAttributeIndex == -1) {
            charBuild.LearnNewAttribute(attId);
        } else {
            //if it's an upgrade.
            charBuild.UpgradeAttributeAtIndex(targetAttributeIndex);
        }
        attributeSelectPanel.transform.position 
            = new Vector3 ( attributeSelectPanel.transform.position.x, 1500.0f, attributeSelectPanel.transform.position.z );
        //disable LevelUpPanel and enable game panel.
        attributeSelectPanel.enabled = false;
        Game.instance.AcceptInput(true);
        //TODO better pause resume method.
        Time.timeScale = 1.0f;
    }

	
}


