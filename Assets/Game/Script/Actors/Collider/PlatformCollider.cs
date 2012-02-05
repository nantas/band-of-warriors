using UnityEngine;
using System.Collections;

public class PlatformCollider : MonoBehaviour {
	
	private WarriorController player;

    [System.NonSerialized]public float stayHeight;
    [System.NonSerialized]public float leftEdge;
    [System.NonSerialized]public float rightEdge; 
	
	void Awake() {
        BoxCollider thisCollider = GetComponent<BoxCollider>();
        Vector3 colliderCenter = transform.position + thisCollider.center;
        stayHeight = colliderCenter.y + thisCollider.size.y/2;
        leftEdge = colliderCenter.x - thisCollider.size.x/2;
        rightEdge = colliderCenter.x + thisCollider.size.x/2;
	}

    void OnTriggerEnter(Collider _other) {
        if (_other.tag == "player_movement") {
            Debug.Log("###111### player collide with platform.");
            player = _other.transform.root.GetComponent<WarriorController>();
            //if (_other.transform.position.y > transform.position.y) {
            if (player.FSM_Control.FsmVariables.GetFsmBool("isAffectedByGravity").Value == true) {
                Debug.Log("###222### player collide from above.");
                player.currentPlatform = this;
                player.transform.position = new Vector3(player.transform.position.x, 
                                               stayHeight, player.transform.position.z);
                player.OnPlatformUpdate();
            }
        }

    }
	
	void LateUpdate () {
		//hack: force collision z index
		transform.position = new Vector3(transform.position.x, transform.position.y, 200);
		
	}
	
	
}

