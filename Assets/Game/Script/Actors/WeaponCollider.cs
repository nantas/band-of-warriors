using UnityEngine;
using System.Collections;

public class WeaponCollider : MonoBehaviour {
	
	private WarriorControl controller;
	
	void Awake () {
		controller = transform.root.GetComponent<WarriorControl>();
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
	}
	
	void AttackEnemy() {
		//Debug.Log("attacking enemy!");
        //play weapon flash white
        controller.allBodyParts[2].spanim.Play("flash_white");
	}
	
}
