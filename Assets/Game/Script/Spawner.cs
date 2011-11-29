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


public class Spawner : MonoBehaviour {
	
	public int maxSlimeCount = 10;
	private int aliveSlimeCount = 0;
    private int totalSlimeSpawned = 0;

	public SlimePool slimePool = new SlimePool();
	
	//spawner locations
	public SpawnLocation topSpawner;
	public SpawnLocation botSpawner;
	public SpawnLocation leftSpawner;
	public SpawnLocation rightSpawner;
	
	void Awake () {
		slimePool.Init(Game.instance.enemyLayer);
	}
	
	// Use this for initialization
	void Start () {
		Invoke("SpawnASlime", 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SpawnASlime () {
		int spawnSelector = Random.Range(1,4);
		switch (spawnSelector) {
			case 1:
				SpawnASlimeFrom (topSpawner);
				break;
			case 2:
				SpawnASlimeFrom (botSpawner);
				break;
			case 3:
				SpawnASlimeFrom (leftSpawner);
				break;
			case 4:
				SpawnASlimeFrom (rightSpawner);
				break;
			default:
				Debug.LogError("can't get a valid spawner");
				break;
		}
        totalSlimeSpawned += 1;
        if (totalSlimeSpawned > 15) {
            Invoke("SpawnASlime", Random.Range(0.7f, 1.2f));
        } else {
            Invoke("SpawnASlime", Random.Range(1.5f, 2.0f));
        }
	}
	
	void SpawnASlimeFrom (SpawnLocation _spawner) {
		float leftBorder = _spawner.transform.position.x - _spawner.width/2;
		float rightBorder = _spawner.transform.position.x + _spawner.width/2;
		Vector2 spawnPos = new Vector2(Random.Range(leftBorder,rightBorder), _spawner.transform.position.y);
		if (aliveSlimeCount <= maxSlimeCount) {
			Slime slime = SpawnSlimeAt (spawnPos);
			slime.GetIntoField(_spawner.moveDirection);
		}
	}
	
	public Slime SpawnSlimeAt (Vector2 _pos) {
		return slimePool.Request(_pos, Quaternion.identity);
	}
	
	public void DestroySlime (Slime _slime) {
		_slime.enabled = false;
		slimePool.Return(_slime);
	}
}
