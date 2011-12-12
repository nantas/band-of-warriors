using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SlimePool {

    public int size;
    public GameObject prefab;

    private Slime[] initSlimes;
    private int idx = 0;
    private Slime[] slimes;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initSlimes = new Slime[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initSlimes[i] = obj.GetComponent<Slime>();
				if (obj.GetComponent<exLayer>()) {
                	obj.GetComponent<exLayer>().parent = _layer;
				} else {
					Debug.LogError ("please add a layer component to enemy prefab.");
				}
            }
        }
        Reset();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        slimes = new Slime[size];
        for ( int i = 0; i < size; ++i ) {
            slimes[i] = initSlimes[i];
            slimes[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public Slime Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        Slime result = slimes[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( Slime _slime ) {
        ++idx;
        slimes[idx] = _slime;
    }
}

[System.Serializable]
public class FlymonPool {

    public int size;
    public GameObject prefab;

    private Flymon[] initFlymons;
    private int idx = 0;
    private Flymon[] flymons;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initFlymons = new Flymon[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero,
                                                        Quaternion.identity) as GameObject;
                initFlymons[i] = obj.GetComponent<Flymon>();
				if (obj.GetComponent<exLayer>()) {
                	obj.GetComponent<exLayer>().parent = _layer;
				} else {
					Debug.LogError ("please add a layer component to enemy prefab.");
				}
            }
        }
        Reset();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        flymons = new Flymon[size];
        for ( int i = 0; i < size; ++i ) {
            flymons[i] = initFlymons[i];
            flymons[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public Flymon Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        Flymon result = flymons[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( Flymon _flymon ) {
        ++idx;
        flymons[idx] = _flymon;
    }
}

[System.Serializable]
public class CoinPool {

    public int size;
    public GameObject prefab;

    private Coin[] initCoins;
    private int idx = 0;
    private Coin[] coins;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initCoins = new Coin[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initCoins[i] = obj.GetComponent<Coin>();
				if (obj.GetComponent<exLayer>()) {
                	obj.GetComponent<exLayer>().parent = _layer;
				} else {
					Debug.LogError ("please add a layer component to coin prefab.");
				}
            }
        }
        Reset();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        coins = new Coin[size];
        for ( int i = 0; i < size; ++i ) {
            coins[i] = initCoins[i];
            coins[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public Coin Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        Coin result = coins[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( Coin _coin ) {
        ++idx;
        coins[idx] = _coin;
    }
}

[System.Serializable]
public class ScorePool {

    public int size;
    public GameObject prefab;

    private exSpriteBase[] initScores;
    private int idx = 0;
    private exSpriteBase[] scores;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initScores = new exSpriteBase[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initScores[i] = obj.GetComponent<exSpriteBase>();
				if (obj.GetComponent<exLayer>()) {
                	obj.GetComponent<exLayer>().parent = _layer;
				} else {
					Debug.LogError ("please add a layer component to score prefab.");
				}
            }
        }
        Reset();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        scores = new exSpriteBase[size];
        for ( int i = 0; i < size; ++i ) {
            scores[i] = initScores[i];
            scores[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public exSpriteBase Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        exSpriteBase result = scores[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( exSpriteBase _score ) {
        ++idx;
        scores[idx] = _score;
    }
}





// ------------------------------------------------------------------ 
// Desc: 
// ------------------------------------------------------------------ 


public class Spawner : MonoBehaviour {
	
	public int maxSlimeCount = 15;
    public int maxFlymonCount = 3;
    public int maxCoinCount = 15;
    [System.NonSerialized] public int aliveCoinCount = 0;

	private int aliveSlimeCount = 0;
    private int totalSlimeSpawned = 0;
    private int aliveFlymonCount = 0;
    private int totalFlymonSpawned = 0;

	public SlimePool slimePool = new SlimePool();
    public FlymonPool flymonPool = new FlymonPool();
    public CoinPool coinPool = new CoinPool();
    public ScorePool scorePool = new ScorePool();
	
	//spawner locations
	public SpawnLocation topSpawner;
	public SpawnLocation botSpawner;
	public SpawnLocation leftSpawner;
	public SpawnLocation rightSpawner;
	
	void Awake () {
		slimePool.Init(Game.instance.enemyLayerGround);
        flymonPool.Init(Game.instance.enemyLayerAir);
        coinPool.Init(Game.instance.coinLayer);
        scorePool.Init(Game.instance.scoreLayer);
	}
	
	// Use this for initialization
	void Start () {
		Invoke("SpawnASlime", 2.0f);
        Invoke("SpawnAFlymon", 5.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SpawnASlime () {
		int spawnSelector = Random.Range(1,20);
        if ( spawnSelector < 4 ) {
			SpawnASlimeFrom (leftSpawner);
        }
        if ( spawnSelector >= 4 && spawnSelector <= 10 ) {
			SpawnASlimeFrom (topSpawner);
        }
        if ( spawnSelector > 10 && spawnSelector <= 17 ) {
			SpawnASlimeFrom (botSpawner);
        }
        if ( spawnSelector > 17 ) {
			SpawnASlimeFrom (rightSpawner);
        }
        totalSlimeSpawned += 1;
        if (totalFlymonSpawned <= 15) {
            Invoke("SpawnASlime", Random.Range(2.0f, 3.0f));
        } else if (totalFlymonSpawned > 15 && totalFlymonSpawned < 55) {
            Invoke("SpawnASlime", Random.Range(1.5f, 2.5f));
        } else if (totalFlymonSpawned >= 55 && totalFlymonSpawned < 150) {
            Invoke("SpawnASlime", Random.Range(1.5f, 2.0f));
        } else if (totalFlymonSpawned >= 150 ) {
            Invoke("SpawnASlime", Random.Range(1.0f, 1.5f));
        }
	}



	void SpawnASlimeFrom (SpawnLocation _spawner) {
		float leftBorder = _spawner.transform.position.x - _spawner.width/2;
		float rightBorder = _spawner.transform.position.x + _spawner.width/2;
		Vector2 spawnPos = new Vector2(Random.Range(leftBorder,rightBorder), _spawner.transform.position.y);
		if (aliveSlimeCount <= maxSlimeCount) {
			Slime slime = SpawnSlimeAt (spawnPos);
            aliveSlimeCount += 1;
			slime.GetIntoField(_spawner.moveDirection);
		}
	}
	
	public Slime SpawnSlimeAt (Vector2 _pos) {
		return slimePool.Request(_pos, Quaternion.identity);
	}
	
	public void DestroySlime (Slime _slime) {
		_slime.enabled = false;
        aliveSlimeCount -= 1;
        if (aliveSlimeCount < 0) aliveSlimeCount = 0;
		slimePool.Return(_slime);
	}

    public Coin SpawnCoinAt (Vector2 _pos) {
        aliveCoinCount += 1;
        return coinPool.Request(_pos, Quaternion.identity);
    }

    public void DestroyCoin (Coin _coin) {
        aliveCoinCount -= 1;
        if (aliveCoinCount < 0) aliveCoinCount = 0;
        _coin.enabled = false;
        coinPool.Return(_coin);
    }

    public exSpriteBase SpawnScoreAt (Vector2 _pos){
        return scorePool.Request(_pos, Quaternion.identity); 
    }

    public void DestroyScore (exSpriteBase _score) {
        _score.enabled = false;
        scorePool.Return(_score);
    }

	void SpawnAFlymon () {
		int spawnSelector = Random.Range(1,3);
		switch (spawnSelector) {
			case 1:
				SpawnAFlymonFrom (topSpawner);
				break;
			case 2:
				SpawnAFlymonFrom (leftSpawner);
				break;
			case 3:
				SpawnAFlymonFrom (rightSpawner);
				break;
			default:
				Debug.LogError("can't get a valid spawner");
				break;
		}
        totalFlymonSpawned += 1;
        if (totalFlymonSpawned <= 20) {
            Invoke("SpawnAFlymon", Random.Range(3.5f, 5.5f));
        } else if (totalFlymonSpawned > 20 && totalFlymonSpawned < 50) {
            Invoke("SpawnAFlymon", Random.Range(2.5f, 3.5f));
        } else if (totalFlymonSpawned >= 50 && totalFlymonSpawned < 100) {
            Invoke("SpawnAFlymon", Random.Range(1.5f, 2.5f));
        } else if (totalFlymonSpawned >= 100 ) {
            Invoke("SpawnAFlymon", Random.Range(1.0f, 2.0f));
        }
	}	

    public Flymon SpawnFlymonAt (Vector2 _pos) {
        return flymonPool.Request(_pos, Quaternion.identity);
    }

    void SpawnAFlymonFrom (SpawnLocation _spawner) {
       	float leftBorder = _spawner.transform.position.x - _spawner.width/2;
		float rightBorder = _spawner.transform.position.x + _spawner.width/2;
		Vector2 spawnPos = new Vector2(Random.Range(leftBorder,rightBorder), _spawner.transform.position.y);
		if (aliveFlymonCount <= maxFlymonCount) {
            aliveFlymonCount += 1;
			Flymon flymon = SpawnFlymonAt (spawnPos);
			flymon.GetIntoField();
		}
	}


    public void DestroyFlymon (Flymon _flymon) {
        _flymon.enabled = false;
        aliveFlymonCount -= 1;
        if (aliveFlymonCount < 0) aliveFlymonCount = 0;
        flymonPool.Return(_flymon);
    }

}
