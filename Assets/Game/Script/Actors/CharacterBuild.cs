// ======================================================================================
// File         : CharacterBuild.cs
// Author       : nantas 
// Last Change  : 01/14/2012 | 16:20:12 PM | Saturday,January
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class CharacterBuild : MonoBehaviour {

    //AttributeInfo is to store which attribute character learned and which level it is.
    [System.Serializable]
    public class AttributeInfo {
        //attribute id, this is the index of the attribute in attributes list.
        //first you put attribute assets into the list of attributes,
        //the index of each attribute in the list will be their id.
        public int id;
        //the current learned level of the attribute,
        //each attribute can have multiple levels so you can level them up multiple times.
        public int modifierLv;

        //constructor
        public AttributeInfo() {
            id = 999;
            modifierLv = 0;
        }
    }

    //this is the list contains all the possible attribute a character can learn.
    public List<Attribute> attributes = new List<Attribute>();
    //character level
    [System.NonSerialized] public int charLevel = 1;
    //current experience, will reset to 0 after level up.
    [System.NonSerialized] public int curExp = 0;
    //this is the list contains all the attributes character already learned.
    public List<AttributeInfo> charAttributes = new List<AttributeInfo>(); 
    //dictionary to store the value of attribute affected variable name and their value
    public Dictionary<string, float> attEffectDic = new Dictionary<string, float>(); 

    //when player gets experience.
    public void OnPlayerExpChange(int _amount) {
        curExp += _amount;
        //check if the current experience exceed the need for level up.
        //according to the expReqForLvl table in game class.
        if ( curExp >= Game.instance.expReqForLvl[charLevel-1] ) {
            //if the experience gain is more than enough for level up, save the extra exp for next level.
            int extraExp = curExp - Game.instance.expReqForLvl[charLevel-1];
            //handle character level up.
            charLevel += 1;
            curExp = 0;
            //disable attribute gain for now.
            //StartCoroutine(OnPlayerLvlUp());
            OnPlayerExpChange(extraExp);
        } else {
            //update experience bar display.
            Game.instance.theGamePanel.EXPbar.ratio = ((float)curExp)/
                ((float)Game.instance.expReqForLvl[charLevel-1]);
        }
    }

    //when character level up, handle hud display and call up the LevelUpPanel.
    public IEnumerator OnPlayerLvlUp() {
        Game.instance.theGamePanel.ShowLevelUpText();
        Game.instance.theGamePanel.UpdateCharacterInfo();
        if (Game.instance.theLevelManager.currentLevel < 5) {
            Game.instance.OnPlayerHPChange(20); 
        }
        yield return new WaitForSeconds(0.8f);
        //randomly pick 3 attribute for player to choose from.
        GetAttributeCandidate();
    }

    void GetAttributeCandidate() {
        //this variable is to indicate that the attribute picked is already maxed out in level.
        bool isAttributeExceeded = false;
        //the array to store the id of the final pick of 3 attributes.
        int[] candidate = new int[3];
        //get a list of numbers and shuffle it to get the first 3 random unique number 
        //run a loop for at most 1000 times, if can't find enough available attributes after this,
        //just display available ones and omit the other slots.
        for (int l = 0; l < 1000; l++) {
            isAttributeExceeded = false;
            //create an array with all the ids in it 
            int[] range = new int[attributes.Count];
            for (int i = 0; i < attributes.Count; i++) {
                range[i] = i;
            }
            //shuffle the array to get 3 unique random id.
            for (int i = range.Length - 1; i > 0; i--) {
                int j = Random.Range(0, i);
                int temp = range[i];
                range[i] = range[j];
                range[j] = temp;
            }

            //check the first 3 id, and see if there's still upgrade available
            for (int i = 0; i < 3; i++ ) {
                bool isThisAttributeExceeded = false;
                if (charAttributes.Count > 0) { 
                    Attribute newAttribute = attributes[range[i]];
                    foreach (AttributeInfo attInfo in charAttributes) {
                        if (attInfo.id == range[i]) {
                            if (attInfo.modifierLv >= newAttribute.modifiers.Length - 1) {
                                //if the attribute picked is maxed out, set this variable to true 
                                isThisAttributeExceeded = true;
                                break;
                            } 
                        }
                    }
                }
                //store the result.
                candidate[i] = range[i];
                //if one of the picks is maxed out
                if (isThisAttributeExceeded == true) {
                    //set the id to -1 if no upgrade is available
                    candidate[i] = -1;
                    isAttributeExceeded = true;
                }
            }
            //break the loop when all 3 picks are available for upgrade.
            if (!isAttributeExceeded) break;

        } 
        //once the pick is done, show the LevelUpPanel
        ShowAttributeSelection(candidate);
    }

    //bring up LevelUpPanel with 3 picks (or less) of attributes to select.
    void ShowAttributeSelection( int[] _candidate ) {
        //TODO need proper method to pause game.
        Time.timeScale = 0;
        //find the target panel.
        LevelUpPanel targetPanel = Game.instance.theGamePanel.panelLevelUp;
        //the lv of the attribute we are upgrade to.
        int nextLv;
        //check each pick if it's a new attribute to learn or upgrade to an old one. 
        for (int i = 0; i < 3; i++ ) {
            nextLv = 0;
            if (_candidate[i] != -1) {
                foreach (AttributeInfo attInfo in charAttributes) {
                    if (attInfo.id == _candidate[i]) {
                        nextLv = attInfo.modifierLv + 1; 
                        break;
                    }
                }
                //if the attribute is ready to learn, set up the display and button.
                float modifierDisplay = attributes[_candidate[i]].modifiers[nextLv];
                targetPanel.attributeSlot[i].GetComponent<BoxCollider>().enabled = true;
                //set the id in the button to match the pick, 
                //so we know what we learn when button is pressed.
                targetPanel.attributeSlot[i].attId = _candidate[i]; 
                targetPanel.attributeSlot[i].modifierLv = nextLv;
                targetPanel.attributeSlot[i].attributeName.text = attributes[_candidate[i]].name;
                targetPanel.attributeSlot[i].attributeDesc.text = attributes[_candidate[i]].description
                    + modifierDisplay;    
            } else {
                //if a pick is not available, display so.
                targetPanel.attributeSlot[i].attId = -1;
                targetPanel.attributeSlot[i].modifierLv = -1;
                targetPanel.attributeSlot[i].attributeName.text = "not available";
                targetPanel.attributeSlot[i].attributeDesc.text = "no attribute upgrade available";
                targetPanel.attributeSlot[i].GetComponent<BoxCollider>().enabled = false;
            }

        }
        //enable the LevelUpPanel, and disable the game panel
        targetPanel.GetComponent<exUIPanel>().enabled = true;
        targetPanel.transform.position = new Vector3(Camera.main.transform.position.x,
                                                     0, targetPanel.transform.position.z);
        Game.instance.AcceptInput(false);
    }

    //function for other class to use, to check if an attribute is learned or not.
    //if it's learned, return the index of the attribute in charAttributes list.
    //if it's not, return -1
    public int isAttributeLearned(int _id) {
        if (charAttributes.Count == 0) return -1;
        for ( int i = 0; i < charAttributes.Count; i++ ) {
            AttributeInfo attInfo = charAttributes[i];
            if (attInfo.id == _id) {
                return i;
            }
        }
        return -1;
    }

    //add the new attribute to character's learned attribute list, which is charAttributes.
    public void LearnNewAttribute(int _id) {
        AttributeInfo newAttInfo = new AttributeInfo(); 
        newAttInfo.id = _id;
        newAttInfo.modifierLv = 0;
        charAttributes.Add(newAttInfo);
        //update dictionary
        UpdateAttributeEffectDictionary(_id);
    }

    //level up an existing attribute
    public void UpgradeAttributeAtIndex(int _index) {
        AttributeInfo targetAttInfo = charAttributes[_index];
        targetAttInfo.modifierLv += 1;
        //get updated attribute id and update dictionary
        int targetId = targetAttInfo.id;
        UpdateAttributeEffectDictionary(targetId);
    }

    //update attribute effect dictionary
    public void UpdateAttributeEffectDictionary(int _id) {
        //add dictionary keypair or update value for linked variable according to id
        //get the modifierLv for the attribute with id 
        int attLevel = charAttributes[isAttributeLearned(_id)].modifierLv;
        //get the value of current modifierLv.
        float effectValue = attributes[_id].modifiers[attLevel];
        //get the linked variable name
        string varName = attributes[_id].linkedVar;
        //update the dictionary
        if (attEffectDic.ContainsKey(varName)) {
            attEffectDic[varName] = effectValue;
        } else {
            attEffectDic.Add(varName, effectValue);
        }
    }

    //get the multiplier value of a certain key, this can be multiplied with the default
    //variable directly.
    public float GetAttributeEffectMultiplier(string _varName) {
        if (attEffectDic.ContainsKey(_varName)) {
            if (_varName == "att_jumpCountBoost") {
                return attEffectDic[_varName];
            } else {
                float multiplier = 1 + attEffectDic[_varName]/100;
                return multiplier;
            }
        } else {
            return 1.0f;
        }
    }

}
