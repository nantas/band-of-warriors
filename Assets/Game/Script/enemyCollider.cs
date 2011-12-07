using UnityEngine;
using System.Collections;

public class enemyCollider : MonoBehaviour {
	
	private Enemy controller;
	
	void Awake() {
		controller = transform.parent.GetComponent<Enemy>();
	}

	void OnTriggerEnter (Collider other) {
        bool isEnemyOnLeft;
        if (transform.position.x < other.transform.position.x) {
            isEnemyOnLeft = true;
        } else {
            isEnemyOnLeft = false;
        }

		if (other.tag == "player") {
			other.SendMessage("TouchedEnemy", isEnemyOnLeft);
		}
		if (other.tag == "player_weapon") {
			other.SendMessage("AttackEnemy");
			controller.SendMessage("OnDamaged", isEnemyOnLeft);
		}
	}
	
	void Update () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}
