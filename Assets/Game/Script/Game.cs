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
    //base platform, usually the ground floor
    public PlatformCollider theBasePlatform;
    //player hp
    public int maxPlayerHP = 100;	
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

    private int initPlayerHP;


	
	protected virtual void Init () {
		theSpawner = GetComponent<Spawner>();
        theScoreCounter = GetComponent<ScoreCounter>();
        theLevelManager = GameObject.FindWithTag("levelManager").GetComponent<LevelManager>();
        theItemCarrier = GameObject.FindWithTag("itemCarrier").GetComponent<ItemCarrier>();
        initPlayerHP = maxPlayerHP;
	}

	// Use this for initialization
	void Start () {
        playerHP = maxPlayerHP;
        currentExp = 0;
        curCharIndex = 1;
        theGamePanel.charSelectPanel.transform.position 
            = new Vector3 ( Camera.main.transform.position.x , 0, theGamePanel.charSelectPanel.transform.position.z );
        AcceptInput(false);
        theGamePanel.charSelectPanel.enabled = true;
        // AcceptInput(true);
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
        } else if (playerHP > maxPlayerHP) {
            playerHP = maxPlayerHP;
        }
        theGamePanel.HPbar.ratio = ((float)playerHP)/((float)maxPlayerHP);
    }

    public void OnPlayerAttributeUpdate() {
        //ATTR: att_hpBoost multiplier
        float maxHPBoost = thePlayer.charBuild.GetAttributeEffectMultiplier("att_hpBoost");        
        maxPlayerHP = Mathf.FloorToInt(initPlayerHP * maxHPBoost);

        //ATTR: att_invinBoost multiplier
        float invinTimeBoost = thePlayer.charBuild.GetAttributeEffectMultiplier("att_invinBoost");
        thePlayer.playerController.FSM_Hit.FsmVariables.GetFsmFloat("varInvincibleDuration").Value
            = thePlayer.playerController.initInvincibleDuration * invinTimeBoost;

        //ATTR: att_jumpBoost multiplier
        float jumpHeightBoost = thePlayer.charBuild.GetAttributeEffectMultiplier("att_jumpBoost");
        thePlayer.playerController.jumpSpeed = thePlayer.playerController.initJumpSpeedStatic * jumpHeightBoost;

        //ATTR: att_speedBoost multiplier
        float speedBoost = thePlayer.charBuild.GetAttributeEffectMultiplier("att_speedBoost");
        thePlayer.playerController.moveSpeed = thePlayer.playerController.initMoveSpeedStatic * speedBoost;

        //ATTR: att_lootDropBoost multiplier
        float lootDropBoost = thePlayer.charBuild.GetAttributeEffectMultiplier("att_lootDropBoost");
        thePlayer.playerController.lootChanceBoostAttribute = lootDropBoost - 1.0f;

        //ATTR: att_damageBoost multiplier
        float damageBoost = thePlayer.charBuild.GetAttributeEffectMultiplier("att_damageBoost");
        thePlayer.playerController.attackPower = Mathf
            .FloorToInt(thePlayer.playerController.initAttackPowerStatic * damageBoost);
    }

    public void OnPlayerExpChange(int _amount) {
        CharacterBuild charBuild = thePlayer.charBuild;
        //ATTR: att_expBoost multiplier
        float expBoostMultiplier = charBuild.GetAttributeEffectMultiplier("att_expBoost");        
        int expAfterBoost = Mathf.FloorToInt(_amount * expBoostMultiplier);
        charBuild.OnPlayerExpChange(expAfterBoost);

        //ATTR: att_leech multiplier
        float leechChanceMultiplier = charBuild.GetAttributeEffectMultiplier("att_leech") - 1.0f;
        if (Random.Range(0.0f, 1.0f) < leechChanceMultiplier) {
            OnPlayerHPChange(Mathf.FloorToInt(maxPlayerHP/20));
        }

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
