using UnityEngine;
using System.Collections;

public class EnemyCollider : MonoBehaviour {
	
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
			other.GetComponent<PlayerCollider>().TouchedEnemy(isEnemyOnLeft, 
                                                              controller.attackPower);

		}
		if (other.tag == "player_weapon") {
			other.SendMessage("AttackEnemy");
			controller.SendMessage("OnDamaged", isEnemyOnLeft);
		}
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}
