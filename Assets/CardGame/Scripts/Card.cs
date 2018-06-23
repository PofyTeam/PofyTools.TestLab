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
        public bool IsOpen { get; protected set; }

        public void Close()
        {
            this._animator.SetBool("IsActive", false);
            this.IsOpen = false;
        }

        public void Open()
        {
            this._animator.SetBool("IsActive", true);
            this.IsOpen = true;
        }

        public void Toggle(bool open)
        {
            if (open)
                Open();
            else
                Close();
        }

        public void Toggle()
        {
            if (this.IsOpen)
                Close();
            else
                Open();
        }

        #endregion
        #region Mono

        void OnMouseDown()
        {
            Toggle();
        }

        #endregion

        #region Animation Events

        public void OnEvent(int code)
        {
            if (code == 0)
                PofyTools.Sound.SoundManager.PlayVariation("CardPickAndFlip");
            //else
            //    PofyTools.Sound.SoundManager.PlayVariation ("CardPick");
        }

        #endregion
    }

}