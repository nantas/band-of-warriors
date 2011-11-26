using UnityEngine;
using System.Collections;

public class enemyCollider : MonoBehaviour {
	
	private Slime controller;
	
	void Awake() {
		controller = transform.parent.GetComponent<Slime>();
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "player") {
			other.SendMessage("TouchedEnemy");
		}
		if (other.tag == "player_weapon") {
			other.SendMessage("AttackEnemy");
			controller.SendMessage("OnDamaged");
		}
	}
	
	void Update () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}
