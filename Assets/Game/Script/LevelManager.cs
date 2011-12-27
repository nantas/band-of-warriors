// ======================================================================================
// File         : LevelManager.cs
// Author       : nantas 
// Last Change  : 12/24/2011 | 16:15:36 PM | Saturday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    
    [System.Serializable]
    public class SpawnerInfo {
        public EnemyClass enemyType;
        public int maxAliveCount;
        public float minSpawnInterval;
        public float maxSpawnInterval;
    }

    [System.Serializable]
    public class LevelInfo {
        public SpawnerInfo[] spawnerInfo;
        public EnemyClass targetEnemy;
        public int targetKillNum;
    }

    public Spawner commonSpawner;
    public GameObject spawnerContainer;
    public LevelInfo[] levelInfos;
    [System.NonSerialized]public int currentLevel = 0;
    [System.NonSerialized]public int currentKillNum = 0;

    [System.NonSerialized]public PlayMakerFSM FSM_Level;

    void Awake () {
        Init();
        Invoke("StartLevel", 2.0f);
    }

    public void Init () {
        SpawnerEnemy[] spawners;
        spawners = spawnerContainer.transform.GetComponentsInChildren<SpawnerEnemy>();
        foreach (SpawnerEnemy spawner in spawners) {
            spawner.levelManager = this;
        }
    }

    public void OnEnemyKilled (EnemyClass _killedType) {
        if (currentLevel < levelInfos.Length) {
            EnemyClass targetType = levelInfos[currentLevel].targetEnemy;
            int targetNumber = levelInfos[currentLevel].targetKillNum;
            if ( _killedType == targetType || targetType == EnemyClass.AnyEnemy ) {
                currentKillNum += 1;
                if (currentKillNum >= targetNumber) {
                    GoLevelUp();
                }
            }
        }
    }

    public void GoLevelUp() {
        currentLevel += 1;
        if (currentLevel < levelInfos.Length) {
            currentKillNum = 0;
            CancelAllSpawn();
            StartLevel();
        } else {
            CancelAllSpawn();
            LevelComplete();
        }
    }

    public void StartLevel() {
        Game.instance.theGamePanel.OnStageUpdate(currentLevel);
        SpawnerInfo[] spawnerInfos = levelInfos[currentLevel].spawnerInfo;
        if ( spawnerInfos == null ) {
            Debug.LogError("no available spawn info in LevelInfo class!");
        } else {
            foreach (SpawnerInfo spawnerInfo in spawnerInfos) {
                SpawnerEnemy spawnerA = GetSpawnerFromEnemyType(spawnerInfo.enemyType);
                spawnerA.maxEnemyCount = spawnerInfo.maxAliveCount;
                spawnerA.minSpawnTime = spawnerInfo.minSpawnInterval;
                spawnerA.maxSpawnTime = spawnerInfo.maxSpawnInterval;
                spawnerA.SpawnAnEnemy();
            }
        }
    }

    public SpawnerEnemy GetSpawnerFromEnemyType (EnemyClass _enemyType) {
        if (_enemyType == EnemyClass.Slime) {
            return spawnerContainer.GetComponentInChildren<Spawner_Slime>();
        } else
        if (_enemyType == EnemyClass.Flymon) {
            return spawnerContainer.GetComponentInChildren<Spawner_Flymon>();
        } else
        if (_enemyType == EnemyClass.BigSlime || _enemyType == EnemyClass.FastSlime ) {
            return spawnerContainer.GetComponentInChildren<Spawner_BigSlime>();
        } else
        if (_enemyType == EnemyClass.Cubat) {
            return spawnerContainer.GetComponentInChildren<Spawner_Cubat>();
        } else {
            Debug.LogError("invalid enemy type, can't get spawner.");
            return null;
        }
    }

    public void CancelAllSpawn() {
        SpawnerEnemy[] spawners = spawnerContainer.GetComponentsInChildren<SpawnerEnemy>();
        foreach ( SpawnerEnemy spawner in spawners ) {
            spawner.CancelInvoke();
        }
    }

    public void LevelComplete() {
        Debug.Log("LevelComplete");
    }

}