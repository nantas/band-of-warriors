// ======================================================================================
// File         : Arrow.cs
// Author       : Wu Jie 
// Last Change  : 01/01/2012 | 22:17:36 PM | Sunday,January
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class Arrow : Projectile {

    public float initSpeed;
    public Collider spCollider;
    public exSprite spArrow;
    public ParticleEmitter fxArrow;
	[System.NonSerialized]public Spawner_Arrow spawner;
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
    }

    void Update () {
        if (isAffectedByGravity) {
            velocity.y += Game.instance.gravity;
        }
        float horiDist = velocity.x * Time.deltaTime;
        float vertDist = velocity.y * Time.deltaTime;
        float angleToChange = Vector2.Angle(transform.right, velocity);
        //handle movement
        if (isMoving) {
            transform.Rotate(0,0,angleToChange, Space.World);
            transform.Translate(horiDist, vertDist, 0, Space.World);
            //handle hit ground
            if (transform.position.y <= Game.instance.groundPosY) {
                isMoving = false;
                if (spawner) {
                    spawner.Invoke("DestroyArrow", 1.0f);
                } else {
                    Debug.LogError("failed to find spawner for arrow.");
                }
            }
        }
    }




}


