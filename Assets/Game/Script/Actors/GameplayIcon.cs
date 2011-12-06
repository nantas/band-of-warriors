using UnityEngine;
using System.Collections;

public class GameplayIcon : MonoBehaviour {
    
    public string iconFileName;

    void OnDrawGizmos () {
        Gizmos.DrawIcon (transform.position, iconFileName);
    }

}
