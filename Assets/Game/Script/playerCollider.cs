using UnityEngine;
using System.Collections;

public class playerCollider : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		Debug.Log("player touched enemy.");
	}
	
}
