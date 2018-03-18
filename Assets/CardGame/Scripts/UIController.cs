using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools;
using UnityEngine.UI;
using PofyTools.UI;

namespace Guvernal.CardGame
{
    public class UIController : Panel
    {
        [Header ("Panels")]
        public DialogView dialogView;
        public NotificationView notificationView;

        [SerializeField]
        protected Text _mainText;

        #region ISubscribalbe
        public override bool Subscribe ()
        {
            if (base.Subscribe ())
            {
                Board.gameStarted += this.OnGameStart;
                Board.movedToField += this.OnMoveToField;

                //Panels
                this.dialogView.Subscribe ();
                this.notificationView.Subscribe ();

                return true;
            }
            return false;
        }

        public override bool Unsubscribe ()
        {
            if (base.Unsubscribe ())
            {
                Board.gameStarted -= this.OnGameStart;
                Board.movedToField -= this.OnMoveToField;

                //Panels
                this.dialogView.Unsubscribe ();
                this.notificationView.Unsubscribe ();

                return true;
            }
            return false;
        }

        #endregion

        #region Listeners
        void OnGameStart ()
        {
            AddState (this._keyboardInputState);
        }

        void OnMoveToField (BoardField field)
        {
            BoardField north = field.GetNorthField (),
                south = field.GetSouthField (),
                west = field.GetWestField (),
                east = field.GetEastField ();

            this._mainText.text = "You arrived at " + field.name + ".";
            this._mainText.text += "\n" + "At NORTH is " + ((north != null && north.type != BoardField.Type.Water) ? north.name : " just water.");
            this._mainText.text += "\n" + "At SOUTH is " + ((south != null && south.type != BoardField.Type.Water) ? south.name : " just water.");
            this._mainText.text += "\n" + "At EAST is " + ((east != null && east.type != BoardField.Type.Water) ? east.name : " just water.");
            this._mainText.text += "\n" + "At WEST is " + ((west != null && west.type != BoardField.Type.Water) ? west.name : " just water.");

        }
        #endregion

        #region IStateable

        private KeyboardInputState _keyboardInputState;

        public override void ConstructAvailableStates ()
        {
            this._keyboardInputState = new KeyboardInputState (this);
        }

        public class KeyboardInputState : StateObject<Panel>
        {
            public KeyboardInputState (Panel co) : base (co)
            {

            }

            public override void InitializeState ()
            {
                this.hasUpdate = true;
                this.ignoreStacking = true;
                this.isPermanent = true;

                base.InitializeState ();
            }

            public override bool UpdateState ()
            {
                bool up = Input.GetKeyDown (KeyCode.UpArrow),
                    down = Input.GetKeyDown (KeyCode.DownArrow),
                    left = Input.GetKeyDown (KeyCode.LeftArrow),
                    right = Input.GetKeyDown (KeyCode.RightArrow);

                if (up) { Board.requestMove (Direction.North); return false; }
                if (down) { Board.requestMove (Direction.South); return false; }
                if (right) { Board.requestMove (Direction.East); return false; }
                if (left) { Board.requestMove (Direction.West); return false; }

                return false;
            }
        }
        #endregion
    }
}
