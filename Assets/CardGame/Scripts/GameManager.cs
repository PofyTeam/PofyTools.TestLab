namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PofyTools;
    using PofyTools.Sound;

    /// <summary>
    /// Master Runtime Game Class
    /// </summary>
    public class GameManager : MonoBehaviour, IInitializable, ISubscribable
    {
        public GameCamera gameCameraPrefab;
        public UIController uiControllerPrefab;

        public static GameCamera Camera { get; protected set; }
        public static UIController UI { get; protected set; }
        #region IInitalizable
        public bool isInitialized { get; protected set; }

        public bool Initialize ()
        {
            if (!this.isInitialized)
            {
                //Do Initialize
                GameDefinitions.Init ();
                //HACK
                var defaultWeaponDefinition = GameDefinitions.Weapons.GetValue ("sword");

                WeaponCardData defaultWeaponData = new WeaponCardData (defaultWeaponDefinition);

                GameDefinitions.Inventory.AddWeapon (defaultWeaponData);

                SoundManager.PlayMusic();
                this.isInitialized = true;
                return true;
            }
            return false;
        }
        #endregion

        #region ISubscribable
        public bool isSubscribed { get; protected set; }

        public bool Subscribe ()
        {
            if (!this.isSubscribed)
            {
                //Do Subscribe
                this.isSubscribed = true;
                return true;
            }
            return false;
        }

        public bool Unsubscribe ()
        {
            if (this.isSubscribed)
            {
                //Do Unsubscribe
                this.isSubscribed = false;
                return true;
            }
            return false;
        }
        #endregion

        #region Mono
        void Awake () { Initialize (); }
        void Start () { Subscribe (); }
        void OnDestroy () { Unsubscribe (); }
        #endregion

    }

}