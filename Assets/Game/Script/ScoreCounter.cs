using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ScoreCounter : MonoBehaviour {

    [System.NonSerialized] public int playerGold;

    void Awake () {
        playerGold = 0;
        if (!Game.instance.theGamePanel) {
            Debug.LogError("please assign a GamePanel actor in ScoreCounter");
        }
    }

    public void OnScoreChange (int _amount) {
        playerGold += _amount;
        Game.instance.theGamePanel.scoreDisplay.text = "$" + playerGold;
    }
}

