using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	
	public exSprite spCoin;
	public Collider spCollider;
	
	
	void OnEnable () {
		if (spCoin) spCoin.enabled = true;
		if (spCollider) spCollider.enabled = true;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
        iTween.Stop(gameObject);
		if (spCoin) spCoin.enabled = false;
		if (spCollider) spCollider.enabled = false;
		
	}
	
	// Use this for initialization
	void Start () {
		gameObject.Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
