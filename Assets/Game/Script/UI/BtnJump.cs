using UnityEngine;
using System.Collections;

public class BtnJump : MonoBehaviour {
	
	private WarriorController playerControl;

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
		Debug.Log("jump button pressed!");
		playerControl = Game.instance.thePlayer.GetComponent<WarriorController>();;
        Debug.Log(playerControl.gameObject);
        Debug.Log(playerControl.isAcceptInput());
        if (playerControl.isAcceptInput()) {
    		playerControl.StartJump();
        }
	}
	
	void OnButtonRelease () {
		
	}
}
