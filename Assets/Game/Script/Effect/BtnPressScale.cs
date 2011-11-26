// ======================================================================================
// File         : BtnPressScale.cs
// Author       : Wu Jie 
// Last Change  : 11/07/2011 | 21:07:16 PM | Monday,November
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

public class BtnPressScale : MonoBehaviour {

    public Vector3 scaleFrom = Vector3.one;
    public Vector3 scaleTo = new Vector3( 0.8f, 0.8f, 1.0f);
    public exTimebasedCurve scaleCurve;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

	void Awake () {
        exUIButton uiButton = GetComponent<exUIButton>();
        uiButton.OnButtonPress += OnButtonPress;
        uiButton.OnButtonRelease += OnButtonRelease;
        uiButton.OnHoverOut += OnHoverOut;
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Reset () {
        enabled = false;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonPress () {
        enabled = true;
        scaleCurve.Inverse(false);
        scaleCurve.Start();
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonRelease () {
       // enabled = true;
       // scaleCurve.Inverse(true);
       // scaleCurve.Start();
       enabled = false;
       transform.localScale = scaleFrom;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnHoverOut () {
        enabled = true;
        scaleCurve.Inverse(true);
        scaleCurve.Start();
    } 

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Update () {
        // if time up, we stop it.
        if ( scaleCurve.IsTimeUp() ) {
            enabled = false;
            transform.localScale = scaleTo;
        }

        //
        float v = scaleCurve.Step();
        transform.localScale = exEase.Lerp ( scaleFrom, scaleTo, v );
    }
}

