using UnityEngine;
using System.Collections;

public enum EnemyClass {
	Slime,
	BigSlime,
	FastSlime,
	Flymon,
	Cubat,
	Lurker
}

public class Enemy : MonoBehaviour {
	
	public exSprite spEnemy;
	public Collider spCollider;
    public int expPerKill = 0;
    public float moveSpeed = 100.0f;
    public int attackPower = 0;
	
	protected bool isTakingDamage;
	
	
	
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
	void Start () {
		isTakingDamage = false;
		gameObject.Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
