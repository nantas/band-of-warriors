// ======================================================================================
// File         : Spawner_Cubat.cs
// Author       : nantas 
// Last Change  : 12/20/2011 | 21:50:35 PM | Tuesday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CubatPool {

    public int size;
    public GameObject prefab;

    private Cubat[] initCubats;
    private int idx = 0;
    private Cubat[] cubats;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initCubats = new Cubat[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero,
                                                        Quaternion.identity) as GameObject;
                initCubats[i] = obj.GetComponent<Cubat>();
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
        cubats = new Cubat[size];
        for ( int i = 0; i < size; ++i ) {
            cubats[i] = initCubats[i];
            cubats[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public Cubat Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        Cubat result = cubats[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( Cubat _cubat ) {
        ++idx;
        cubats[idx] = _cubat;
    }
}



///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class Spawner_Cubat : MonoBehaviour {
	
    public int maxCubatCount = 3;

    private int aliveCubatCount = 0;
    private int totalCubatSpawned = 0;

    public CubatPool cubatPool = new CubatPool();
	
	//spawner locations
	[System.NonSerialized]public SpawnLocation topSpawner;
	[System.NonSerialized]public SpawnLocation leftSpawner;
	[System.NonSerialized]public SpawnLocation rightSpawner;
	
	void Awake () {
        cubatPool.Init(Game.instance.enemyLayerAir);
        topSpawner = GameObject.Find("spawner_top").GetComponent<SpawnLocation>();
        leftSpawner = GameObject.Find("spawner_left").GetComponent<SpawnLocation>();
        rightSpawner = GameObject.Find("spawner_right").GetComponent<SpawnLocation>();

	}
	
	// Use this for initialization
	void Start () {
        Invoke("SpawnACubat", 5.0f);
	}
	
	void SpawnACubat () {
        float maxRanTime = 3.0f;
        if (aliveCubatCount < maxCubatCount) {
            int spawnSelector = Random.Range(1,3);
            switch (spawnSelector) {
                case 1:
                    SpawnACubatFrom (topSpawner);
                    break;
                case 2:
                    SpawnACubatFrom (leftSpawner);
                    break;
                case 3:
                    SpawnACubatFrom (rightSpawner);
                    break;
                default:
                    Debug.LogError("can't get a valid spawner");
                    break;
            }
            totalCubatSpawned += 1;
            if (totalCubatSpawned >= 10 && totalCubatSpawned < 25) {
                maxCubatCount = 3;
                maxRanTime = 2.5f;
            } else if (totalCubatSpawned >= 25 && totalCubatSpawned < 50) {
                maxCubatCount = 4;
                maxRanTime = 2.0f;
            } else if (totalCubatSpawned >= 50 ) {
                maxCubatCount = 4;
                maxRanTime = 1.5f;
            }
        }
        Invoke("SpawnACubat", Random.Range(1.0f, maxRanTime));
	}	

    public Cubat SpawnCubatAt (Vector2 _pos) {
        return cubatPool.Request(_pos, Quaternion.identity);
    }

    void SpawnACubatFrom (SpawnLocation _spawner) {
       	float leftBorder = _spawner.transform.position.x - _spawner.width/2;
		float rightBorder = _spawner.transform.position.x + _spawner.width/2;
		Vector2 spawnPos = new Vector2(Random.Range(leftBorder,rightBorder), _spawner.transform.position.y);
        Cubat cubat = SpawnCubatAt (spawnPos);
        cubat.SetSpawner(this);
        aliveCubatCount += 1;
        cubat.GetIntoField();
	}


    public void DestroyCubat (Cubat _cubat) {
        _cubat.enabled = false;
        aliveCubatCount -= 1;
        if (aliveCubatCount < 0) aliveCubatCount = 0;
        cubatPool.Return(_cubat);
    }

}

