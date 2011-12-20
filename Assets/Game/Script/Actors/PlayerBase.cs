using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

	public exSprite[] allBodyParts;
    public exSprite spFX;
    public PlayMakerFSM lancerFSM;
    

	[System.NonSerialized] public MoveDir charMoveState;
    [System.NonSerialized] public LancerController playerController;
	  
	void Awake () {
        playerController = transform.GetComponent<LancerController>();
		animation["walk"].speed = playerController.initMoveSpeed/120.0f;
	}
	
	// Use this for initialization
	void Start () {

	}

 
	public void OnHurtStart() {
        //playing hurt flash effect
        foreach (exSprite sprite in allBodyParts) {
            sprite.spanim.Play("flash");
        }
	}



}
