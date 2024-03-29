// ======================================================================================
// File         : ProgressBar.cs
// Author       : Wu Jie 
// Last Change  : 10/03/2011 | 14:28:43 PM | Monday,October
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
///
/// ProgressBar
///
///////////////////////////////////////////////////////////////////////////////

public class ProgressBar : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // properties
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    [SerializeField] float ratio_ = 1.0f;
    public float ratio {
        get { return ratio_; }
        set {
            ratio_ = Mathf.Clamp( ratio_, 0.0f, 1.0f );
            if ( ratio_ != value ) {
                ratio_ = value;
                clipPlane.width = totalWidth * ratio_;
            }
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    exSoftClip clipPlane;
    float totalWidth = 0.0f;

    ///////////////////////////////////////////////////////////////////////////////
    // functions
    ///////////////////////////////////////////////////////////////////////////////

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Awake () {
        clipPlane = GetComponent<exSoftClip>();
        if (clipPlane) {
            //Debug.Log(clipPlane);
        }
        totalWidth = clipPlane.width;
        clipPlane.width = totalWidth * ratio_;
    }
}

