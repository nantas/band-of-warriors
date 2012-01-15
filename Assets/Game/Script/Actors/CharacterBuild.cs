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

    [System.Serializable]
    public class AttributeInfo {
        public int id;
        public int modifierLv;

        public AttributeInfo() {
            id = 999;
            modifierLv = 0;
        }
    }

    public List<Attribute> attributes = new List<Attribute>();
    [System.NonSerialized] public int charLevel = 1;
    [System.NonSerialized] public int curExp = 0;
    public List<AttributeInfo> charAttributes = new List<AttributeInfo>();  

    public void OnPlayerExpChange(int _amount) {
        //TODO: add level up table and logic
        curExp += _amount;
        if ( curExp >= Game.instance.expReqForLvl[charLevel-1] ) {
            int extraExp = curExp - Game.instance.expReqForLvl[charLevel-1];
            //TODO:put the level handle into on level change function
            charLevel += 1;
            curExp = 0;
            StartCoroutine(OnPlayerLvlUp());
            OnPlayerExpChange(extraExp);
        } else {
            Game.instance.theGamePanel.EXPbar.ratio = ((float)curExp)/
                ((float)Game.instance.expReqForLvl[charLevel-1]);
        }
    }

    public IEnumerator OnPlayerLvlUp() {
        Game.instance.theGamePanel.ShowLevelUpText();
        Game.instance.theGamePanel.playerLvlDisplay.text = "lv" + charLevel;
        Game.instance.OnPlayerHPChange(20); 
        yield return new WaitForSeconds(1.0f);
        GetAttributeCandidate();
    }

    void GetAttributeCandidate() {
        bool isAttributeExceeded = false;
        int[] candidate = new int[3];
        //get a list of numbers and shuffle it to get the first 3 random unique number 
        for (int l = 0; l < 1000; l++) {
            isAttributeExceeded = false;
            int[] range = new int[attributes.Count];
            for (int i = 0; i < attributes.Count; i++) {
                range[i] = i;
            }
            for (int i = range.Length - 1; i > 0; i--) {
                int j = Random.Range(0, i);
                int temp = range[i];
                range[i] = range[j];
                range[j] = temp;
            }

            for (int i = 0; i < 3; i++ ) {
                bool isThisAttributeExceeded = false;
                if (charAttributes.Count > 0) { 
                    Attribute newAttribute = attributes[range[i]];
                    foreach (AttributeInfo attInfo in charAttributes) {
                        if (attInfo.id == range[i]) {
                            if (attInfo.modifierLv >= newAttribute.modifiers.Length - 1) {
                                isThisAttributeExceeded = true;
                                break;
                            } 
                        }
                    }
                }
                candidate[i] = range[i];
                if (isThisAttributeExceeded == true) {
                    candidate[i] = -1;
                    isAttributeExceeded = true;
                }
            }

            if (!isAttributeExceeded) break;

        } 

        ShowAttributeSelection(candidate);
    }

    void ShowAttributeSelection( int[] _candidate ) {
        Time.timeScale = 0;
        LevelUpPanel targetPanel = Game.instance.theGamePanel.panelLevelUp;
        int nextLv;
        for (int i = 0; i < 3; i++ ) {
            nextLv = 0;
            if (_candidate[i] != -1) {
                foreach (AttributeInfo attInfo in charAttributes) {
                    if (attInfo.id == _candidate[i]) {
                        nextLv = attInfo.modifierLv + 1; 
                        break;
                    }
                }
                float modifierDisplay = attributes[_candidate[i]].modifiers[nextLv];
                targetPanel.attributeSlot[i].GetComponent<BoxCollider>().enabled = true;
                targetPanel.attributeSlot[i].attId = _candidate[i]; 
                targetPanel.attributeSlot[i].modifierLv = nextLv;
                targetPanel.attributeSlot[i].attributeName.text = attributes[_candidate[i]].name;
                targetPanel.attributeSlot[i].attributeDesc.text = attributes[_candidate[i]].description
                    + modifierDisplay;    
            } else {
                targetPanel.attributeSlot[i].attId = -1;
                targetPanel.attributeSlot[i].modifierLv = -1;
                targetPanel.attributeSlot[i].attributeName.text = "not available";
                targetPanel.attributeSlot[i].attributeDesc.text = "no attribute upgrade available";
                targetPanel.attributeSlot[i].GetComponent<BoxCollider>().enabled = false;
            }

        }
        targetPanel.GetComponent<exUIPanel>().enabled = true;
        targetPanel.transform.position = new Vector3(Camera.main.transform.position.x,
                                                     0, targetPanel.transform.position.z);
        Game.instance.AcceptInput(false);
    }

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

    public void LearnNewAttribute(int _id) {
        AttributeInfo newAttInfo = new AttributeInfo(); 
        newAttInfo.id = _id;
        newAttInfo.modifierLv = 0;
        charAttributes.Add(newAttInfo);
    }

    public void UpgradeAttributeAtIndex(int _index) {
        AttributeInfo targetAttInfo = charAttributes[_index];
        targetAttInfo.modifierLv += 1;
    }

}
