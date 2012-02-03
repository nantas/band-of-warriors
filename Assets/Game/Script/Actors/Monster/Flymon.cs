using UnityEngine;
using System.Collections;

public class Flymon : Enemy {
    void Start() {
        hurtAnimName = "flymon_hurt";
    }
	
    public void MoveToRandomLoc () {
        Vector3 targetPos = new Vector3(Random.Range(Game.instance.leftSpawnEntry.position.x, 
                                                     Game.instance.rightSpawnEntry.position.x),
                                        Random.Range(Game.instance.groundPosY, Game.instance.flyPosY),
                                                     transform.position.z);
        float dist = Vector3.Distance(targetPos, transform.position);
        float moveTime = Mathf.Abs(dist)/moveSpeed;
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
	
    public override void OnEnemyDie() {
        Spawner_Flymon thisSpawner = spawner.GetComponent<Spawner_Flymon>() as Spawner_Flymon;
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
                    coin.score = 20;
                } else {
                    coin.score = 10;
                }
                coin.PopUp();
            }            
        }
    }
	
}

