using UnityEngine;
using System.Collections;

public class Slime : MonoBehaviour {
	
	public exSprite spEnemy;
	public Collider spCollider;
    public float moveSpeed = 100.0f;
	
	private bool isTakingDamage;
	
	
	
	void OnEnable () {
		isTakingDamage = false;
		if (spEnemy) spEnemy.enabled = true;
		if (spCollider) spCollider.enabled = true;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
		if (spEnemy) spEnemy.enabled = false;
		if (spCollider) spCollider.enabled = false;
		
	}
	
	// Use this for initialization
	void Start () {
		isTakingDamage = false;
		gameObject.Init();
	}
	
	public void StartIdle () {
	//	spEnemy.spanim.Play("slime_idle");
    //  Debug.Log("move in finished.");
	}
	// Update is called once per frame
	void Update () {
	
	}

    public void MoveToRandomLoc () {
        Vector3 targetPos = new Vector3(Random.Range(Game.instance.leftBoundary, 
                                                     Game.instance.rightBoundary),
                                        Game.instance.groundPosY, transform.position.z);
        float moveTime = Mathf.Abs((targetPos.x - transform.position.x))/moveSpeed;
        float delayTime = Random.Range(0.0f, 0.7f);
        //start moving
        gameObject.MoveTo(targetPos, moveTime, delayTime, EaseType.linear, 
                          "MoveToRandomLoc", gameObject);
    }
	
	public void GetIntoField (MoveDir moveDir) {
        spEnemy.spanim.Play("slime_idle");
		Vector3 targetPos = new Vector3(0, Game.instance.groundPosY, transform.position.z);
		float moveTime = 0;
		if (moveDir == MoveDir.Down) {
			targetPos.x = transform.position.x;
			moveTime = 2.0f;
		}
		if (moveDir == MoveDir.Up) {
			targetPos.x = transform.position.x;
			moveTime = 1.0f;
		}
		if (moveDir == MoveDir.Left) {
			targetPos.x = Game.instance.rightBoundary - 30;
			moveTime = 0.7f;
		}
		if (moveDir == MoveDir.Right) {
			targetPos.x = Game.instance.leftBoundary + 30;
			moveTime = 0.7f;
		}
		gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeInOutQuad, "MoveToRandomLoc", gameObject);
//		gameObject.MoveTo(targetPos, moveTime, 0);		
		
	}
	
	public IEnumerator OnDamaged(bool _isHurtFromRight) {
		if (!isTakingDamage) {
			isTakingDamage = true;
			Spawner spawner = Game.instance.theSpawner;
			spCollider.enabled = false;
            iTween.Stop(gameObject);
            if (_isHurtFromRight) {
                //push slime a bit, hacked magic number
                transform.Translate(-30.0f,0,0);
            } else {
                transform.Translate(30.0f,0,0);
            }
            spEnemy.spanim.Play("slime_hurt");
			float animTime = spEnemy.spanim.animations[1].length;
			yield return new WaitForSeconds(animTime);
			spawner.DestroySlime(this);
		}
	}
	
}
