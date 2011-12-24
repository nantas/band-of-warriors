using UnityEngine;
using System.Collections;

public class WeaponCollider : MonoBehaviour {
	
	private PlayerBase controller;
    private Spawner commonSpawner;
	
	void Awake () {
		controller = transform.root.GetComponent<PlayerBase>();
        commonSpawner = Game.instance.theSpawner;
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
	}
	
	public IEnumerator AttackEnemy(Vector2 _pos) {
		//Debug.Log("attacking enemy!");
        //play weapon flash white
        controller.allBodyParts[6].spanim.Play("flash_white");
        exSprite fx = commonSpawner.SpawnHitFXAt(_pos) as exSprite;
        fx.spanim.Play("hitFX");
	    float animTime = fx.spanim.animations[0].length;
        yield return new WaitForSeconds(animTime);
        commonSpawner.DestroyHitFX(fx);
	}
	
}
