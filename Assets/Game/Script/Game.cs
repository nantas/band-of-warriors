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
	
    //the common actor spawner reference
	[System.NonSerialized] public Spawner theSpawner;
    [System.NonSerialized] public ScoreCounter theScoreCounter;
    [System.NonSerialized] public LevelManager theLevelManager;
    [System.NonSerialized] public ItemCarrier theItemCarrier;
    [System.NonSerialized] public int playerHP;
    [System.NonSerialized] public int currentExp;
    //current character index, from 1 to 3 at the moment.
    //1: lancer, 2: archer, 3: hammer
    [System.NonSerialized] public int curCharIndex;
    public GamePanel theGamePanel;
    //the physical boundary of the level, player cannot go further over it
	public Transform leftBoundary;
	public Transform rightBoundary;
    //the camera boundary, it mostly used for checking if an actor is out of camera view. 
    public Transform leftSpawnEntry;
    public Transform rightSpawnEntry;
    //player hp
    public int initPlayerHP = 100;	
    //reference for player class
    public PlayerBase thePlayer;
    //layers
	public exLayer enemyLayerGround;
    public exLayer enemyLayerAir;
    public exLayer coinLayer;
    public exLayer scoreLayer;
    public exLayer fxLayer;
    //key positions for align actors on the ground or fly height.
	public float groundPosY = -130;
    public float flyPosY = 100;
    public float gravity = -300.0f;
    //a exp table, to determin how much exp need for each level of character.
    public int[] expReqForLvl;


	
	protected virtual void Init () {
		theSpawner = GetComponent<Spawner>();
        theScoreCounter = GetComponent<ScoreCounter>();
        theLevelManager = GameObject.FindWithTag("levelManager").GetComponent<LevelManager>();
        theItemCarrier = GameObject.FindWithTag("itemCarrier").GetComponent<ItemCarrier>();
	}

	// Use this for initialization
	void Start () {
        playerHP = initPlayerHP;
        currentExp = 0;
        curCharIndex = 1;
        AcceptInput(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlayerHPChange(int _amount) {
        playerHP += _amount;
        if (playerHP <= 0) {
            playerHP = 0;
            //TODO: gameover
            thePlayer.playerController.FSM_Control.FsmVariables.GetFsmBool("isPlayerNoHP").Value = true;
        } else if (playerHP > initPlayerHP) {
            playerHP = initPlayerHP;
        }
        theGamePanel.HPbar.ratio = ((float)playerHP)/((float)initPlayerHP);
    }

    public void OnPlayerExpChange(int _amount) {
        CharacterBuild charBuild = thePlayer.GetComponent<CharacterBuild>();
        charBuild.OnPlayerExpChange(_amount);

    }

    public void OnExpDisplayUpdate() {
        CharacterBuild charBuild = thePlayer.GetComponent<CharacterBuild>();
        currentExp = charBuild.curExp;
        theGamePanel.EXPbar.ratio = ((float)currentExp)/((float)expReqForLvl[charBuild.charLevel-1]);
    }

    //enable/disable game panel and player control.
    public void AcceptInput ( bool _accept ) {
        Camera.main.GetComponent<CameraFollow>().enabled = _accept;
        exUIPanel panelSelf = theGamePanel.GetComponent<exUIPanel>();
        panelSelf.enabled = _accept;
    }
}
