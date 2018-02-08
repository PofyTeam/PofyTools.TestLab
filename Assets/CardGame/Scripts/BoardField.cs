namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PofyTools;
    using UnityEngine.UI;

    public class BoardField : MonoBehaviour, IInitializable//, ISubscribable
    {
        #region Components

        [SerializeField]
        protected Image _image;
        public Image image { get { return this._image; } }

        #endregion

        #region Board
        protected Board _board;
        public Board board { get { return this._board; } }

        [SerializeField]
        protected Vector2Int _coordinates;

        public Vector2Int coordinates
        {
            get { return this._coordinates; }
        }

        #endregion

        #region IInitializable

        [SerializeField]
        protected bool _isInitialized;
        public bool isInitialized { get { return this._isInitialized; } }

        public bool Initialize ()
        {
            if (!this._isInitialized)
            {
                //Initialization
                if (!this._image)
                    this._image = GetComponent<Image> ();

                this._isInitialized = true;
                return true;
            }
            return false;
        }

        public bool Initialize (Board board, Vector2Int coordinates)
        {
            if (this.Initialize ())
            {
                this._board = board;
                this._coordinates = coordinates;
                return true;
            }
            return false;
        }

        #endregion

        #region ISubscribable



        #endregion

        #region API

        public BoardField GetNorthField ()
        {
            return this.board.GetField (this.coordinates.x, this.coordinates.y - 1);
        }

        public BoardField GetSouthField ()
        {
            return this.board.GetField (this.coordinates.x, this.coordinates.y + 1);
        }

        public BoardField GetEastField ()
        {
            return this.board.GetField (this.coordinates.x+1, this.coordinates.y);
        }

        public BoardField GetWestField ()
        {
            return this.board.GetField (this.coordinates.x-1, this.coordinates.y);
        }


        #endregion
    }
}
