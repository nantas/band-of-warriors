using UnityEngine;
using System.Collections;

//global enum defines enemy types, used to spawn enemy and track mission objective.
public enum EnemyClass {
	Slime,
	BigSlime,
	FastSlime,
	Flymon,
	Cubat,
	Lurker,
    AnyEnemy
}

public class Enemy : MonoBehaviour {
	
    public EnemyClass enemyType;
	public exSprite spEnemy;
	public Collider spCollider;
    public int expPerKill = 0;
    public float moveSpeed = 100.0f;
    public int attackPower = 0;
    public int enemyHp = 1;
    public float lootDropRate = 0.0f;
    [System.NonSerialized]public int initEnemyHpStatic;
	
	protected bool isTakingDamage;

	[System.NonSerialized]public SpawnerEnemy spawner;
	
    public void SetSpawner (SpawnerEnemy _spawner) {
        spawner = _spawner;
    }
	
	// Use this for initialization
	void Awake () {
        initEnemyHpStatic = enemyHp;
		isTakingDamage = false;
		gameObject.Init();
	}	

	protected virtual void OnEnable () {
		isTakingDamage = false;
		if (spEnemy) spEnemy.enabled = true;
		if (spCollider) spCollider.enabled = true;
        //reset enemy hp
        enemyHp = initEnemyHpStatic;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
		if (spEnemy) spEnemy.enabled = false;
		if (spCollider) spCollider.enabled = false;
	}
	

	
	//return true if test succeed in dropping loot
	public bool isEnemyDroppingLoot() {
		float lootSelector = Random.Range(0.0f, 1.0f);
		float thisLootDropChance = lootDropRate + Game.instance.thePlayer.playerController.lootChanceBoostAttribute
			+ Game.instance.thePlayer.playerController.lootChanceBoostCombo;
		if (thisLootDropChance > 1.0f) thisLootDropChance = 1.0f;
		if (lootSelector <= thisLootDropChance) {
			return true;
		} else {
			return false;
		}
	}

	//return true if the loot has better value than normal
	public bool isLootWithBonus() {
		float lootSelector = Random.Range(0.0f, 1.0f);
		float thisLootDropChance = lootDropRate + Game.instance.thePlayer.playerController.lootChanceBoostAttribute
			+ Game.instance.thePlayer.playerController.lootChanceBoostCombo;
		if (thisLootDropChance > 1.0f) thisLootDropChance = 1.0f;
		if (lootSelector < thisLootDropChance/5) {
			return true;
		} else {
			return false;
		}
	}



}
