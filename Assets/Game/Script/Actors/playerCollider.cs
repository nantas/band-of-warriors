using UnityEngine;
using System.Collections;

public class playerCollider : MonoBehaviour {
	
	public WarriorControl controller;
	
	void Awake () {
	}
	void TouchedEnemy (bool _isHurtFromLeft) {
		//Debug.Log("player touched enemy.");
		controller.SendMessage("OnPlayerHurt", _isHurtFromLeft);
	}
	
	void Update () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
}
