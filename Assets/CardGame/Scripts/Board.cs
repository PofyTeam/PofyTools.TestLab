namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using PofyTools;
    using PofyTools.Distribution;

    public class Board : MonoBehaviour
    {
        #region Graphics
        public Sprite sprite1,
            sprite3,
            sprite5,
            sprite9,
            sprite17,
            sprite33,
            sprtie65,
            sprite129,
            sprite257,
            sprite7,
            sprite13;
        #endregion

        public Vector2Int boardSize = Vector2Int.one;
        protected BoardField[,] _fields;
        [SerializeField]
        protected RectTransform _rectTransform;

        [SerializeField]
        protected GridLayoutGroup _grid;

        [SerializeField]
        protected BoardField fieldPrefab;

        [SerializeField]
        Sprite[] sprites;

        [ContextMenu ("Add Field")]
        public void AddField (int x, int y)
        {
            var field = Instantiate<BoardField> (this.fieldPrefab);
            field.Initialize (this, new Vector2Int (x, y));
            field.transform.SetParent (this.transform, false);
            if (Chance.FiftyFifty)
            {
                field.image.sprite = this.sprites.GetRandom ();
            }
            else
            {
                field.image.sprite = null;
                field.image.color = Color.black;
            }

            this._fields[x, y] = field;
            field.name = "x: " + x + " - y: " + y;
        }

        [ContextMenu ("PopulateBoard")]
        public void PopulateBoard ()
        {
            this._grid.cellSize = new Vector2Int ((int)this._rectTransform.sizeDelta.x / Mathf.Max (this.boardSize.x, 1), (int)this._rectTransform.sizeDelta.y / Mathf.Max (this.boardSize.y, 1));
            this._fields = new BoardField[this.boardSize.x, this.boardSize.y];

            for (int i = 0; i < this.boardSize.y; ++i)
            {
                for (int j = 0; j < this.boardSize.x; ++j)
                {
                    AddField (j, i);
                }
            }
        }

        [ContextMenu ("Clear Board")]
        public void ClearBoard ()
        {
            this.transform.ClearChildren ();
        }

        [ContextMenu ("Rebuild Board")]
        public void RebuildBoard ()
        {
            ClearBoard ();
            PopulateBoard ();
        }

        /// <summary>
        /// Gets Field at provided coodrinates
        /// </summary>
        /// <param name="x"> Horizontal Coordinate</param>
        /// <param name="y">Vertical Coordinate</param>
        /// <returns>Board Field</returns>
        public BoardField GetField (int x, int y)
        {
            if (x > this._fields.GetLength (0) - 1 || x < 0 || y > this._fields.GetLength (1) - 1 || y < 0)
            {
                return null;
            }
            return this._fields[x, y];
        }
    }

}