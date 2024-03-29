// ======================================================================================
// File         : BtnBuyRevive.cs
// Author       : Wu Jie 
// Last Change  : 10/31/2011 | 16:20:54 PM | Monday,October
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
//
///////////////////////////////////////////////////////////////////////////////

public class BtnBuyRevive : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // serialize properties
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	void Awake () {
        exUIButton uiButton = GetComponent<exUIButton>();
        uiButton.OnButtonRelease += OnButtonRelease;
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonRelease () {
        Game.instance.iapCtrl.BuyRevive();
    }
}

