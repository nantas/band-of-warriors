using UnityEngine;
using System.Collections;

public class BigSlime : Enemy {
    public int HP = 3;
    public float hurtInvincibleTime = 0.5f;
	[System.NonSerialized]public Spawner_BigSlime spawner;

    public void SetSpawner (Spawner_BigSlime _spawner) {
        spawner = _spawner;
    }

    void OnEnable() {
		isTakingDamage = false;
		if (spEnemy) spEnemy.enabled = true;
		if (spCollider) spCollider.enabled = true;
        HP = 3;
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
        spEnemy.spanim.Play("big_slime_idle");
		Vector3 targetPos = new Vector3(0, Game.instance.groundPosY, transform.position.z);
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
	
	public IEnumerator OnDamaged(bool _isHurtFromRight) {
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
            HP -= 1;
            spEnemy.spanim.Play("big_slime_hurt");
			yield return new WaitForSeconds(hurtInvincibleTime);
            if (HP <= 0) {
                StartCoroutine(OnDeath());
            } else {
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
            float animTime = 0.3f;
            yield return new WaitForSeconds(animTime);
            Game.instance.OnPlayerExpChange(expPerKill);
            spawner.SpawnFastSlimeFrom(this, 3);
            yield return new WaitForSeconds(0.1f);
            spawner.DestroySlime(this);
            SpawnLoot();        
    }
	
    public void SpawnLoot () {
        int lootSelector = Random.Range(1, 100);
        Spawner commonSpawner = Game.instance.theSpawner;
        //spawn coin
        if (lootSelector < 35 + Game.instance.thePlayer.playerController.curAddLootChance) {
            if (commonSpawner.aliveCoinCount < commonSpawner.maxCoinCount) {
                Coin coin = commonSpawner.SpawnCoinAt(new Vector2(transform.position.x, transform.position.y));
                if (lootSelector < 15 + Game.instance.thePlayer.playerController.curAddLootChance/2 ) {
                    coin.score = 5;
                } else {
                    coin.score = 2;
                }
                coin.PopUp();
            }
        }
    }

}

