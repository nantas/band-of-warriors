// ======================================================================================
// File         : Spawner_BigSlime.cs
// Author       : nantas 
// Last Change  : 12/20/2011 | 20:43:19 PM | Tuesday,December
// Description  : 
// ======================================================================================



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BigSlimePool {

    public int size;
    public GameObject prefab;

    private BigSlime[] initBigSlimes;
    private int idx = 0;
    private BigSlime[] bigSlimes;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initBigSlimes = new BigSlime[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initBigSlimes[i] = obj.GetComponent<BigSlime>();
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
        bigSlimes = new BigSlime[size];
        for ( int i = 0; i < size; ++i ) {
            bigSlimes[i] = initBigSlimes[i];
            bigSlimes[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public BigSlime Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        BigSlime result = bigSlimes[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( BigSlime _slime ) {
        ++idx;
        bigSlimes[idx] = _slime;
    }
}


[System.Serializable]
public class FastSlimePool {

    public int size;
    public GameObject prefab;

    private FastSlime[] initFastSlimes;
    private int idx = 0;
    private FastSlime[] fastSlimes;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initFastSlimes = new FastSlime[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initFastSlimes[i] = obj.GetComponent<FastSlime>();
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
        fastSlimes = new FastSlime[size];
        for ( int i = 0; i < size; ++i ) {
            fastSlimes[i] = initFastSlimes[i];
            fastSlimes[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public FastSlime Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        FastSlime result = fastSlimes[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( FastSlime _slime ) {
        ++idx;
        fastSlimes[idx] = _slime;
    }
}


public class Spawner_BigSlime : MonoBehaviour {
	
	public int maxBigSlimeCount = 3;
    public int maxFastSlimeCount = 6;

	private int aliveBigSlimeCount = 0;
    private int totalBigSlimeSpawned = 0;
    private int aliveFastSlimeCount = 0;
    private int totalFastSlimeSpawned = 0;

	public BigSlimePool bigSlimePool = new BigSlimePool();
    public FastSlimePool fastSlimePool = new FastSlimePool();
	
	//spawner locations
	[System.NonSerialized]public SpawnLocation leftSpawner;
	[System.NonSerialized]public SpawnLocation rightSpawner;
	
	void Awake () {
		bigSlimePool.Init(Game.instance.enemyLayerGround);
        leftSpawner = GameObject.Find("spawner_left").GetComponent<SpawnLocation>();
        rightSpawner = GameObject.Find("spawner_right").GetComponent<SpawnLocation>();
	}
	
	// Use this for initialization
	void Start () {
		Invoke("SpawnASlime", 2.0f);
	}
	
	
	void SpawnASlime () {
		if (aliveBigSlimeCount < maxBigSlimeCount) {
            int spawnSelector = Random.Range(1,2);
            if ( spawnSelector == 1 ) {
                SpawnASlimeFrom (leftSpawner);
            }
            if ( spawnSelector == 2 ) {
                SpawnASlimeFrom (rightSpawner);
            }
            //set new maxSlimeCount
            totalBigSlimeSpawned += 1;
            if (totalBigSlimeSpawned >= 15 && totalBigSlimeSpawned < 30) {
                maxBigSlimeCount = 5;
            } else if (totalBigSlimeSpawned >= 30 && totalBigSlimeSpawned < 55) {
                maxBigSlimeCount = 8;
            } else if (totalBigSlimeSpawned >= 55 && totalBigSlimeSpawned < 90) {
                maxBigSlimeCount = 11;
            } else if (totalBigSlimeSpawned >= 90) {
                maxBigSlimeCount = 15;
            }
        }
        Invoke("SpawnASlime", Random.Range(0.5f, 1.0f));
	}



	void SpawnASlimeFrom (SpawnLocation _spawner) {
		float leftBorder = _spawner.transform.position.x - _spawner.width/2;
		float rightBorder = _spawner.transform.position.x + _spawner.width/2;
		Vector2 spawnPos = new Vector2(Random.Range(leftBorder,rightBorder), _spawner.transform.position.y);
        BigSlime slime = SpawnSlimeAt (spawnPos);
        slime.SetSpawner(this);
        aliveBigSlimeCount += 1;
        slime.GetIntoField(MoveDir.Left);
	}
	
	public BigSlime SpawnSlimeAt (Vector2 _pos) {
		return bigSlimePool.Request(_pos, Quaternion.identity);
	}
	
	public void DestroySlime (BigSlime _slime) {
		_slime.enabled = false;
        aliveBigSlimeCount -= 1;
        if (aliveBigSlimeCount < 0) aliveBigSlimeCount = 0;
		bigSlimePool.Return(_slime);
	}

    public void DestroyFastSlime (FastSlime _slime) {
        _slime.enabled = false;
        aliveFastSlimeCount -= 1;
        if (aliveFastSlimeCount <0) aliveFastSlimeCount = 0;
        fastSlimePool.Return(_slime);
    }


}

