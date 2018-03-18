namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using PofyTools;

    public class Card : StateableActor, IToggleable
    {

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Transform _visual;

        #region IToggleable
        public bool isOpen { get; protected set; }

        public void Close ()
        {
            this._animator.SetBool ("IsActive", false);
            this.isOpen = false;
        }

        public void Open ()
        {
            this._animator.SetBool ("IsActive", true);
            this.isOpen = true;
        }

        public void Toggle ()
        {
            if (this.isOpen)
                Close ();
            else
                Open ();
        }

        #endregion
        #region Mono

        void OnMouseDown ()
        {
            Toggle ();
        }

        #endregion
    }

}