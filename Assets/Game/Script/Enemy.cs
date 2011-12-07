using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public exSprite spEnemy;
	public Collider spCollider;
	
	protected bool isTakingDamage;
	
	
	
	void OnEnable () {
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
	
	public void StartIdle () {
	//	spEnemy.spanim.Play("slime_idle");
    //  Debug.Log("move in finished.");
	}
	// Update is called once per frame
	void Update () {
	
	}


}
