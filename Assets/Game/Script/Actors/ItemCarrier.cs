using UnityEngine;
using System.Collections;

public class ItemCarrier : Enemy {

    public ParticleEmitter fx;
    public float moveTimePerWave;
    private Vector3 originPos;
    [System.NonSerialized]public MoveDir moveDir;

    void Start () {
        originPos = transform.position;
        moveDir = MoveDir.Stop;
    }

	
    public void WaveMoveRight () {
        Vector3 startPos = transform.position;
        Vector3 peakPos = new Vector3(startPos.x + 100.0f, startPos.y + 150.0f,
                                startPos.z);
        Vector3 botPos = new Vector3(startPos.x + 200.0f, startPos.y - 50.0f,
                                startPos.z);
        Vector3 endPos = new Vector3(startPos.x + 300.0f, startPos.y,
                                     startPos.z);
        Vector3[] path = new Vector3[4];
        path[0] = startPos;
        path[1] = peakPos;
        path[2] = botPos;
        path[3] = endPos;
		float moveTime = moveTimePerWave;
		gameObject.MoveTo(path, moveTime, 0.0f, EaseType.linear, "WaveMoveRight", gameObject);
		
    }

    public void WaveMoveLeft () {
        Vector3 startPos = transform.position;
        Vector3 peakPos = new Vector3(startPos.x - 100.0f, startPos.y + 150.0f,
                                startPos.z);
        Vector3 botPos = new Vector3(startPos.x - 200.0f, startPos.y - 50.0f,
                                startPos.z);
        Vector3 endPos = new Vector3(startPos.x - 300.0f, startPos.y,
                                     startPos.z);
        Vector3[] path = new Vector3[4];
        path[0] = startPos;
        path[1] = peakPos;
        path[2] = botPos;
        path[3] = endPos;
		float moveTime = moveTimePerWave;
		gameObject.MoveTo(path, moveTime, 0.0f, EaseType.linear, "WaveMoveLeft", gameObject);
		
    }
	
	public void SpawnItemCarrier () {
        spCollider.enabled = true;
        fx.emit = true;
        int dirSelector = Random.Range(1,10);
        if (dirSelector > 5) {
            //spawn carrier from the left
            transform.position = new Vector3 (Game.instance.leftBoundary.position.x, Game.instance.leftBoundary.position.y,
                                              transform.position.z);
            moveDir = MoveDir.Right;
            WaveMoveRight();
        } else {
            //spawn carrier from the right 
            transform.position = new Vector3 (Game.instance.rightBoundary.position.x, Game.instance.rightBoundary.position.y,
                                              transform.position.z);
            moveDir = MoveDir.Left;
            WaveMoveLeft();
        }
		
	}

    void LateUpdate () {
        if (transform.position.x < Game.instance.leftBoundary.position.x 
            && moveDir == MoveDir.Left) {
            OnOutOfBound();
            return;
        }
        if (transform.position.x > Game.instance.rightBoundary.position.x
            && moveDir == MoveDir.Right) {
            OnOutOfBound();
            return;
        }
    }

    void OnOutOfBound() {
        transform.position = originPos;
        moveDir = MoveDir.Stop;
        Game.instance.theLevelManager.CancelInvoke("HealthPackTimer");
        Game.instance.theLevelManager.Invoke("HealthPackTimer", 35.0f);
    }

	
	public void OnDamaged(bool _isHurtFromRight) {
			spCollider.enabled = false;
            fx.emit = false;
            iTween.Stop(gameObject);
            OnEnemyDie();
	}

    public void OnEnemyDie() {
        SpawnLoot();
        transform.position = originPos;
        moveDir = MoveDir.Stop;
        Game.instance.theLevelManager.CancelInvoke("HealthPackTimer");
        Game.instance.theLevelManager.Invoke("HealthPackTimer", 35.0f);
    }

    public void SpawnLoot () {
        Spawner commonSpawner = Game.instance.theSpawner;
        //spawn coin
        HealthPack hp = commonSpawner.SpawnHealthPackAt(new Vector2(transform.position.x, 
                                                          transform.position.y));
        hp.PopUp();
    }





}

