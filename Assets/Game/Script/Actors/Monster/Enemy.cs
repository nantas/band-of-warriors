using UnityEngine;
using System.Collections;

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
	
	// Update is called once per frame
	void Update () {
	
	}




}
