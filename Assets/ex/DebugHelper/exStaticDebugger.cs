// ======================================================================================
// File         : exStaticDebugger.cs
// Author       : Wu Jie 
// Last Change  : 02/19/2012 | 21:20:42 PM | Sunday,February
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

public static class exStaticDebugger {

    ///////////////////////////////////////////////////////////////////////////////
    //
    ///////////////////////////////////////////////////////////////////////////////

    public static void Assert ( bool _condition, string _msg, bool _break = false ) {
        if ( _condition == false ) {
            Debug.LogError ( "Assert Failed: " + _msg );
            if ( _break )
                Debug.Break ();
        }
    } 
}

