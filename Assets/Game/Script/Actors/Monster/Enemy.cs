using UnityEngine;
using System.Collections;

//global enum defines enemy types, used to spawn enemy and track mission objective.
public enum EnemyClass {
	Slime,
	BigSlime,
	FastSlime,
	Flymon,
	Cubat,
	Lurker,
    AnyEnemy
}

public class Enemy : MonoBehaviour {
	
    public EnemyClass enemyType;
	public exSprite spEnemy;
	public Collider spCollider;
    public int expPerKill = 0;
    public float moveSpeed = 100.0f;
    public int attackPower = 0;
    public int enemyHp = 1;
    public float lootDropRate = 0.0f;
    [System.NonSerialized]public int initEnemyHpStatic;
    //use this variable to store sprite animation name for certain enemy class
    [System.NonSerialized]public string hurtAnimName;
    //move constraint only used for ground enemy
    [System.NonSerialized]public PlatformCollider currentPlatform;
	
	protected bool isTakingDamage;

	[System.NonSerialized]public SpawnerEnemy spawner;
	
    public void SetSpawner (SpawnerEnemy _spawner) {
        spawner = _spawner;
    }
	
	// Use this for initialization
	void Awake () {
        initEnemyHpStatic = enemyHp;
		isTakingDamage = false;
		gameObject.Init();
	}	

	protected virtual void OnEnable () {
		isTakingDamage = false;
		if (spEnemy) spEnemy.enabled = true;
		if (spCollider) spCollider.enabled = true;
        //reset enemy hp
        enemyHp = initEnemyHpStatic;
	}


	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
		if (spEnemy) spEnemy.enabled = false;
		if (spCollider) spCollider.enabled = false;
	}
	

	
	//return true if test succeed in dropping loot
	public bool isEnemyDroppingLoot() {
		float lootSelector = Random.Range(0.0f, 1.0f);
		float thisLootDropChance = lootDropRate + Game.instance.thePlayer.playerController.lootChanceBoostAttribute
			+ Game.instance.thePlayer.playerController.lootChanceBoostCombo;
		if (thisLootDropChance > 1.0f) thisLootDropChance = 1.0f;
		if (lootSelector <= thisLootDropChance) {
			return true;
		} else {
			return false;
		}
	}

	//return true if the loot has better value than normal
	public bool isLootWithBonus() {
		float lootSelector = Random.Range(0.0f, 1.0f);
		float thisLootDropChance = lootDropRate + Game.instance.thePlayer.playerController.lootChanceBoostAttribute
			+ Game.instance.thePlayer.playerController.lootChanceBoostCombo;
		if (thisLootDropChance > 1.0f) thisLootDropChance = 1.0f;
		if (lootSelector < thisLootDropChance/5) {
			return true;
		} else {
			return false;
		}
	}

    //handle enemy hurt logic
   	public virtual IEnumerator OnDamaged(bool _isHurtFromRight) {
		if (!isTakingDamage) {
			isTakingDamage = true;
			spCollider.enabled = false;
            iTween.Stop(gameObject);
            //handle push back
            float pushAmount = 50.0f;
            Vector3 moveAmount = new Vector3(pushAmount, 0, 0);
            float moveTime = 0.05f;
            if (_isHurtFromRight) {
                moveAmount.x = -pushAmount;
            }
            gameObject.MoveBy(moveAmount, Space.World, moveTime, 0, EaseType.easeInQuad);            
            spEnemy.spanim.Play(hurtAnimName);
			float animTime = spEnemy.spanim.animations[1].length;
			yield return new WaitForSeconds(animTime);
            OnEnemyDie();
		}
	}

    public virtual void OnEnemyDie() {
    }

    //get a target position for random ground moving enemy
    protected Vector3 GetRandomGroundPos() {
        Debug.Log("currentPlatform: " + currentPlatform);
        float leftMostX = Mathf.Max(Game.instance.leftSpawnEntry.position.x, currentPlatform.leftEdge);
        float rightMostX = Mathf.Min(Game.instance.rightSpawnEntry.position.x, currentPlatform.rightEdge);
        Vector3 targetPos = new Vector3(Random.Range(leftMostX, rightMostX),
                                        currentPlatform.stayHeight, transform.position.z);
        return targetPos;
    }

    public void UpdateMoveConstraint(PlatformCollider _platform) {
        Debug.Log("_platform: " + _platform);
        currentPlatform = _platform;
    }



}
