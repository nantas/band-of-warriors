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
    [System.NonSerialized] public int playerHP;
    [System.NonSerialized] public int playerLvl;
    [System.NonSerialized] public int currentExp;
    public GamePanel theGamePanel;
	public Transform leftBoundary;
	public Transform rightBoundary;
    public Transform leftSpawnEntry;
    public Transform rightSpawnEntry;
    public int initPlayerHP = 100;	
    public PlayerBase thePlayer;
	public exLayer enemyLayerGround;
    public exLayer enemyLayerAir;
    public exLayer coinLayer;
    public exLayer scoreLayer;
	public float groundPosY = -130;
    public float flyPosY = 100;
    public float gravity = -300.0f;
    public int[] expReqForLvl;


	
	protected virtual void Init () {
		theSpawner = GetComponent<Spawner>();
        theScoreCounter = GetComponent<ScoreCounter>();
		
	}

	// Use this for initialization
	void Start () {
        playerHP = initPlayerHP;
        playerLvl = 1;
        currentExp = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlayerHPChange(int _amount) {
        playerHP += _amount;
        if (playerHP <= 0) {
            playerHP = 0;
            //TODO: gameover
        } else if (playerHP > initPlayerHP) {
            playerHP = initPlayerHP;
        }
        theGamePanel.HPbar.ratio = ((float)playerHP)/((float)initPlayerHP);
    }

    public void OnPlayerExpChange(int _amount) {
        //TODO: add level up table and logic
        currentExp += _amount;
        if ( currentExp >= expReqForLvl[playerLvl-1] ) {
            int extraExp = currentExp - expReqForLvl[playerLvl-1];
            //TODO:put the level handle into on level change function
            playerLvl += 1;
            currentExp = 0;
            OnPlayerLvlUp();
            OnPlayerExpChange(extraExp);
        } else {
            theGamePanel.EXPbar.ratio = ((float)currentExp)/((float)expReqForLvl[playerLvl-1]);
        }
    }

    public void OnPlayerLvlUp() {
        theGamePanel.playerLvlDisplay.text = "lv" + playerLvl;
        OnPlayerHPChange(20);
    }
}
