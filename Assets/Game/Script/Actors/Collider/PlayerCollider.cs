using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {
	
	private WarriorController controller;
	
	void Awake () {
        controller = transform.root.GetComponent<WarriorController>();
	}
	public void TouchedEnemy (bool _isHurtFromLeft, int _damageAmount) {
        //ATTR: att_defBoost multiplier
        float defBoostMultiplier = Game.instance.thePlayer
            .charBuild.GetAttributeEffectMultiplier("att_defBoost");        
        int amountAfterDef = Mathf.FloorToInt(_damageAmount * (2.0f - defBoostMultiplier));
        controller.OnDamagePlayer(_isHurtFromLeft, amountAfterDef);
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
}
