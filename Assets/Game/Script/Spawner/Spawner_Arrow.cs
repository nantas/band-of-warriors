// ======================================================================================
// File         : Spawner_Arrow.cs
// Author       : Wu Jie 
// Last Change  : 01/01/2012 | 22:01:25 PM | Sunday,January
// Description  : 
// ======================================================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class ArrowPool {

    public int size;
    public GameObject prefab;

    private Arrow[] initArrows;
    private int idx = 0;
    private Arrow[] arrows;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Init ( exLayer _layer ) {
        initArrows = new Arrow[size]; 
        if ( prefab != null ) {
            for ( int i = 0; i < size; ++i ) {
                GameObject obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
                initArrows[i] = obj.GetComponent<Arrow>();
				if (obj.GetComponent<exLayer>()) {
                	obj.GetComponent<exLayer>().parent = _layer;
				} else {
					Debug.LogError ("please add a layer component to arrow prefab.");
				}
            }
        }
        Reset();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Reset () {
        arrows = new Arrow[size];
        for ( int i = 0; i < size; ++i ) {
            arrows[i] = initArrows[i];
            arrows[i].enabled = false;
        }
        idx = size-1;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public Arrow Request ( Vector3 _pos, Quaternion _rot )  {
        if ( idx < 0 )
            Debug.LogError ("Error: the pool do not have enough free item.");

        Arrow result = arrows[idx];
        --idx; 

        result.transform.position = new Vector3 ( _pos.x, _pos.y, result.transform.position.z );
        result.transform.rotation = _rot;
        result.enabled = true;
        return result;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public void Return ( Arrow _arrow ) {
        ++idx;
        if ( idx >= size )
            idx = size - 1;
        arrows[idx] = _arrow;
    }
}


///////////////////////////////////////////////////////////////////////////////
// class 
// 
// Purpose: 
// 
///////////////////////////////////////////////////////////////////////////////

public class Spawner_Arrow : MonoBehaviour {

    public int aliveArrowCount = 0;
    public ArrowPool arrowPool = new ArrowPool();

	void Awake () {
        //Debug.Log("init arrows.");
        arrowPool.Init(Game.instance.fxLayer);
	}

    public Arrow SpawnArrowAt (Vector2 _pos) {
        aliveArrowCount += 1;
        return arrowPool.Request(_pos, Quaternion.identity);
    }

    public void DestroyArrow (Arrow _arrow) {
        aliveArrowCount -= 1;
        if (aliveArrowCount < 0) aliveArrowCount = 0;
        _arrow.enabled = false;
        arrowPool.Return(_arrow);
    }

}

