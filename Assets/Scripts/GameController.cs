using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
	public static GameController Game;

	public int currencyCount {
		get{ return this.currencyCount; }
		private set {
			this.currencyCount = value;
			EventManager.Events.currencyChanged (this.currencyCount);
		}
	}



	void Awake ()
	{
		Game = this;
	}

	void SpendCurrency (int amount)
	{
		this.currencyCount -= amount;
	}
}
