using UnityEngine;
using System.Collections;

public class CoinCollider : MonoBehaviour {
	
	private Coin controller;
	
	void Awake() {
		controller = transform.parent.GetComponent<Coin>();
	}

	void OnTriggerEnter (Collider other) {
        bool isPlayerOnRight;
        if (transform.position.x < other.transform.position.x) {
            isPlayerOnRight = true;
        } else {
            isPlayerOnRight = false;
        }

		if (other.tag == "player") {
			controller.SendMessage("OnPickedUp", isPlayerOnRight);
        }
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}
