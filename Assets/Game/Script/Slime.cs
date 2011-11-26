using UnityEngine;
using System.Collections;

public class Slime : MonoBehaviour {
	
	public exSprite spEnemy;
	public Collider spCollider;
	
	private bool isTakingDamage;
	
	void OnEnable () {
		isTakingDamage = false;
		if (spEnemy) spEnemy.enabled = true;
		if (spCollider) spCollider.enabled = true;
		spEnemy.spanim.Play("slime_idle");
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
		StartIdle();
	}
	
	void StartIdle () {
		spEnemy.spanim.Play("slime_idle");
	}
	// Update is called once per frame
	void Update () {
	
	}
	
	public IEnumerator OnDamaged() {
		if (!isTakingDamage) {
			isTakingDamage = true;
			Spawner spawner = Game.instance.theSpawner;
			spCollider.enabled = false;
			spEnemy.spanim.Play("slime_hurt");
			float animTime = spEnemy.spanim.animations[1].length;
			yield return new WaitForSeconds(animTime);
			spawner.DestroySlime(this);
		}
	}
	
}
