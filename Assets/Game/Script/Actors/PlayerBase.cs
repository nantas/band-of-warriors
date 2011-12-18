using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

    public float dashDuration = 0.5f;
    public float airDashDuration = 0.4f;
    public float dashSpeed = 400f;
    public float stunTime = 0.1f;
	public float flashTime = 0.4f;
	public exSprite[] allBodyParts;
    public exSprite spFX;
    public PlayMakerFSM lancerFSM;
    

	[System.NonSerialized] public MoveDir charMoveState;
    [System.NonSerialized] public PlayerController playerController;
	  
	void Awake () {
		flashTime = allBodyParts[0].spanim.animations[0].length;
        lancerFSM = transform.GetComponent<PlayMakerFSM>();
        playerController = transform.GetComponent<PlayerController>();
		animation["walk"].speed = playerController.initMoveSpeed/120.0f;
	}
	
	// Use this for initialization
	void Start () {

	}

    public void OnDashStart() {
        spFX.spanim.Play("speedline");
    }

    public void OnDashStop() {
        spFX.spanim.Play("speedline_off");
    }

    public void OnAirDashStart() {
        spFX.spanim.Play("speedline");
    }

    public void OnAirDashStop() {
        spFX.spanim.Play("speedline_off");
    }
 
	public void OnHurtStart() {
        //playing hurt flash effect
        foreach (exSprite sprite in allBodyParts) {
            sprite.spanim.Play("flash");
        }
	}

    public void OnPlayerNoHP() {
        Debug.Log("player has no hp. go to dead state.");
        playerController.isAboutToDie = true;
    }

}
