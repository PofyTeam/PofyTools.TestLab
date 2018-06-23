using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;
using PofyTools.Distribution;
using PofyTools.Pool;
using PofyTools.Sound;
using PofyTools.UI;

public class Chest : StateableActor, IAnimated, ICollidable
{
    #region ICollidable implementation

    protected Rigidbody _selfRigidBody;

    public Rigidbody SelfRigidbody
    {
        get
        {
            return this._selfRigidBody;
        }
    }

    protected Collider _selfCollider;

    public Collider SelfCollider
    {
        get
        {
            return this._selfCollider;
        }
    }

    #endregion

    #region IAnimated implementation

    protected Animator _selfAnimator;

    public Animator SelfAnimator
    {
        get
        {
            return this._selfAnimator;
        }
    }

    #endregion

    #region Variables

    public Transform fxSocket;

    public ParticleSystem effectPrefab;

    public ParticleSystem _fx{ get; private set; }

    //public Chance chanceToGold;
    public Transform goldTransform;

    //public Deck<int> goldValues;
    public ScreenInfo infoPrefab;
    public Pool<ScreenInfo> infoPool;
    [Header("Sounds")]
    public AudioClip chestHit;
    public AudioClip chestOpen;
    public AudioClip chestClose;
    public AudioClip coins;

    public AudioClip winMusic;

    #endregion

    #region Mono

    //    protected override void Awake()
    //    {
    //
    //
    //        base.Awake();
    //
    //
    //    }
    //
    public override bool Initialize()
    {
        if (base.Initialize())
        {
            this._selfAnimator = GetComponent<Animator>();
            this._selfRigidBody = GetComponent<Rigidbody>();

            this._fx = Instantiate<ParticleSystem>(this.effectPrefab);
            this._fx.transform.parent = this.fxSocket;
            this._fx.transform.localPosition = Vector3.zero;
            return true;
        }
        return false;
    }

    protected OpenChestState openState;

    public bool isOpen
    {
        get{ return this.openState.IsActive; }
    }

    [ContextMenu("Open Chest")]
    public void OnRayHit()
    {
        if (!this.isOpen)
        {
            AddState(this.openState);
//            SoundManager.DuckMusic(1, 0.5f);
        }
        else
        {
            RemoveState(this.openState);
//            SoundManager.DuckMusic(0.05f, 0.5f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float volume = collision.impulse.magnitude;
        //Debug.LogError (volume);
        volume = volume / 3000;
//		if (this.hitSource == null || !this.hitSource.isPlaying)
        SoundManager.Play(clip: this.chestHit, volume: volume, lowPriority: true);
    }

    #endregion

    #region implemented abstract members of StateableActor

    public override void ConstructAvailableStates()
    {
        this.openState = new OpenChestState(this);
        //this.chanceToGold = new Chance(0.9f);
        //Deck<int>.Card card1 = new Deck<int>.Card(1, 5000);
        //Deck<int>.Card card10 = new Deck<int>.Card(10, 1000);
        //Deck<int>.Card card25 = new Deck<int>.Card(25, 500);
        //Deck<int>.Card card50 = new Deck<int>.Card(50, 100);
        //Deck<int>.Card card250 = new Deck<int>.Card(250, 25);
        //Deck<int>.Card card500 = new Deck<int>.Card(500, 13);
        //Deck<int>.Card card1000 = new Deck<int>.Card(1000, 5);
        //Deck<int>.Card card7777 = new Deck<int>.Card(7777, 1);

        //this.goldValues = new Deck<int>(card1, card10, card25, card50, card250, card500, card1000, card7777);
        //this.goldValues = this.goldValues.CreateDistributionDeck();

        this.infoPool = new Pool<ScreenInfo>(this.infoPrefab, 4, true);
    }

    public override void InitializeStateStack()
    {
        this._stateStack = new List<IState>(1);
    }

    #endregion
}

public class OpenChestState:StateObject<Chest>
{
    public OpenChestState()
        : base()
    {
    }

    public OpenChestState(Chest co)
        : base(co)
    {
    }

    public override void InitializeState()
    {
        this.HasUpdate = true;
        base.InitializeState();
    }

    private AudioSource _openSource;

    public override void EnterState()
    {
        this.ControlledObject.goldTransform.gameObject.SetActive (false);
        int gold = Chance.GenerateDigits(3,50,999);
        Debug.Log (gold);
        //if (this.controlledObject.chanceToGold.RandomValue)
        if(gold>0)
        {
			
            this.ControlledObject.goldTransform.gameObject.SetActive(true);
            ScreenInfo newInfo = this.ControlledObject.infoPool.Obtain();

            //newInfo.message.text = this.controlledObject.goldValues.PickNextCard().instance.ToString();
            newInfo.message.text = gold.ToString ();
            newInfo.target = this.ControlledObject.SelfTransform;
            newInfo.ResetFromPool();
            this._openSource = SoundManager.PlayVariation(this.ControlledObject.coins);
            SoundManager.PlayCustomMusic(this.ControlledObject.winMusic);
            this.ControlledObject._fx.Play();
        }
        else
        {
            ScreenInfo newInfo = this.ControlledObject.infoPool.Obtain();

            newInfo.message.text = "KITA!";
            newInfo.target = this.ControlledObject.SelfTransform;
            newInfo.ResetFromPool();
        }

        this.ControlledObject.SelfRigidbody.isKinematic = true;
        this.ControlledObject.SelfAnimator.SetTrigger("Open");
        this._openSource = SoundManager.PlayVariation(this.ControlledObject.chestOpen);
        base.EnterState();
    }

    public override void ExitState()
    {
//		this._controlledObject.selfRigidbody.isKinematic = false;
        this.ControlledObject.SelfAnimator.SetTrigger("Close");
        this._openSource = SoundManager.PlayVariation(this.ControlledObject.chestClose);
        SoundManager.PlayCustomMusic(SoundManager.Sounds.music);
        this.ControlledObject._fx.Stop();
        base.ExitState();
    }


}
