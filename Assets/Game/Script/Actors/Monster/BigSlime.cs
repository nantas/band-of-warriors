using UnityEngine;
using System.Collections;

public class BigSlime : Enemy {
    public float hurtInvincibleTime = 0.5f;

    protected override void OnEnable() {
		isTakingDamage = false;
		if (spEnemy) spEnemy.enabled = true;
		if (spCollider) spCollider.enabled = true;
        enemyHp = initEnemyHpStatic;
    }

    void Start() {
        hurtAnimName = "big_slime_hurt";
    }

    //move to random position along ground height.
    public void MoveToRandomLoc () {
        Vector3 targetPos = GetRandomGroundPos(); 
        float moveTime = Mathf.Abs((targetPos.x - transform.position.x))/moveSpeed;
        float delayTime = Random.Range(0.0f, 0.7f);
        //start moving, and repeating after reach the position.
        gameObject.MoveTo(targetPos, moveTime, delayTime, EaseType.linear, 
                          "MoveToRandomLoc", gameObject);
    }
	
    //after spawn, move into the camera view.
	public void GetIntoField (MoveDir moveDir) {
        spEnemy.spanim.Play("big_slime_idle");
		Vector3 targetPos = new Vector3(0, Game.instance.groundPosY, transform.position.z);
        PlatformCollider platform = Game.instance.theBasePlatform;
        UpdateMoveConstraint(platform);
		float moveTime = 0;
		if (moveDir == MoveDir.Left) {
			targetPos.x = Game.instance.rightSpawnEntry.position.x - 30;
			moveTime = 0.7f;
		}
		if (moveDir == MoveDir.Right) {
			targetPos.x = Game.instance.leftSpawnEntry.position.x + 30;
			moveTime = 0.7f;
		}
		gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeInOutQuad, "MoveToRandomLoc", gameObject);
		
	}
	
	public override IEnumerator OnDamaged(bool _isHurtFromRight) {
		if (!isTakingDamage) {
			isTakingDamage = true;
			spCollider.enabled = false;
            iTween.Stop(gameObject);
            if (_isHurtFromRight) {
                //push slime a bit, hacked magic number
                transform.Translate(-50.0f,0,0);
            } else {
                transform.Translate(50.0f,0,0);
            }
            //player damage 
            int damageAmount = Game.instance.thePlayer.playerController.attackPower;
            enemyHp -= damageAmount;
            spEnemy.spanim.Play("big_slime_hurt");
			yield return new WaitForSeconds(hurtInvincibleTime);
            if (enemyHp <= 0) {
                StartCoroutine(OnDeath());
            } else {
                //if hp is not 0, go back to random move.
                isTakingDamage = false;
                spCollider.enabled = true;
                spEnemy.spanim.Play("big_slime_idle");
                MoveToRandomLoc();
            }
		}
	}

    public IEnumerator OnDeath() {
            spEnemy.spanim.Play("big_slime_die");
//          float animTime = spEnemy.spanim.animations[2].length;
            //magic number animation time.
            float animTime = 0.3f;
            yield return new WaitForSeconds(animTime);
            Game.instance.OnPlayerExpChange(expPerKill);
            Spawner_BigSlime thisSpawner = spawner.GetComponent<Spawner_BigSlime>() as Spawner_BigSlime;
            //explodes and spawn fast slime.
            thisSpawner.SpawnFastSlimeFrom(this, 3);
            yield return new WaitForSeconds(0.1f);
            thisSpawner.DestroyEnemy(this);
            spawner.levelManager.OnEnemyKilled(enemyType);
            SpawnLoot();    
    }

	
    public void SpawnLoot () {
        Spawner commonSpawner = Game.instance.theSpawner;
        //spawn coin
        if (isEnemyDroppingLoot()) {
            if (commonSpawner.aliveCoinCount < commonSpawner.maxCoinCount) {
                Coin coin = commonSpawner.SpawnCoinAt(new Vector2(transform.position.x, transform.position.y));
                if (isLootWithBonus()) {
                    coin.score = 40;
                } else {
                    coin.score = 25;
                }
                coin.PopUp();
            }            
        }
    }

}

