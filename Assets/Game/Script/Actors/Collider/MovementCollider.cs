using UnityEngine;
using System.Collections;

public class MovementCollider : MonoBehaviour {
	
	private WarriorController controller;
	
	void Awake () {
        controller = transform.root.GetComponent<WarriorController>();
	}

	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
}
