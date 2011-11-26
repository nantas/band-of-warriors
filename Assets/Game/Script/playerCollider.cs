using UnityEngine;
using System.Collections;

public class playerCollider : MonoBehaviour {
	
	private WarriorControl controller;
	
	void Awake () {
		controller = transform.parent.GetComponent<WarriorControl>();
	}
	void TouchedEnemy () {
		//Debug.Log("player touched enemy.");
		//controller.OnPlayerHurt();
		controller.SendMessage("OnPlayerHurt");
	}
	
	void Update () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
}
