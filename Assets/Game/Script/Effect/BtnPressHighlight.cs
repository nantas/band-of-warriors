// ======================================================================================
// File         : BtnPressHighlight.cs
// Author       : Wu Jie 
// Last Change  : 10/31/2011 | 12:04:32 PM | Monday,October
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

public class BtnPressHighlight : MonoBehaviour {

    ///////////////////////////////////////////////////////////////////////////////
    // serialize properties
    ///////////////////////////////////////////////////////////////////////////////

	public exSpriteFont spriteText;
	public Material additiveMaterial;

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

	private Material originalMaterial;

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
        originalMaterial = spriteText.renderer.material;
	}

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void Reset () {
        Transform textTrans = transform.Find("Board/Text");
        if ( textTrans != null ) {
            spriteText = textTrans.GetComponent<exSpriteFont>();
        }
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonPress () {
		originalMaterial = spriteText.renderer.material;
		spriteText.renderer.material = additiveMaterial;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnButtonRelease () {
        if ( spriteText.renderer.material != originalMaterial )
            spriteText.renderer.material = originalMaterial;
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    void OnHoverOut () {
        if ( spriteText.renderer.material != originalMaterial )
            spriteText.renderer.material = originalMaterial;
    } 
}
