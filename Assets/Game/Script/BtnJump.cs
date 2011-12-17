using UnityEngine;
using System.Collections;

public class BtnJump : MonoBehaviour {
	
	private PlayerBase player;

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
		player = Game.instance.thePlayer;
		player.playerController.StartJump();
	}
	
	void OnButtonRelease () {
		
	}
}
