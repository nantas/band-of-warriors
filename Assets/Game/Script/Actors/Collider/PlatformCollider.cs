using UnityEngine;
using System.Collections;

public class PlatformCollider : MonoBehaviour {
	
	private WarriorController player;
    private float groundHeight;
    private float leftBound;
    private float rightBound; 
	
	void Awake() {
        BoxCollider thisCollider = GetComponent<BoxCollider>();
        Vector3 colliderCenter = transform.position + thisCollider.center;
        groundHeight = colliderCenter.y + thisCollider.size.y/2;
        leftBound = colliderCenter.x - thisCollider.size.x/2;
        rightBound = colliderCenter.x + thisCollider.size.x/2;
	}

    void OnTriggerEnter(Collider _other) {
        if (_other.tag == "player_movement") {
            Debug.Log("###111### player collide with platform.");
            player = _other.transform.root.GetComponent<WarriorController>();
            //if (_other.transform.position.y > transform.position.y) {
            if (player.FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == true) {
                Debug.Log("###222### player collide from above.");
 
                player.moveConstraint.stayHeight = groundHeight;
                player.moveConstraint.leftEdge = leftBound;
                player.moveConstraint.rightEdge = rightBound;
                player.transform.position = new Vector3(player.transform.position.x, 
                                               groundHeight, player.transform.position.z);
                player.OnPlatformUpdate();
            }
        }

    }
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}

