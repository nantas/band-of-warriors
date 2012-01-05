using UnityEngine;
using System.Collections;

public class ArrowCollider : MonoBehaviour {
	
    public exSprite spArrow;
    private Arrow arrow;
    private Spawner commonSpawner;
	
	void Awake () {
        arrow = transform.root.GetComponent<Arrow>();
        commonSpawner = Game.instance.theSpawner;
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
	}
	
	public IEnumerator AttackEnemy(Vector2 _pos) {
		//Debug.Log("attacking enemy!");
        Game.instance.theGamePanel.OnComboUpdate();
        //play weapon flash white
        spArrow.spanim.Play("arrow_hit");
        exSprite fx = commonSpawner.SpawnHitFXAt(_pos) as exSprite;
        fx.spanim.Play("hitFX");
        if (arrow.isPenetrating == false) {
            arrow.spawner.DestroyArrow(arrow);
        }
	    float animTime = fx.spanim.animations[0].length;
        yield return new WaitForSeconds(animTime);
        commonSpawner.DestroyHitFX(fx);
	}
	
}
