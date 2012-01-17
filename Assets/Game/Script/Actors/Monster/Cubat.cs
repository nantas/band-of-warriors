using UnityEngine;
using System.Collections;

public class Cubat : Enemy {
	
    //fly to a position to get ready for the pounce.
    public void MoveToNearPlayer () {
        float playerX = Game.instance.thePlayer.transform.position.x;
        float targetX = Random.Range(playerX - 150, playerX + 150);
        Vector3 targetPos = new Vector3(targetX,
                                        Random.Range(Game.instance.flyPosY-100, Game.instance.flyPosY+50),
                                                     transform.position.z);
        float dist = Vector3.Distance(targetPos, transform.position);
        float moveTime = Mathf.Abs(dist)/moveSpeed;
        //magic number.
        float delayTime = Random.Range(0.0f, 0.7f);
        GetFlipDirection(targetPos.x);
        //start moving
        gameObject.MoveTo(targetPos, moveTime, delayTime, EaseType.easeInOutQuad, 
                          "MoveToPlayer", gameObject);
    }

    //dash to player after a short delay.
    public void MoveToPlayer () {
        Vector3 playerPos = Game.instance.thePlayer.transform.position;
        //magic number.
        float moveTime = Random.Range(0.5f, 0.7f);
        float delayTime = Random.Range(0.3f, 0.5f);
        GetFlipDirection(playerPos.x);
        float dist = Vector3.Distance(transform.position, playerPos);
        //start moving
        if (dist <= 500) {
            gameObject.MoveTo(playerPos, moveTime, delayTime, EaseType.easeOutQuart, 
                          "MoveToNearPlayer", gameObject);
        } else {
            MoveToNearPlayer();
        }
    }

	
	public void GetIntoField () {
        spEnemy.spanim.Play("cubat_idle");
		Vector3 targetPos = new Vector3(0, Game.instance.flyPosY, transform.position.z);
        targetPos.x = Random.Range(Game.instance.leftSpawnEntry.position.x, 
                                   Game.instance.rightSpawnEntry.position.x);
        float dist = Vector3.Distance(targetPos, transform.position);
        float moveTime = Mathf.Abs(dist)/moveSpeed;
        GetFlipDirection(targetPos.x);
		gameObject.MoveTo(targetPos, moveTime, 0, 
                          EaseType.easeInOutQuad, "MoveToNearPlayer", gameObject);	
	}

    //turn sprite around according to which direction it's heading toward.
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
			spCollider.enabled = false;
            iTween.Stop(gameObject);
            if (_isHurtFromRight) {
                //push slime a bit, hacked magic number
                transform.Translate(-30.0f,0,0);
            } else {
                transform.Translate(30.0f,0,0);
            }
            spEnemy.spanim.Play("cubat_hurt");
			float animTime = spEnemy.spanim.animations[1].length;
			yield return new WaitForSeconds(animTime);
            OnEnemyDie();
		}
	}

    public void OnEnemyDie() {
        Spawner_Cubat thisSpawner = spawner.GetComponent<Spawner_Cubat>() as Spawner_Cubat;
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
                    coin.score = 35;
                } else {
                    coin.score = 20;
                }
                coin.PopUp();
            }            
        }
    }
	
}


