// ======================================================================================
// File         : Spawner_Slime.cs
// Author       : nantas 
// Last Change  : 12/20/2011 | 20:43:19 PM | Tuesday,December
// Description  : 
// ======================================================================================



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

public class Spawner_Slime : SpawnerEnemy {
	
	private int aliveSlimeCount = 0;

	public SlimePool slimePool = new SlimePool();
	
	//spawner locations
	[System.NonSerialized]public SpawnLocation topSpawner;
	[System.NonSerialized]public SpawnLocation leftSpawner;
	[System.NonSerialized]public SpawnLocation rightSpawner;
	
	void Awake () {
        topSpawner = GameObject.Find("spawner_top").GetComponent<SpawnLocation>();
        leftSpawner = GameObject.Find("spawner_left").GetComponent<SpawnLocation>();
        rightSpawner = GameObject.Find("spawner_right").GetComponent<SpawnLocation>();
	}
	
	// Use this for initialization
	void Start () {
		slimePool.Init(Game.instance.enemyLayerGround);
	}
	
	
	public override void SpawnAnEnemy () {
		if (aliveSlimeCount < maxEnemyCount) {
            int spawnSelector = Random.Range(1,20);
            if ( spawnSelector < 7 ) {
                SpawnASlimeFrom (leftSpawner);
            }
            if ( spawnSelector >= 7 && spawnSelector <= 13 ) {
                SpawnASlimeFrom (topSpawner);
            }
            if ( spawnSelector > 13 ) {
                SpawnASlimeFrom (rightSpawner);
            }
        }
        Invoke("SpawnAnEnemy", Random.Range(minSpawnTime, maxSpawnTime));
	}



	void SpawnASlimeFrom (SpawnLocation _spawner) {
		float leftBorder = _spawner.transform.position.x - _spawner.width/2;
		float rightBorder = _spawner.transform.position.x + _spawner.width/2;
		Vector2 spawnPos = new Vector2(Random.Range(leftBorder,rightBorder), _spawner.transform.position.y);
        Slime slime = SpawnSlimeAt (spawnPos);
        slime.SetSpawner(this);
        aliveSlimeCount += 1;
        slime.GetIntoField(_spawner.moveDirection);
	}
	
	public Slime SpawnSlimeAt (Vector2 _pos) {
		return slimePool.Request(_pos, Quaternion.identity);
	}
	
	public void DestroyEnemy (Slime _slime) {
		_slime.enabled = false;
        aliveSlimeCount -= 1;
        if (aliveSlimeCount < 0) aliveSlimeCount = 0;
		slimePool.Return(_slime);
	}

}
