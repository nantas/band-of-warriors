using UnityEngine;
using System.Collections;

public enum MoveDir {
	Left,
	Right,
	Up,
	Down,
	Stop
}

public class Game : MonoBehaviour {

    protected static Game instance_ = null; 
    public static Game instance {
        get {
            if ( instance_ == null ) {
                instance_ = FindObjectOfType ( typeof(Game) ) as Game;
                if ( instance_ != null )
                    instance_.Init();
            }
            return instance_;
        }
    }
	
	[System.NonSerialized] public Spawner theSpawner;
	
	public WarriorControl theplayer;
	public exLayer enemyLayer;
	public float leftBoundary = -480;
	public float rightBoundary = 480;
	public float groundPosY = -130;
	
	protected virtual void Init () {
		theSpawner = GetComponent<Spawner>();
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
