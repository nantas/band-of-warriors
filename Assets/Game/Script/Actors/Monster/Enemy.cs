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
    public float lootDropRate = 0.0f;
	
	protected bool isTakingDamage;

	[System.NonSerialized]public SpawnerEnemy spawner;
	
    public void SetSpawner (SpawnerEnemy _spawner) {
        spawner = _spawner;
    }
	
	
	protected virtual void OnEnable () {
		isTakingDamage = false;
		if (spEnemy) spEnemy.enabled = true;
		if (spCollider) spCollider.enabled = true;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
		if (spEnemy) spEnemy.enabled = false;
		if (spCollider) spCollider.enabled = false;
	}
	
	// Use this for initialization
	void Awake () {
		isTakingDamage = false;
		gameObject.Init();
	}
	
	//return true if test succeed in dropping loot
	public bool isEnemyDroppingLoot() {
		float lootSelector = Random.Range(0.0f, 1.0f);
		float thisLootDropChance = lootDropRate + Game.instance.thePlayer.playerController.lootDropBoostAttribute
			+ Game.instance.thePlayer.playerController.lootDropBoostCombo;
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
		float thisLootDropChance = lootDropRate + Game.instance.thePlayer.playerController.lootDropBoostAttribute
			+ Game.instance.thePlayer.playerController.lootDropBoostCombo;
		if (thisLootDropChance > 1.0f) thisLootDropChance = 1.0f;
		if (lootSelector < thisLootDropChance/5) {
			return true;
		} else {
			return false;
		}
	}



}
