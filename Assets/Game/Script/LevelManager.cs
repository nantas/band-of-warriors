// ======================================================================================
// File         : LevelManager.cs
// Author       : nantas 
// Last Change  : 12/24/2011 | 16:15:36 PM | Saturday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    
    //SpawnerInfo is the spawner needed for a certain type of enemy
    //also it will contains information of max alive and spawn interval.
    [System.Serializable]
    public class SpawnerInfo {
        public EnemyClass enemyType;
        public int maxAliveCount;
        public float minSpawnInterval;
        public float maxSpawnInterval;
    }

    //LevelInfo contains information about in a certain level,
    //which SpawnerInfo are used and what's the mission for the level.
    //usually kill certain numbers of a certain enemy.
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

    void Awake () {
        Init();
        Invoke("StartLevel", 2.0f);
        Invoke("HealthPackTimer", 35.0f);
    }

    public void Init () {
        SpawnerEnemy[] spawners;
        spawners = spawnerContainer.transform.GetComponentsInChildren<SpawnerEnemy>();
        foreach (SpawnerEnemy spawner in spawners) {
            spawner.levelManager = this;
        }
    }

    public void HealthPackTimer() {
        ItemCarrier itemCarrier = Game.instance.theItemCarrier;
        if (itemCarrier.moveDir == MoveDir.Stop) {
            itemCarrier.SpawnItemCarrier();
        }
    }

    public void OnEnemyKilled (EnemyClass _killedType) {
        if (currentLevel < levelInfos.Length) {
            EnemyClass targetType = levelInfos[currentLevel].targetEnemy;
            int targetNumber = levelInfos[currentLevel].targetKillNum;
            if ( _killedType == targetType || targetType == EnemyClass.AnyEnemy ) {
                currentKillNum += 1;
                Game.instance.theGamePanel.OnMissionUpdate(targetType, 
                                                   (targetNumber-currentKillNum));
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
        //update hud display.
        Game.instance.theGamePanel.OnStageUpdate(currentLevel);
        Game.instance.theGamePanel.OnMissionUpdate(levelInfos[currentLevel].targetEnemy, 
                                  (levelInfos[currentLevel].targetKillNum-currentKillNum));
        //get SpawnerInfo for the current level.
        SpawnerInfo[] spawnerInfos = levelInfos[currentLevel].spawnerInfo;
        if ( spawnerInfos == null ) {
            Debug.LogError("no available spawn info in LevelInfo class!");
        } else {
            foreach (SpawnerInfo spawnerInfo in spawnerInfos) {
                //invoke spawn for each enemy spawner 
                SpawnerEnemy spawnerA = GetSpawnerFromEnemyType(spawnerInfo.enemyType);
                spawnerA.maxEnemyCount = spawnerInfo.maxAliveCount;
                spawnerA.minSpawnTime = spawnerInfo.minSpawnInterval;
                spawnerA.maxSpawnTime = spawnerInfo.maxSpawnInterval;
                spawnerA.SpawnAnEnemy();
            }
        }
    }

    //input EnemyClass, output Spawner for the enemy.
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
        //TODO level complete display
        CancelAllSpawn();
        Game.instance.theGamePanel.ShowLevelCompleteText();
        Debug.Log("LevelComplete");
    }

}
