using UnityEngine;
using System.Collections;

public class WarriorControl : MonoBehaviour {

	public float movementSpeed = 120.0f;
	
	[System.NonSerialized] public MoveDir charMoveState;

	public exSprite[] allBodyParts;

	private bool isTakingDamage = false;
	private float flashTime = 0.0f;

	  
	void Awake () {
		flashTime = allBodyParts[0].spanim.animations[0].length;
	}
	
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(1,1,1);
		charMoveState = MoveDir.Right;
		animation["walk"].speed = movementSpeed/120.0f;
		StartWalk();
		
	}
	
	// Update is called once per frame
	void Update () {
		//handle input
		if ( Input.GetButton("Right")) {
			TurnRight();
		}
		if ( Input.GetButton("Left")) {
			TurnLeft();
		}
		if ( Input.GetButton("Jump") ) {
			StartJump();
		}
		
		//handle movement
		float horizonDist = Time.deltaTime * movementSpeed;		
		switch (charMoveState) {
			case MoveDir.Right:
				//don't need to change
				break;
			case MoveDir.Left:
				horizonDist = -horizonDist;
				break;
			case MoveDir.Stop:
				horizonDist = 0;
				break;
			default:
				horizonDist = 0;
				break;
		}
		//move character
		if ((transform.position.x + horizonDist < Game.instance.rightBoundary) 
			&& (transform.position.x + horizonDist > Game.instance.leftBoundary) ) {
			transform.Translate (horizonDist, 0, 0);
		}
	
	}
	
	public void TurnRight() {
		if (charMoveState != MoveDir.Right) {
			charMoveState = MoveDir.Right;
			transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void TurnLeft() {
		if (charMoveState != MoveDir.Left) {
			charMoveState = MoveDir.Left;
			transform.localScale = new Vector3(-1,1,1);
		}
	}
	
	public IEnumerator OnPlayerHurt() {
		Debug.Log("Calling OnPlayerHurt");
		if (!isTakingDamage) {
			isTakingDamage = true;
			//playing hurt flash effect
			foreach (exSprite sprite in allBodyParts) {
				sprite.spanim.Play("flash");
			}
			Debug.Log("playing hurt animation");
			yield return new WaitForSeconds(flashTime);
			isTakingDamage = false;	
		}
	}
	
	public void StartWalk() {
		if (!animation.IsPlaying("walk")) {
			animation.Play("walk");
		}
	}
	
	public void StartJump() {
		if (!animation.IsPlaying("jump")) {
			animation.Play("jump");
		}
	}
	
	public void OnJumpFinish() {
		StartWalk();
	}
}
