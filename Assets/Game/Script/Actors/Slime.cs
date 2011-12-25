using UnityEngine;
using System.Collections;

public class Slime : Enemy {
	[System.NonSerialized]public Spawner_Slime spawner;

    public void SetSpawner (Spawner_Slime _spawner) {
        spawner = _spawner;
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
			moveTime = 2.0f;
		}
		if (moveDir == MoveDir.Up) {
			targetPos.x = transform.position.x;
			moveTime = 1.0f;
		}
		if (moveDir == MoveDir.Left) {
			targetPos.x = Game.instance.rightSpawnEntry.position.x - 30;
			moveTime = 0.7f;
		}
		if (moveDir == MoveDir.Right) {
			targetPos.x = Game.instance.leftSpawnEntry.position.x + 30;
			moveTime = 0.7f;
		}
		gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeInOutQuad, "MoveToRandomLoc", gameObject);
//		gameObject.MoveTo(targetPos, moveTime, 0);		
		
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
            spEnemy.spanim.Play("slime_hurt");
			float animTime = spEnemy.spanim.animations[1].length;
			yield return new WaitForSeconds(animTime);
			spawner.DestroySlime(this);
            Game.instance.OnPlayerExpChange(expPerKill);
            SpawnLoot();
		}
	}
	
    public void SpawnLoot () {
        int lootSelector = Random.Range(1, 100);
        Spawner commonSpawner = Game.instance.theSpawner;
        //spawn coin
        if (lootSelector < 20 + Game.instance.thePlayer.playerController.curAddLootChance) {
            if (commonSpawner.aliveCoinCount < commonSpawner.maxCoinCount) {
                Coin coin = commonSpawner.SpawnCoinAt(new Vector2(transform.position.x, transform.position.y));
                if (lootSelector < 20) coin.score = 2;
                coin.PopUp();
            }
        }
    }

}
