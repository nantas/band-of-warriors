// ======================================================================================
// File         : PlayerFSMActions.cs
// Author       : nantas 
// Last Change  : 12/17/2011 | 15:13:56 PM | Saturday,December
// Description  : handles player move, jump, action states update 
// ======================================================================================



using UnityEngine;
using System.Collections;
using System;

namespace HutongGames.PlayMaker.Actions {
	//Enemy Move Action
    [ActionCategory("BOW_Player")]
    [Tooltip("This action updates player states in FSM")]
    public class PlayerStateUpdate: FsmStateAction {

        private PlayerController playerController;
        public FsmOwnerDefault gameObject; 
        public FsmString targetJumpState;
        public FsmString targetActionState;
        public FsmString targetHurtState;


        public override void Reset () {
        }

        public override void OnEnter() {
            playerController = Fsm.GetOwnerDefaultTarget(gameObject)
                .GetComponent<PlayerController>() as PlayerController;
            playerController.charJumpState = (JumpState)Enum.Parse(typeof(JumpState), targetJumpState.Value, false);
            playerController.charHurtState = (HurtState)Enum.Parse(typeof(HurtState), targetHurtState.Value, false);
            playerController.charActionState = (ActionState)Enum.Parse(typeof(ActionState), targetActionState.Value, false);
            Finish();
        }


    }

	

}
