using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;
using PofyTools.Distribution;
using PofyTools.Pool;
using PofyTools.Sound;

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

	public Chance chanceToGold;
	public Transform goldTransform;

	public Deck<int> goldValues;
	public ScreenInfo infoPrefab;
	public Pool<ScreenInfo> infoPool;
	[Header ("Sounds")]
	public AudioClip chestHit;
	public AudioClip chestOpen;
	public AudioClip chestClose;
	public AudioClip coins;

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

	void OnCollisionEnter (Collision collision)
	{
		float volume = collision.impulse.magnitude;
		//Debug.LogError (volume);
		volume = volume / 3000;
//		if (this.hitSource == null || !this.hitSource.isPlaying)
		SoundManager.Play (clip: this.chestHit, volume: volume, lowPriority: true);
	}

	#endregion

	#region implemented abstract members of StateableActor

	public override void ConstructAvailableStates ()
	{
		this.openState = new OpenChestState (this);
		this.chanceToGold = new Chance (0.25f);

		Deck<int>.Card card10 = new Deck<int>.Card (10, 1000);
		Deck<int>.Card card25 = new Deck<int>.Card (25, 500);
		Deck<int>.Card card50 = new Deck<int>.Card (50, 100);
		Deck<int>.Card card250 = new Deck<int>.Card (250, 25);
		Deck<int>.Card card1000 = new Deck<int>.Card (1000, 5);
		Deck<int>.Card card7777 = new Deck<int>.Card (7777, 1);

		this.goldValues = new Deck<int> (card10, card25, card50, card250, card1000, card7777);
		this.goldValues = this.goldValues.CreateDistributionDeck ();

		this.infoPool = new Pool<ScreenInfo> (this.infoPrefab, 4, true);
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

	private AudioSource openSource;

	public override void EnterState ()
	{
		this._controlledObject.goldTransform.gameObject.SetActive (false);
		if (this._controlledObject.chanceToGold.RandomValue) {
			
			this._controlledObject.goldTransform.gameObject.SetActive (true);
			ScreenInfo newInfo = this._controlledObject.infoPool.Obtain ();

			newInfo.message.text = this._controlledObject.goldValues.PickNextCard ().instance.ToString ();
			newInfo.target = this._controlledObject.selfTransform;
			newInfo.ResetFromPool ();
			this.openSource = SoundManager.PlayVariation (this._controlledObject.coins);
			this._controlledObject._fx.Play ();
		} else {
			ScreenInfo newInfo = this._controlledObject.infoPool.Obtain ();

			newInfo.message.text = "KITA!";
			newInfo.target = this._controlledObject.selfTransform;
			newInfo.ResetFromPool ();
		}

		this._controlledObject.selfRigidbody.isKinematic = true;
		this._controlledObject.selfAnimator.SetTrigger ("Open");
		this.openSource = SoundManager.PlayVariation (this._controlledObject.chestOpen);

	}

	public override void ExitState ()
	{
		this._controlledObject.selfRigidbody.isKinematic = false;
		this._controlledObject.selfAnimator.SetTrigger ("Close");
		this.openSource = SoundManager.PlayVariation (this._controlledObject.chestClose);
		this._controlledObject._fx.Stop ();
	}


}
