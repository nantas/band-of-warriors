using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {
	
	public exSprite spCoin;
	public Collider spCollider;
    public int score = 1;
    public float scoreGoUpHeight = 100.0f;
    public float coinMoveUpTime = 0.2f;
    public float scoreMoveUpTime = 0.1f;
    public enum PickUpState {
        Disabled,
        Available,
        PopUp,
        Stop
    }

    public float coinStayHeight = -100.0f;
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
        if (spCollider) spCollider.enabled = true;
        Vector3 targetPos = new Vector3 (transform.position.x, coinStayHeight, transform.position.z);
        float moveTime = Mathf.Abs((coinMoveUpTime/(transform.position.y - coinStayHeight)) * coinJumpHeight);
        gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeInQuart, "CoinTimer", gameObject);
    }

	// Update is called once per frame
	void Update () {

    }
    
    public void CoinTimer () {
        Invoke("CoinBlink", 5.0f);
    }
    
    public void CoinBlink () {
        spCoin.spanim.Play("coin_blink");
        Invoke("CoinDisappear", 2.0f);
    }

    public void CoinDisappear () {
        //TODO: add coin blink animation
        spCoin.spanim.Stop();
        Game.instance.theSpawner.DestroyCoin(this);
    }

    public void OnPickedUp (bool _isPlayerOnRight) {
        Spawner spawner = Game.instance.theSpawner;
		spCollider.enabled = false;
        spawner.DestroyCoin(this);
        ShowCoinScore (score);
    }

    public void ShowCoinScore (int _score) {
        Spawner spawner = Game.instance.theSpawner;
        Vector2 scorePos = new Vector2(transform.position.x, transform.position.y);
        exSpriteFont score = spawner.SpawnScoreAt(scorePos) as exSpriteFont;
        score.text = "+" + _score;
        //score.gameObject.Init();
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
