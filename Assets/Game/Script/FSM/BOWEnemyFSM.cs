// ======================================================================================
// File         : BOWEnemyFSM.cs
// Author       : nantas 
// Last Change  : 12/14/2011 | 23:06:16 PM | Wednesday,December
// Description  : playmaker actions for enemy FSM 
// ======================================================================================

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions {
	//Enemy Move Action
    [ActionCategory("BOW_Enemy")]
    [Tooltip("Enemy move around behavior FSM")]
    public class EnemyMoveAround: FsmStateAction {
        
        public FsmVector3 vector3Variable;
        public FsmFloat delayVariable;
        public FsmFloat moveTimeVariable;

        public override void Reset () {

        }

        public override void OnEnter() {
            //generate random parameters
//            float randomX = Random.Range(Game.instance.leftSpawnEntry.position.x, 
//                                         Game.instance.rightSpawnEntry.position.x);
            float randomX = Random.Range(-400.0f, 400.0f);
            vector3Variable.Value = new Vector3( randomX, 0, 0);
            delayVariable.Value = Random.Range(0f, 0.7f);
            moveTimeVariable.Value = Random.Range(1f,2f);
            Finish();
        }


    }

	

}
