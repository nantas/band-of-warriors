using UnityEngine;
using System.Collections;

public class BtnLeft : MonoBehaviour {
	

	void Awake () {
		exUIButton uiButton = GetComponent<exUIButton>();
		uiButton.OnButtonPress += OnButtonPress;
		uiButton.OnButtonRelease += OnButtonRelease;
	}
	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnButtonPress () {
		//Debug.Log("jump button pressed!");
        if (Game.instance.thePlayer.playerController.isAcceptInput()) {
    		Game.instance.thePlayer.playerController.TurnLeft();
        }
	}
	void OnButtonRelease () {
        Game.instance.thePlayer.playerController.ReleaseCharge(BtnHoldState.Left);
	}	
}
