using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {
	
	private PlayerController controller;
	
	void Awake () {
        controller = transform.root.GetComponent<PlayerController>();
	}
	public void TouchedEnemy (bool _isHurtFromLeft, int _damageAmount) {
		//controller.OnPlayerHurt(_isHurtFromLeft, _damageAmount);
        controller.OnDamagePlayer(_isHurtFromLeft, _damageAmount);
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
}
