// ======================================================================================
// File         : Spawner_Flymon.cs
// Author       : nantas 
// Last Change  : 12/20/2011 | 21:50:35 PM | Tuesday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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



///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class Spawner_Flymon : SpawnerEnemy {
	
    private int aliveFlymonCount = 0;

    public FlymonPool flymonPool = new FlymonPool();
	
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
        flymonPool.Init(Game.instance.enemyLayerAir);
	}
	
	public override void SpawnAnEnemy () {
        if (aliveFlymonCount < maxEnemyCount) {
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
        }
        Invoke("SpawnAnEnemy", Random.Range(minSpawnTime, maxSpawnTime));
	}	

    public Flymon SpawnFlymonAt (Vector2 _pos) {
        return flymonPool.Request(_pos, Quaternion.identity);
    }

    void SpawnAFlymonFrom (SpawnLocation _spawner) {
       	float leftBorder = _spawner.transform.position.x - _spawner.width/2;
		float rightBorder = _spawner.transform.position.x + _spawner.width/2;
		Vector2 spawnPos = new Vector2(Random.Range(leftBorder,rightBorder), _spawner.transform.position.y);
        Flymon flymon = SpawnFlymonAt (spawnPos);
        flymon.SetSpawner(this);
        aliveFlymonCount += 1;
        flymon.GetIntoField();
	}


    public void DestroyEnemy (Flymon _flymon) {
        _flymon.enabled = false;
        aliveFlymonCount -= 1;
        if (aliveFlymonCount < 0) aliveFlymonCount = 0;
        flymonPool.Return(_flymon);
    }

}
