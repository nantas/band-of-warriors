using UnityEngine;
using System.Collections;

public class ItemCollider : MonoBehaviour {
	
	private Item controller;
	
	void Awake() {
		controller = transform.root.GetComponent<Item>();
	}

	void OnTriggerEnter (Collider other) {
        bool isPlayerOnRight;
        if (transform.position.x < other.transform.position.x) {
            isPlayerOnRight = true;
        } else {
            isPlayerOnRight = false;
        }

		if (other.tag == "player") {
			controller.OnPickedUp();
        }
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}
