// ======================================================================================
// File         : AttributeUtility.cs
// Author       : Wang Nan 
// Last Change  : 10/03/2011 | 15:41:01 PM | Monday,October
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

static public class AttributeUtility {

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    [MenuItem ("Assets/Create/BoW/Attribute")]
    public static void Create () {
        Attribute newAsset = Create ( exEditorHelper.GetCurrentDirectory(), "New Attribute" );
        EditorGUIUtility.PingObject(newAsset);
    }

    // ------------------------------------------------------------------ 
    // Desc: 
    // ------------------------------------------------------------------ 

    public static Attribute Create ( string _path, string _name ) {
        //
        if ( new DirectoryInfo(_path).Exists == false ) {
            Debug.LogError ( "can't create asset, path not found" );
            return null;
        }
        if ( string.IsNullOrEmpty(_name) ) {
            Debug.LogError ( "can't create asset, the name is empty" );
            return null;
        }
        string assetPath = Path.Combine( _path, _name + ".asset" );

        //
        Attribute newAsset = ScriptableObject.CreateInstance<Attribute>();
        AssetDatabase.CreateAsset(newAsset, assetPath);
        Selection.activeObject = newAsset;
        return newAsset;
    }
}

