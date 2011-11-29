using UnityEngine;
using System.Collections;

public class SpawnLocation : MonoBehaviour {

	public MoveDir moveDirection;
	[System.NonSerialized] public float width;
	
	void Awake () {
		width = transform.GetComponent<exSprite>().width;
	}

}
