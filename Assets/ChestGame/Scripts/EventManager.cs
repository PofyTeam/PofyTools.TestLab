using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void IntDelegate (int value);
public delegate void UpdateDelegate ();
public delegate void FloatDelegate (float value);
public delegate void BoolDelegate (bool value);
public delegate void StringDelegate (string value);
public delegate void ParamsDelegate (params object[] args);


public class EventManager : MonoBehaviour
{
	public static EventManager Events;

	public IntDelegate currencyChanged;
	// Use this for initialization

	void Awake ()
	{
		Events = this;
	}

	void Start ()
	{
		this.currencyChanged = this.IntIdle;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void IntIdle (int value)
	{
	}

	private void VoidIdle ()
	{
	}
}
