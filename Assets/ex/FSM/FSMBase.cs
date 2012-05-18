// ======================================================================================
// File         : FSMBase.cs
// Author       : Wu Jie 
// Last Change  : 04/19/2012 | 23:18:35 PM | Thursday,April
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

///////////////////////////////////////////////////////////////////////////////
// \class FSMBase 
// 
// \brief 
// 
///////////////////////////////////////////////////////////////////////////////

public class FSMBase : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // non-serialized
    ///////////////////////////////////////////////////////////////////////////////

    [System.NonSerialized] public fsm.Machine stateMachine;

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected void Awake () {
        stateMachine = new fsm.Machine();
        InitStateMachine ();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public virtual void Reset () {
        if ( stateMachine != null )
            stateMachine.Restart();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    protected virtual void InitStateMachine () {}
}
