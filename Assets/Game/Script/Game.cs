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
    [System.NonSerialized] public ScoreCounter theScoreCounter;
    public GamePanel theGamePanel;
	public Transform leftBoundary;
	public Transform rightBoundary;
    public Transform leftSpawnEntry;
    public Transform rightSpawnEntry;
    public int initPlayerHP = 50;	
    public WarriorControl theplayer;
	public exLayer enemyLayerGround;
    public exLayer enemyLayerAir;
    public exLayer coinLayer;
    public exLayer scoreLayer;
	public float groundPosY = -130;
    public float flyPosY = 100;
    public float gravity = -300.0f;

    private int playerHP;
    private int playerLvl;
	
	protected virtual void Init () {
		theSpawner = GetComponent<Spawner>();
        theScoreCounter = GetComponent<ScoreCounter>();
		
	}

	// Use this for initialization
	void Start () {
        playerHP = initPlayerHP;
        playerLvl = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlayerHPChange(int _amount) {
        playerHP -= _amount;
        if (playerHP <= 0) {
            playerHP = 0;
            //TODO: gameover
        }
        theGamePanel.HPbar.ratio = ((float)playerHP)/((float)initPlayerHP);
    }

    public void OnPlayerExpChange(int _amount) {
        //TODO: add level up table and logic
    }
}
