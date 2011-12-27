// ======================================================================================
// File         : SpawnerEnemy.cs
// Author       : nantas 
// Last Change  : 12/27/2011 | 15:02:38 PM | Tuesday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class SpawnerEnemy : MonoBehaviour {
    [System.NonSerialized] public LevelManager levelManager;
    public int maxEnemyCount = 1;
    public float minSpawnTime = 2.0f;
    public float maxSpawnTime = 4.0f;


    public virtual void SpawnAnEnemy() {
    }

}


