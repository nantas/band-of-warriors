using UnityEngine;
using System.Collections;

public class IAPCtrl : MonoBehaviour {

    public void BuyRevive() {
        if (Application.isEditor) {
            OnRevivePaySuccess();
        } else {
            CmBillingAndroid.Instance.DoBilling(true,/* if sms */ true, /* if forced once */ "001", "", "GameLogic", "OnBillingResult"); 
        }
    }

	public void OnBillingResult(string result)
	{ 
		Debug.Log("BillingResult="+result);
		string[] results = result.Split('|');
		if(CmBillingAndroid.BillingResult.CANCELLED==results[1])
		{
            Debug.Log("unipay failed, alias: " + results[0]);
		} else if (CmBillingAndroid.BillingResult.SUCCESS == results[1]) {
            Debug.Log("buy success, alias: " + results[0]);
            OnRevivePaySuccess();
        }
	}

    void OnRevivePaySuccess() {
        Game.instance.Revive();
    }
}
