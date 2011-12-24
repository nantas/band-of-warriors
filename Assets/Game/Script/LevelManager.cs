// ======================================================================================
// File         : LevelManager.cs
// Author       : nantas 
// Last Change  : 12/24/2011 | 16:15:36 PM | Saturday,December
// Description  : 
// ======================================================================================

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public Spawner commonSpawner;
    [System.NonSerialized]public Spawner_Slime slimeSpawner;
    [System.NonSerialized]public Spawner_Flymon flymonSpawner;
    [System.NonSerialized]public Spawner_BigSlime bigSlimeSpawner;
    [System.NonSerialized]public Spawner_Cubat cubatSpawner;
    [System.NonSerialized]public PlayMakerFSM FSM_Level;

    void Awake () {
        slimeSpawner = GetComponent<Spawner_Slime>();
        flymonSpawner = GetComponent<Spawner_Flymon>();
        bigSlimeSpawner = GetComponent<Spawner_BigSlime>();
        cubatSpawner = GetComponent<Spawner_Cubat>();
        FSM_Level = GetComponent<PlayMakerFSM>();
    }

    public int GetMaxSlimeCount() {
        return FSM_Level.FsmVariables.GetFsmInt("maxSlimeCount").Value;
    }

    public int GetMaxFlymonCount() {
        return FSM_Level.FsmVariables.GetFsmInt("maxFlymonCount").Value;
    }

    public int GetMaxBigSlimeCount() {
        return FSM_Level.FsmVariables.GetFsmInt("maxBigSlimeCount").Value;
    }

    public int GetMaxCubatCount() {
        return FSM_Level.FsmVariables.GetFsmInt("maxCubatCount").Value;
    }

    public void GetAllMaxCount() {
        slimeSpawner.maxSlimeCount = GetMaxSlimeCount();
        flymonSpawner.maxFlymonCount = GetMaxFlymonCount();
        bigSlimeSpawner.maxBigSlimeCount = GetMaxBigSlimeCount();
        cubatSpawner.maxCubatCount = GetMaxCubatCount();
    }


    public void OnPlayerLevelChanged() {
        FSM_Level.FsmVariables.GetFsmInt("playerLevel").Value = Game.instance.playerLvl;
        //TODO: display level up on game panel
        Game.instance.theGamePanel.ShowLevelUpText();
    }


    public void CancelAllSpawn() {
        slimeSpawner.CancelInvoke();
        flymonSpawner.CancelInvoke();
        bigSlimeSpawner.CancelInvoke();
        cubatSpawner.CancelInvoke();
    }

    public void StartLevel01() {
        CancelAllSpawn();
        slimeSpawner.maxSlimeCount = GetMaxSlimeCount();
        slimeSpawner.SpawnASlime();
    }

    public void StartLevel02() {
        CancelAllSpawn();
        slimeSpawner.maxSlimeCount = GetMaxSlimeCount();
        flymonSpawner.maxFlymonCount = GetMaxFlymonCount();
        slimeSpawner.SpawnASlime();
        flymonSpawner.SpawnAFlymon();
    }

    public void StartLevel03() {
        CancelAllSpawn();
        slimeSpawner.maxSlimeCount = GetMaxSlimeCount();
        flymonSpawner.maxFlymonCount = GetMaxFlymonCount();
        slimeSpawner.SpawnASlime();
        flymonSpawner.SpawnAFlymon();
    }

    public void StartLevel04() {
        CancelAllSpawn();
        slimeSpawner.maxSlimeCount = GetMaxSlimeCount();
        flymonSpawner.maxFlymonCount = GetMaxFlymonCount();
        slimeSpawner.SpawnASlime();
        flymonSpawner.SpawnAFlymon();
    }

    public void StartLevel05() {
        CancelAllSpawn();
        GetAllMaxCount();
        slimeSpawner.SpawnASlime();
        flymonSpawner.SpawnAFlymon();
        bigSlimeSpawner.SpawnABigSlime();
    }

    public void StartLevel06() {
        CancelAllSpawn();
        GetAllMaxCount();
        slimeSpawner.SpawnASlime();
        flymonSpawner.SpawnAFlymon();
        bigSlimeSpawner.SpawnABigSlime();
    }

    public void StartLevel07() {
        CancelAllSpawn();
        GetAllMaxCount();
        slimeSpawner.SpawnASlime();
        bigSlimeSpawner.SpawnABigSlime();
        cubatSpawner.SpawnACubat();
    }

    public void StartLevel08() {
        CancelAllSpawn();
        GetAllMaxCount();
        slimeSpawner.SpawnASlime();
        bigSlimeSpawner.SpawnABigSlime();
        cubatSpawner.SpawnACubat();
    }

    public void StartLevel09() {
        CancelAllSpawn();
        GetAllMaxCount();
        slimeSpawner.SpawnASlime();
        flymonSpawner.SpawnAFlymon();
        bigSlimeSpawner.SpawnABigSlime();
        cubatSpawner.SpawnACubat();
    }

    public void StartLevel10() {
        CancelAllSpawn();
        GetAllMaxCount();
        slimeSpawner.SpawnASlime();
        flymonSpawner.SpawnAFlymon();
        bigSlimeSpawner.SpawnABigSlime();
        cubatSpawner.SpawnACubat();
    }


    public void QuestComplete() {
        Debug.Log("QuestComplete");
    }

}
