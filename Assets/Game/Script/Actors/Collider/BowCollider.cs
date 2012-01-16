using UnityEngine;
using System.Collections;

public class BowCollider : MonoBehaviour {
	
    public exSprite spWeapon;
	private PlayerBase controller;
    private Spawner commonSpawner;
    private Collider collider;
    //this is the bow's durability, when it reaches maxHitBeforeBreak it will not collide with enemy anymore
    public int maxHitBeforeBreak = 2;
    public float timePerHitRecover = 1.0f;
    private int curHit = 0;
	
	void Awake () {
		controller = transform.root.GetComponent<PlayerBase>();
        collider = GetComponent<Collider>();
        commonSpawner = Game.instance.theSpawner;
	}
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
	}

    void Start () {
        //invoke a timer to recover durability over time.
        InvokeRepeating("HitReduceTimer", 0, timePerHitRecover);
    }

    //reduce hit number over time.
    void HitReduceTimer() {
        curHit -= 1;
        if ( curHit < 0 )
            curHit = 0;
    }
	
	public IEnumerator AttackEnemy(Vector2 _pos) {
		//Debug.Log("attacking enemy!");
        Game.instance.theGamePanel.OnComboUpdate();
        //play weapon flash white
        spWeapon.spanim.Play("flash_white");
        //increase hit number when collide with enemy.
        curHit += 1;
        //when hit number reaches limit, bow will temporarily break. and recover after 3 seconds.
        if (curHit > maxHitBeforeBreak) {
            spWeapon.spanim.Play("broken");
            collider.enabled = false;
            Invoke("WeaponRecover", 3.0f);
        }
        exSprite fx = commonSpawner.SpawnHitFXAt(_pos) as exSprite;
        fx.spanim.Play("hitFX");
	    float animTime = fx.spanim.animations[0].length;
        yield return new WaitForSeconds(animTime);
        commonSpawner.DestroyHitFX(fx);
	}

    //reset hit number when recovers.
    void WeaponRecover() {
        spWeapon.spanim.Stop();
        collider.enabled = true;
        curHit = 0;
    }
	
}

