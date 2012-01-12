using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {
	
	private WarriorController controller;
	
	void Awake () {
        controller = transform.root.GetComponent<WarriorController>();
	}
	public void TouchedEnemy (bool _isHurtFromLeft, int _damageAmount) {
        controller.OnDamagePlayer(_isHurtFromLeft, _damageAmount);
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
}
