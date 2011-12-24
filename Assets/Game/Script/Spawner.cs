using UnityEngine;
using System.Collections;
using System.Collections.Generic;



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
public class HitFXPool {

    public int size;
    public GameObject prefab;

    private exSpriteBase[] initHitFXs;
    private int idx = 0;
    private exSpriteBase[] hitFXs;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initHitFXs = new exSpriteBase[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initHitFXs[i] = obj.GetComponent<exSpriteBase>();
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
        hitFXs = new exSpriteBase[size];
        for ( int i = 0; i < size; ++i ) {
            hitFXs[i] = initHitFXs[i];
            hitFXs[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public exSpriteBase Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        exSpriteBase result = hitFXs[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( exSpriteBase _fx ) {
        ++idx;
        hitFXs[idx] = _fx;
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
	
    public int maxCoinCount = 15;
    [System.NonSerialized] public int aliveCoinCount = 0;

    public CoinPool coinPool = new CoinPool();
    public ScorePool scorePool = new ScorePool();
    public HitFXPool hitFXPool = new HitFXPool();
	
	//spawner locations
	public SpawnLocation topSpawner;
	public SpawnLocation botSpawner;
	public SpawnLocation leftSpawner;
	public SpawnLocation rightSpawner;
	
	void Awake () {
        coinPool.Init(Game.instance.coinLayer);
        scorePool.Init(Game.instance.scoreLayer);
        hitFXPool.Init(Game.instance.fxLayer);
	}
	
	// Use this for initialization
	void Start () {
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

    public exSpriteBase SpawnHitFXAt (Vector2 _pos) {
        return hitFXPool.Request(_pos, Quaternion.identity);
    }

    public void DestroyHitFX (exSpriteBase _fx) {
        _fx.enabled = false;
        hitFXPool.Return(_fx);
    }

}
