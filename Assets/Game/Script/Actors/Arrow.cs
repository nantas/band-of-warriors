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
    public float groundWaitTime = 1.0f;
    public float hitWaitTime = 0.2f;
    public float arrowGravity;
	[System.NonSerialized]public Spawner_Arrow spawner;
    [System.NonSerialized]public ArcherController controller;
    [System.NonSerialized]public bool isPenetrating = false;
    private Vector2 velocity;
    private bool isAffectedByGravity;
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
        isAffectedByGravity = _isAffectedByGravity;
        isMoving = true;
        Vector3 anchorDir = _anchor.right;
        velocity = new Vector2(anchorDir.x * initSpeed, anchorDir.y * initSpeed);
        //Vector2 fromAngle = new Vector2(transform.right.x, transform.right.y);
        //float angleToChange = Vector2.Angle(fromAngle, velocity);
        //transform.Rotate(0,0, angleToChange, Space.World);
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3 ( 0, 0, angle);
        spCollider.transform.position =  new Vector3(spCollider.transform.position.x, spCollider.transform.position.y, 200);
    }

    void Update () {
        if (isMoving) {
            if (isAffectedByGravity) {
                velocity.y += arrowGravity;
            }
            float horiDist = velocity.x * Time.deltaTime;
            float vertDist = velocity.y * Time.deltaTime;
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3 ( 0, 0, angle);
            //handle movement
            transform.Translate(horiDist, vertDist, 0, Space.World);
            //handle out of screen
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


