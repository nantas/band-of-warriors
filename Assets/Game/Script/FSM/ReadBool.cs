using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory(ActionCategory.StateMachine)]
[HutongGames.PlayMaker.Tooltip("Read a bool from another Fsm, either store it value and assign it to another variable in this fsm , or just send event from the value found")]
public class ReadBool : FsmStateAction
{
	[RequiredField]
	public FsmOwnerDefault gameObject;
	[UIHint(UIHint.FsmName)]
	[HutongGames.PlayMaker.Tooltip("Optional name of FSM on Game Object")]
	public FsmString fsmName;
	[RequiredField]
	[UIHint(UIHint.FsmBool)]
	public FsmString variableName;
	[UIHint(UIHint.Variable)]
	public FsmBool storeValue;
	public FsmEvent IsTrue;
	public FsmEvent IsFalse;
	public bool everyFrame;
	
	GameObject goLastFrame;
	PlayMakerFSM fsm;
	
	// cache
	GameObject go;

	public override void Reset ()
	{
		gameObject = null;
		fsmName = "";
		storeValue = null;
		IsTrue = null;
		IsFalse = null;
	}
	
	public override void OnEnter ()
	{
		// get owner reference and cache it here
		go = Fsm.GetOwnerDefaultTarget(gameObject);
		
		DoReadBool();
		
		if(!everyFrame)
			Finish();
	}
	
	void DoReadBool()
	{
		if (go == null) 
			return;
		
		// only get the fsm component if go has changed
		if (go != goLastFrame)
		{
			goLastFrame = go;
			fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
		}			
		
		if (fsm == null) 
			return;
		
		FsmBool fsmBool = fsm.FsmVariables.GetFsmBool(variableName.Value);
		
		if (fsmBool == null) 
			return;
		
		// store in variable if defined
		if(storeValue != null)
			storeValue.Value = fsmBool.Value;
		
		// send event if not null
		if(IsFalse != null && fsmBool.Value == false)
			Fsm.Event(IsFalse);
		else if(IsTrue != null && fsmBool.Value == true)
			Fsm.Event(IsTrue);
			
	}
}
