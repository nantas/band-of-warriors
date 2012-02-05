using UnityEngine;
using System.Collections;

public class FastSlime : Enemy {
    
    public float jumpDistance = 150.0f;


    void Start() {
        hurtAnimName = "fast_slime_hurt";
    }

    void Update() {
        if (currentPlatform != Game.instance.thePlayer.playerController.currentPlatform) {
            if (Mathf.Abs(transform.position.x - Game.instance.thePlayer.transform.position.x) < jumpDistance) {
                currentPlatform = Game.instance.thePlayer.playerController.currentPlatform;
                iTween.Stop(gameObject);
                JumpToPeak(Game.instance.thePlayer.transform.position);
            }
        }
    }

    public void MoveToPlayer () {
        Vector3 targetPos = new Vector3(Game.instance.thePlayer.transform.position.x, 
                                        currentPlatform.stayHeight, transform.position.z);
        float moveTime = Mathf.Abs((targetPos.x - transform.position.x))/moveSpeed;
        float delayTime = Random.Range(0.0f, 0.7f);
        //start moving
        gameObject.MoveTo(targetPos, moveTime, delayTime, EaseType.linear, 
                          "MoveToPlayer", gameObject);
    }

    public void JumpToPeak (Vector3 _targetPos) {
        //find the jump peak point
        Vector3 peakPos = new Vector3((transform.position.x + _targetPos.x)/2, 
                                          Mathf.Max(transform.position.y, _targetPos.y)+ 120.0f, transform.position.z);
        float moveTime = Vector3.Distance(transform.position, peakPos)/moveSpeed/2;
		gameObject.MoveTo(peakPos, moveTime, 0.0f, EaseType.easeOutCubic, "JumpToTarget", gameObject, _targetPos);
    }

    public void JumpToTarget (Vector3 _targetPos) {
        float moveTime = Vector3.Distance(transform.position, _targetPos)/moveSpeed/2;
        gameObject.MoveTo(_targetPos, moveTime, 0.0f, EaseType.easeInCubic, "MoveToPlayer", gameObject);
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
        UpdateMoveConstraint(Game.instance.theBasePlatform);
		gameObject.MoveTo(path, moveTime, 0.0f, EaseType.easeOutCubic, "MoveToPlayer", gameObject);
		
	}


    public override void OnEnemyDie() {
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

