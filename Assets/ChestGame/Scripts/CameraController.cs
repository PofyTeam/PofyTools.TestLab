using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;

public class CameraController : StateableActor
{
    public static CameraController Controller;
    public Transform aimTarget;

    #region Mono


    protected override void Awake()
    {
        Controller = this;
        base.Awake();
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
    public AimState()
        : base()
    {
    }

    public AimState(CameraController co)
        : base(co)
    {
    }

    public override void InitializeState()
    {
        this.HasUpdate = true;
        base.InitializeState();
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override bool UpdateState()
    {
        //return base.UpdateState();
        return false;
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
