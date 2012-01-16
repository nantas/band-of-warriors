using UnityEngine;
using System.Collections;

public class HealthPack : Item {
	
	public exSprite spItem;
	public Collider spCollider;
    //health amount to recover.
    public int healthAmount = 30;
    public float itemGoUpHeight = 100.0f;
    public float itemMoveUpTime = 0.2f;
    //the height it will stay on the ground.
    public float itemStayHeight = -100.0f;
    //how high up the item will jump up after spawned.
    public float itemJumpHeight = 100.0f; 
    //healing particle effect.
    public ParticleEmitter fxHeal;

    private Spawner spawner;

	void OnEnable () {
		if (spItem) spItem.enabled = true;
		if (spCollider) spCollider.enabled = false;
	}
	
	void OnDisable () {
		StopAllCoroutines();
		CancelInvoke();
        iTween.Stop(gameObject);
		if (spItem) spItem.enabled = false;
		if (spCollider) spCollider.enabled = false;
		
	}
	
	// Use this for initialization
	void Start () {
		gameObject.Init();
        spawner = Game.instance.theSpawner;
	}
    
    public void PopUp () {
        spItem.spanim.Play("healthpack_idle");
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y + itemJumpHeight, transform.position.z);
        float moveTime = itemMoveUpTime;
        gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeOutQuart, "DropDown", gameObject); 

    }

    public void DropDown () {
        if (spCollider) spCollider.enabled = true;
        Vector3 targetPos = new Vector3 (transform.position.x, itemStayHeight, transform.position.z);
        float moveTime = Mathf.Abs((itemMoveUpTime/(transform.position.y - itemStayHeight)) * itemJumpHeight);
        gameObject.MoveTo(targetPos, moveTime, 0, EaseType.easeInQuart, "CoinTimer", gameObject);
    }

	// Update is called once per frame
	void Update () {

    }
    
    public void ItemDisappearTimer () {
        Invoke("ItemBlink", 5.0f);
    }
    
    public void ItemBlink () {
        spItem.spanim.Play("healthpack_blink");
        Invoke("ItemDisappear", 2.0f);
    }

    public void ItemDisappear () {
        spItem.spanim.Stop();
        spawner.DestroyHealthPack(this);
    }

    public override void OnPickedUp () {
		spCollider.enabled = false;
        Game.instance.OnPlayerHPChange (healthAmount);
        ShowHealEffect();
    }

    public void ShowHealEffect () {
        fxHeal.Emit();
        spawner.DestroyHealthPack(this);
    }


}

