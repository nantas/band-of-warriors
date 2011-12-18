// ======================================================================================
// File         : PlayerFSMActions.cs
// Author       : nantas 
// Last Change  : 12/17/2011 | 15:13:56 PM | Saturday,December
// Description  : handles player move, jump, action states update 
// ======================================================================================



using UnityEngine;
using System.Collections;

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
            if (!(targetJumpState.IsNone || string.IsNullOrEmpty(targetJumpState.Value))) {
                playerController.charJumpState = (JumpState)System.Enum.Parse(typeof(JumpState), targetJumpState.Value, false);
            }
            if (!(targetHurtState.IsNone || string.IsNullOrEmpty(targetHurtState.Value))){
                playerController.charHurtState = (HurtState)System.Enum.Parse(typeof(HurtState), targetHurtState.Value, false);
            }
            if (!(targetActionState.IsNone || string.IsNullOrEmpty(targetActionState.Value))) {
                playerController.charActionState = (ActionState)System.Enum.Parse(typeof(ActionState), targetActionState.Value, false);
            }
            Finish();
        }


    }

	[ActionCategory("BOW_Player")]
	[Tooltip("Sets the value of a Float Variable to match the value of a variable from a script.")]
	public class InitVariables : FsmStateAction
	{
		public FsmOwnerDefault gameObject;
        [RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmFloat floatDashDuration;
        public FsmFloat floatAirDashDuration;
        public FsmFloat floatStunDuration;
        public FsmFloat floatFlashDuration;

		public PlayerBase playerScript;

		public override void Reset()
		{
			floatDashDuration = null;
            floatAirDashDuration = null;
			playerScript = null;
		}

		public override void OnEnter()
		{
            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
            playerScript = go.GetComponent<PlayerBase>();
            floatDashDuration.Value = (float) playerScript.dashDuration; 
            floatAirDashDuration.Value = (float) playerScript.airDashDuration;
            floatStunDuration.Value = (float) playerScript.stunTime;
            floatFlashDuration.Value = (float) playerScript.flashTime;
		    Finish();		
		}
	}

}
