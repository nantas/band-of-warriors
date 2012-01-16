using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

    //array to store reference for player sprites 
	public exSprite[] allBodyParts;
    //sprite fx such as speedline and weapon slash.
    public exSprite spFX;
    //display trail fx when player has lv up the combo.
    public ParticleEmitter fxComboTrail;
    
    [System.NonSerialized] public WarriorController playerController;
    [System.NonSerialized] public CharacterBuild charBuild;
	  
	void Awake () {
        playerController = transform.GetComponent<WarriorController>();
        charBuild = transform.GetComponent<CharacterBuild>();
		animation["walk"].speed = playerController.moveSpeed/120.0f;
	}
	
	// Use this for initialization
	void Start () {

	}

 
	public void OnHurtStart() {
        //playing hurt flash effect
        foreach (exSprite sprite in allBodyParts) {
            if (sprite) {
                sprite.spanim.Play("flash");
            }
        }
	}

    //change particle display according to combo level.
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
