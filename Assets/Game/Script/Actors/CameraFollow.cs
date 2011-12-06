using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target; 
    public float smoothTime = 0.5f;
    private Vector3 thisTransformPos;
    private Vector2 velocity;
    private float dist2Left;
    private float dist2Right;
    private float dampTargetX = 0.0f;

    void Start() { 
        thisTransformPos = new Vector3(transform.position.x, transform.position.y, transform.position.z); 
        dist2Left = thisTransformPos.x - Game.instance.leftSpawnEntry.position.x;
        dist2Right = thisTransformPos.x - Game.instance.rightSpawnEntry.position.x;
    }

    void Update() {

        if (target.position.x - dist2Left < Game.instance.leftBoundary.position.x) {
            Debug.Log("reach left boundary!");
            dampTargetX = Game.instance.leftBoundary.position.x + dist2Left;
         } else if (target.position.x - dist2Right > Game.instance.rightBoundary.position.x) {
            Debug.Log("reach right boundary!");
            dampTargetX = Game.instance.rightBoundary.position.x + dist2Right;
        } else {
            Debug.Log("camera follow player!");
            dampTargetX = target.position.x;
        }

        thisTransformPos.x = Mathf.SmoothDamp( thisTransformPos.x, dampTargetX, 
                                                     ref velocity.x, smoothTime); 
        transform.position = thisTransformPos;

    }
}
