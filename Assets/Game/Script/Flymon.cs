using UnityEngine;
using System.Collections;

public class Flymon : Enemy {
	
    public float moveSpeed = 100.0f;

    public void MoveToRandomLoc () {
        Vector3 targetPos = new Vector3(Random.Range(Game.instance.leftSpawnEntry.position.x, 
                                                     Game.instance.rightSpawnEntry.position.x),
                                        Game.instance.groundPosY, transform.position.z);
        float moveTime = Mathf.Abs((targetPos.x - transform.position.x))/moveSpeed;
        float delayTime = Random.Range(0.0f, 0.7f);
        GetFlipDirection(targetPos.x);
        //start moving
        gameObject.MoveTo(targetPos, moveTime, delayTime, EaseType.linear, 
                          "MoveToRandomLoc", gameObject);
    }
	
	public void GetIntoField () {
        spEnemy.spanim.Play("flymon_idle");
		Vector3 targetPos = new Vector3(0, Game.instance.flyPosY, transform.position.z);
        targetPos.x = Random.Range(Game.instance.leftSpawnEntry.position.x, 
                                   Game.instance.rightSpawnEntry.position.x);
        float dist = Vector3.Distance(targetPos, transform.position);
        float moveTime = Mathf.Abs(dist)/moveSpeed;
        GetFlipDirection(targetPos.x);
		gameObject.MoveTo(targetPos, moveTime, 0, 
                          EaseType.easeInOutQuad, "MoveToRandomLoc", gameObject);	
		
	}

    public void GetFlipDirection (float _targetPosX) {

        if (_targetPosX < transform.position.x) {
            spEnemy.scale = new Vector2(1, 1);
        } else {
            spEnemy.scale = new Vector2(-1, 1);
        }
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
            spEnemy.spanim.Play("flymon_hurt");
			float animTime = spEnemy.spanim.animations[1].length;
			yield return new WaitForSeconds(animTime);
			spawner.DestroyFlymon(this);
		}
	}
	
}

