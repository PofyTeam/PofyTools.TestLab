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
        public bool IsInitialized { get; protected set; }

        public bool Initialize ()
        {
            if (!this.IsInitialized)
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

                Camera.gameObject.SetActive (true);
                UI.gameObject.SetActive (true);

                Camera.Initialize ();
                UI.Initialize ();

                PofyTools.UI.NotificationView.Show ("Game Manager Initialized!", null, -1f);
                this.IsInitialized = true;
                return true;
            }
            return false;
        }
        #endregion

        #region ISubscribable
        public bool IsSubscribed { get; protected set; }

        public bool Subscribe ()
        {
            if (!this.IsSubscribed)
            {
                //Do Subscribe
                GameManager.Camera.Subscribe ();
                GameManager.UI.Subscribe ();
                PofyTools.UI.NotificationView.Show ("Welcome!", null, -1f);
                //PofyTools.UI.DialogView.Show ("Woul you like to see the map?",
                //    PofyTools.UI.DialogView.Type.Cancel, delegate () { UI.boardView.Open (); });
                this.IsSubscribed = true;
                return true;
            }
            return false;
        }

        public bool Unsubscribe ()
        {
            if (this.IsSubscribed)
            {
                //Do Unsubscribe
                GameManager.Camera.Unsubscribe ();
                GameManager.UI.Unsubscribe ();
                this.IsSubscribed = false;
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