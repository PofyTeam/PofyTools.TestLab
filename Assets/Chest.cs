using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;

public class Chest : StateableActor, IAnimated, ICollidable
{
	#region ICollidable implementation

	protected Rigidbody _selfRigidBody;

	public Rigidbody selfRigidbody {
		get {
			return this._selfRigidBody;
		}
	}

	protected Collider _selfCollider;

	public Collider selfCollider {
		get {
			return this._selfCollider;
		}
	}

	#endregion

	#region IAnimated implementation

	protected Animator _selfAnimator;

	public Animator selfAnimator {
		get {
			return this._selfAnimator;
		}
	}

	#endregion

	#region Variables

	public Transform fxSocket;

	public ParticleSystem effectPrefab;

	public ParticleSystem _fx{ get; private set; }

	#endregion

	#region Mono

	protected override void Awake ()
	{
		this._selfAnimator = GetComponent<Animator> ();
		this._selfRigidBody = GetComponent<Rigidbody> ();

		base.Awake ();
		this._fx = Instantiate<ParticleSystem> (this.effectPrefab);
		this._fx.transform.parent = this.fxSocket;
		this._fx.transform.localPosition = Vector3.zero;

	}

	protected OpenChestState openState;

	protected bool _isOpen{ get; private set; }

	[ContextMenu ("Open Chest")]
	public void OnRayHit ()
	{
		if (!this._isOpen) {
			this._isOpen = true;
			AddState (this.openState);
		} else {
			this._isOpen = false;
			RemoveState (this.openState);

		}
	}


	#endregion

	#region implemented abstract members of StateableActor

	public override void ConstructAvailableStates ()
	{
		this.openState = new OpenChestState (this);
	}

	public override void InitializeStateStack ()
	{
		this._stateStack = new List<IState> (1);
	}

	#endregion
}

public class OpenChestState:StateObject<Chest>
{
	public OpenChestState ()
	{
		InitializeState ();
	}

	public OpenChestState (Chest co)
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
		this._controlledObject.selfRigidbody.isKinematic = true;
		this._controlledObject.selfAnimator.SetTrigger ("Open");
		this._controlledObject._fx.Play ();
	}

	public override void ExitState ()
	{
		this._controlledObject.selfRigidbody.isKinematic = false;
		this._controlledObject.selfAnimator.SetTrigger ("Close");
		this._controlledObject._fx.Stop ();
	}
}
