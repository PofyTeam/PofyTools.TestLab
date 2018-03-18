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

                SoundManager.PlayMusic ();

                Camera = Instantiate (this.gameCameraPrefab);
                UI = Instantiate (this.uiControllerPrefab);

                Camera.Initialize ();
                UI.Initialize ();

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
                GameManager.Camera.Subscribe ();
                GameManager.UI.Subscribe ();
                PofyTools.UI.NotificationView.Show ("Welcome", null, -1f);
                PofyTools.UI.NotificationView.Show ("Game Definitions Initialized!", null, -1f);
                PofyTools.UI.NotificationView.Show ("Game Manager Initialized!", null, -1f);
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
                GameManager.Camera.Unsubscribe ();
                GameManager.UI.Unsubscribe ();
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