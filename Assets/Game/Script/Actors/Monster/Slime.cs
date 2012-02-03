using UnityEngine;
using System.Collections;

public class Slime : Enemy {

    void Start() {
        hurtAnimName = "slime_hurt";
    }


    public void MoveToRandomLoc () {
        Vector3 targetPos = new Vector3(Random.Range(Game.instance.leftSpawnEntry.position.x, 
                                                     Game.instance.rightSpawnEntry.position.x),
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
			moveTime = 3.0f;
		}
		if (moveDir == MoveDir.Up) {
			targetPos.x = transform.position.x;
			moveTime = 1.0f;
		}
		if (moveDir == MoveDir.Left) {
			targetPos.x = Game.instance.rightSpawnEntry.position.x - 30;
			moveTime = 1.0f;
		}
		if (moveDir == MoveDir.Right) {
			targetPos.x = Game.instance.leftSpawnEntry.position.x + 30;
			moveTime = 1.0f;
		}
		gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeInOutQuad, "MoveToRandomLoc", gameObject);
//		gameObject.MoveTo(targetPos, moveTime, 0);		
		
	}
	
    public override void OnEnemyDie() {
        Spawner_Slime thisSpawner = spawner.GetComponent<Spawner_Slime>() as Spawner_Slime;
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
                    coin.score = 10;
                } else {
                    coin.score = 5;
                }
                coin.PopUp();
            }            
        }
    }

}
