using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;

public class CameraController : StateableActor
{
	public static CameraController Controller;
	public Transform aimTarget;

	#region Mono

//	public override void Subscribe ()
//	{
//		base.Subscribe ();
//	}
//
//	public override void Unsubscribe ()
//	{
//		base.Unsubscribe ();
//	}

	protected override void Awake ()
	{
		Controller = this;
		base.Awake ();
	}

	#endregion

	#region implemented abstract members of StateableActor

//	public override void ConstructAvailableStates ()
//	{
////		throw new System.NotImplementedException ();
//	}
//
//	public override void InitializeStateStack ()
//	{
////		throw new System.NotImplementedException ();
//	}

	#endregion
}

public class AimState: StateObject<CameraController>
{
	public AimState ()
	{
		InitializeState ();
	}

	public AimState (CameraController co)
	{
		this._controlledObject = co;
		InitializeState ();
	}

	public override void InitializeState ()
	{
		this._hasUpdate = true;
	}

	public override void EnterState ()
	{
		base.EnterState ();
	}

	public override bool UpdateState ()
	{
		return base.UpdateState ();
	}

	public override void ExitState ()
	{
		base.ExitState ();
	}
}
