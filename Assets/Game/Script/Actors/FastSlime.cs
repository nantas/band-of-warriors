using UnityEngine;
using System.Collections;

public class FastSlime : Enemy {
	[System.NonSerialized]public Spawner_BigSlime spawner;

    public void SetSpawner (Spawner_BigSlime _spawner) {
        spawner = _spawner;
    }

    public void MoveToPlayer () {
        Vector3 targetPos = new Vector3(Random.Range(Game.instance.leftSpawnEntry.position.x, 
                                                     Game.instance.rightSpawnEntry.position.x),
                                        Game.instance.groundPosY, transform.position.z);
        float moveTime = Mathf.Abs((targetPos.x - transform.position.x))/moveSpeed;
        float delayTime = Random.Range(0.0f, 0.7f);
        //start moving
        gameObject.MoveTo(targetPos, moveTime, delayTime, EaseType.linear, 
                          "MoveToPlayer", gameObject);
    }
	
	public void GetIntoField () {
        spEnemy.spanim.Play("fast_slime_idle");
		Vector3 targetPos = new Vector3(0, Game.instance.groundPosY, transform.position.z);
		float moveTime = 0;
		gameObject.MoveTo(targetPos, moveTime, 0.5f, EaseType.easeInOutQuad, "MoveToPlayer", gameObject);
		
	}
	
	public IEnumerator OnDamaged(bool _isHurtFromRight) {
		if (!isTakingDamage) {
			isTakingDamage = true;
			spCollider.enabled = false;
            iTween.Stop(gameObject);
            if (_isHurtFromRight) {
                //push slime a bit, hacked magic number
                transform.Translate(-30.0f,0,0);
            } else {
                transform.Translate(30.0f,0,0);
            }
            spEnemy.spanim.Play("fast_slime_hurt");
			float animTime = spEnemy.spanim.animations[1].length;
			yield return new WaitForSeconds(animTime);
			spawner.DestroyFastSlime(this);
            Game.instance.OnPlayerExpChange(expPerKill);
            SpawnLoot();
		}
	}
	
    public void SpawnLoot () {
        int lootSelector = Random.Range(1, 100);
        Spawner commonSpawner = Game.instance.theSpawner;
        //spawn coin
        if (lootSelector < 35) {
            if (commonSpawner.aliveCoinCount < commonSpawner.maxCoinCount) {
                Coin coin = commonSpawner.SpawnCoinAt(new Vector2(transform.position.x, transform.position.y));
                coin.PopUp();
            }
        }
    }

}

