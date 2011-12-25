using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

	public exSprite[] allBodyParts;
    public exSprite spFX;
    public ParticleEmitter fxComboTrail;
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

    public void OnComboTrailUp (int _lvl) {
        if (_lvl == 1) {
            fxComboTrail.emit = true;
            fxComboTrail.minEmission = 10;
            fxComboTrail.maxEmission = 25;
            fxComboTrail.worldVelocity = new Vector3 (0, 50, 0);
        } else
        if (_lvl == 2) {
            fxComboTrail.emit = true;
            fxComboTrail.minEmission = 20;
            fxComboTrail.maxEmission = 40;
            fxComboTrail.worldVelocity = new Vector3 (0, 100, 0);
        } else
        if (_lvl == 3) {
            fxComboTrail.emit = true;
            fxComboTrail.minEmission = 30;
            fxComboTrail.maxEmission = 55;
            fxComboTrail.worldVelocity = new Vector3 (0, 150, 0);
        } else {
            Debug.Log("wrong input for comboLevel.");
        }
    }

    public void OnComboTrailEnd () {
        fxComboTrail.emit = false;
    }




}
