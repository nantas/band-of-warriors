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
	public Transform leftBoundary;
	public Transform rightBoundary;
    public Transform leftSpawnEntry;
    public Transform rightSpawnEntry;
	
    public WarriorControl theplayer;
	public exLayer enemyLayerGround;
    public exLayer enemyLayerAir;
	public float groundPosY = -130;
    public float flyPosY = 100;
	
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
