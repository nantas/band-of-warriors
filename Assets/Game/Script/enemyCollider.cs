using UnityEngine;
using System.Collections;

public class enemyCollider : MonoBehaviour {
	
	private Slime controller;
	
	void Awake() {
		controller = transform.parent.GetComponent<Slime>();
	}

	void OnTriggerEnter (Collider other) {
        bool isSlimeOnLeft;
        if (transform.position.x < other.transform.position.x) {
            isSlimeOnLeft = true;
        } else {
            isSlimeOnLeft = false;
        }

		if (other.tag == "player") {
			other.SendMessage("TouchedEnemy", isSlimeOnLeft);
		}
		if (other.tag == "player_weapon") {
			other.SendMessage("AttackEnemy");
			controller.SendMessage("OnDamaged", isSlimeOnLeft);
		}
	}
	
	void Update () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}
