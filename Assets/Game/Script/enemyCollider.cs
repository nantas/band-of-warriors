using UnityEngine;
using System.Collections;

public class enemyCollider : MonoBehaviour {

	void OnCollisionEnter (Collision col) {
		Debug.Log("enemy touched.");
	}
	
}
