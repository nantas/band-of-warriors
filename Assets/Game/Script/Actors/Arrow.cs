// ======================================================================================
// File         : Arrow.cs
// Author       : Wu Jie 
// Last Change  : 01/01/2012 | 22:17:36 PM | Sunday,January
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

    public float initSpeed;
    public CapsuleCollider spCollider;
    public exSprite spArrow;
    public ParticleEmitter fxArrow;
    //the delay for destroy after arrow hits the ground. 
    public float groundWaitTime = 1.0f;
    //the delay for destroy after arrow hits enemy.
    public float hitWaitTime = 0.2f;
    //the gravity used by arrow specifically.
    public float arrowGravity;
	[System.NonSerialized]public Spawner_Arrow spawner;
    [System.NonSerialized]public ArcherController controller;
    //variable to check if the arrow will be destroyed after hit enemy.
    [System.NonSerialized]public bool isPenetrating = false;
    private Vector2 velocity;
    //check if an arrow is flying horizontal without being affected by gravity.
    private bool isAffectedByGravity;
    //check if arrow hits something and stopped.
    private bool isMoving;

    void Awake () {
    }

    void OnEnable () {
		if (spArrow) spArrow.enabled = true;
        if (spCollider) spCollider.enabled = true;
		if (fxArrow) fxArrow.enabled = true;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
        //reset arrow collider size, sprite scale and gravity, speed properties.
        spCollider.radius = 8.0f;
        spCollider.height = 50.0f;
        spArrow.scale = new Vector2 (1, 1);
        fxArrow.emit = false;
        initSpeed = 750.0f;
        arrowGravity = -50.0f;
        isPenetrating = false;
		if (spArrow) spArrow.enabled = false;
        if (spCollider) spCollider.enabled = false;
		if (fxArrow) fxArrow.enabled = false;
	}

	
    public void SetSpawner (Spawner_Arrow _spawner) {
        spawner = _spawner;
    }    

    public void LaunchArrowAt (Transform _anchor, bool _isAffectedByGravity) {
        //pass on the gravity setting. it will be used in update.
        isAffectedByGravity = _isAffectedByGravity;
        isMoving = true;
        //align arrow to the same direction of shootAnchor.
        Vector3 anchorDir = _anchor.right;
        //give arrow the initial speed with the same direction of shootAnchor.
        velocity = new Vector2(anchorDir.x * initSpeed, anchorDir.y * initSpeed);
        //calculating the rotation of z for the arrow.
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3 ( 0, 0, angle);
        //update arrow collider position.
        spCollider.transform.position =  new Vector3(spCollider.transform.position.x, spCollider.transform.position.y, 200);
    }

    void Update () {
        if (isMoving) {
            if (isAffectedByGravity) {
                //only add up gravity when the property allows it.
                velocity.y += arrowGravity;
            }
            float horiDist = velocity.x * Time.deltaTime;
            float vertDist = velocity.y * Time.deltaTime;
            //re-calculating arrow angle.
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3 ( 0, 0, angle);
            //handle movement
            transform.Translate(horiDist, vertDist, 0, Space.World);
            //handle out of screen with some magic number margin.
            if (transform.position.x > Game.instance.rightSpawnEntry.position.x + 150 || 
                transform.position.x < Game.instance.leftSpawnEntry.position.x - 150) {
                if (spawner) {
                    spawner.DestroyArrow(this);
                } else {
                    Debug.LogError("failed to find spawner for arrow.");
                }
            } else
            //handle hit ground
            if (transform.position.y <= Game.instance.groundPosY) {
                isMoving = false;
                StartCoroutine(WaitForDestroy());
            }
        } 
    }

    public IEnumerator WaitForDestroy() {
        yield return new WaitForSeconds(groundWaitTime);
        if (spawner) {
            spawner.DestroyArrow(this);
        } else {
            Debug.LogError("failed to find spawner for arrow.");
        }
    }

}


