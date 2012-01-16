using UnityEngine;
using System.Collections;

public class Coin : Item {
	
	public exSprite spCoin;
	public Collider spCollider;
    public int score = 1;
    //how much the score pop up when coin is picked up. 
    public float scoreGoUpHeight = 100.0f;
    //coin move time 
    public float coinMoveUpTime = 0.2f;
    //score move time 
    public float scoreMoveUpTime = 0.1f;
    public enum PickUpState {
        Disabled,
        Available,
        PopUp,
        Stop
    }

    //the ground height for coin when it drops down and stop moving
    public float coinStayHeight = -100.0f;
    //the height coin go up to when it pops up
    public float coinJumpHeight = 100.0f; 
    private PickUpState coinState;

	void OnEnable () {
		if (spCoin) spCoin.enabled = true;
		if (spCollider) spCollider.enabled = false;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
        iTween.Stop(gameObject);
		if (spCoin) spCoin.enabled = false;
		if (spCollider) spCollider.enabled = false;
		
	}
	
	// Use this for initialization
	void Start () {
		gameObject.Init();
        coinState = PickUpState.Disabled;
	}
    
    public void PopUp () {
        spCoin.spanim.Play("coin_idle");
        coinState = PickUpState.PopUp;
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + coinJumpHeight, transform.position.z);
        float moveTime = coinMoveUpTime;
        gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeOutQuart, "DropDown", gameObject); 

    }

    public void DropDown () {
        coinState = PickUpState.Available;
        //only enable collider when it starts dropping
        if (spCollider) spCollider.enabled = true;
        Vector3 targetPos = new Vector3 (transform.position.x, coinStayHeight, transform.position.z);
        float moveTime = Mathf.Abs((coinMoveUpTime/(transform.position.y - coinStayHeight)) * coinJumpHeight);
        gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeInQuart, "CoinTimer", gameObject);
    }

    //5 seconds after a coin hit the ground, it will start blinking.
    public void CoinTimer () {
        Invoke("CoinBlink", 5.0f);
    }
    
    //2 seconds after a coin start blinking, it will be destroyed. 
    public void CoinBlink () {
        spCoin.spanim.Play("coin_blink");
        Invoke("CoinDisappear", 2.0f);
    }

    //destroy coin.
    public void CoinDisappear () {
        spCoin.spanim.Stop();
        Game.instance.theSpawner.DestroyCoin(this);
    }

    public override void OnPickedUp () {
        Spawner spawner = Game.instance.theSpawner;
		spCollider.enabled = false;
        spawner.DestroyCoin(this);
        //ATTR: att_goldBoost multiplier
        float goldBoostMultiplier = Game.instance.thePlayer
            .charBuild.GetAttributeEffectMultiplier("att_goldBoost");
        int goldAfterBoost = Mathf.FloorToInt(score * goldBoostMultiplier);
        ShowCoinScore (goldAfterBoost);
    }

    public void ShowCoinScore (int _score) {
        Spawner spawner = Game.instance.theSpawner;
        ScoreCounter scoreCounter = Game.instance.theScoreCounter;
        Vector2 scorePos = new Vector2(transform.position.x, transform.position.y);
        exSpriteFont score = spawner.SpawnScoreAt(scorePos) as exSpriteFont;
        score.text = "+" + _score + "g";
        //add gold to player.
        scoreCounter.OnScoreChange(_score);
        float moveTime = scoreMoveUpTime;
        Vector3 moveAmount = new Vector3 (0, scoreGoUpHeight, 0);
        score.gameObject.MoveBy(moveAmount, moveTime, 0, 
                          EaseType.easeOutBounce, "FinishShowScore", gameObject, score);
    }

    public IEnumerator FinishShowScore (exSpriteFont _score) {
        yield return new WaitForSeconds(0.1f);
        Spawner spawner = Game.instance.theSpawner;
        spawner.DestroyScore(_score);
    }

}
