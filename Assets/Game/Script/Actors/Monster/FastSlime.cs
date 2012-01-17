using UnityEngine;
using System.Collections;

public class FastSlime : Enemy {

    public void MoveToPlayer () {
        Vector3 targetPos = new Vector3(Game.instance.thePlayer.transform.position.x, 
                                        Game.instance.groundPosY, transform.position.z);
        float moveTime = Mathf.Abs((targetPos.x - transform.position.x))/moveSpeed;
        float delayTime = Random.Range(0.0f, 0.7f);
        //start moving
        gameObject.MoveTo(targetPos, moveTime, delayTime, EaseType.linear, 
                          "MoveToPlayer", gameObject);
    }
	
    //use a set of waypoint to move them up and down.
	public void GetIntoField () {
        spEnemy.spanim.Play("fast_slime_idle");
        Vector3 originPos = transform.position;
        Vector3 peakPos = new Vector3(Random.Range(originPos.x-100, originPos.x+100),
                                Random.Range(0, Game.instance.flyPosY+100), originPos.z);
        Vector3 landPos = new Vector3(peakPos.x*2 - originPos.x, Game.instance.groundPosY,
                                originPos.z);
        Vector3[] path = new Vector3[3];
        path[0] = originPos;
        path[1] = peakPos;
        path[2] = landPos;
		float moveTime = Random.Range(0.7f, 1.2f);
		gameObject.MoveTo(path, moveTime, 0.0f, EaseType.easeOutCubic, "MoveToPlayer", gameObject);
		
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
            OnEnemyDie();
		}
	}

    public void OnEnemyDie() {
        Spawner_BigSlime thisSpawner = spawner.GetComponent<Spawner_BigSlime>() as Spawner_BigSlime;
        thisSpawner.DestroyEnemy(this);
        spawner.levelManager.OnEnemyKilled(enemyType);
        Game.instance.OnPlayerExpChange(expPerKill);
        SpawnLoot();
    }
	
    public void SpawnLoot () {
        Spawner commonSpawner = Game.instance.theSpawner;
        //spawn coin
        if (isEnemyDroppingLoot()) {
            if (commonSpawner.aliveCoinCount < commonSpawner.maxCoinCount) {
                Coin coin = commonSpawner.SpawnCoinAt(new Vector2(transform.position.x, transform.position.y));
                if (isLootWithBonus()) {
                    coin.score = 15;
                } else {
                    coin.score = 8;
                }
                coin.PopUp();
            }            
        }
    }

}

