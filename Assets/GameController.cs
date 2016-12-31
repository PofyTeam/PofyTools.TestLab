using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void IntDelegate (int value);
public delegate void UpdateDelegate ();
public class GameController : MonoBehaviour
{
	public static GameController Game;

	public int currencyCount {
		get{ return this.currencyCount; }
		private set {
			this.currencyCount = value;
			this.currencyChanged (this.currencyCount);
		}
	}

	public IntDelegate currencyChanged;

	void Awake ()
	{
		Game = this;
	}

	void SpendCurrency (int amount)
	{
		this.currencyCount -= amount;
	}
}
