using UnityEngine;
using System.Collections.Generic;

namespace Guvernal.CardGame
{
    using System.Collections;
    //using System.Collections.Generic;
    // using UnityEngine;
    using UnityEngine.UI;
    using PofyTools;
    using PofyTools.Distribution;

    public class Board : MonoBehaviour
    {
        #region Graphics
        public List<Sprite>
            all,
            no4,
            no16,
            no20,
            no64,
            no68,
            no80,
            no84,
            no256,
            no260,
            no272,
            no276,
            no320,
            no324,
            no336,
            no340;

        public Dictionary<int, List<Sprite>> _sprites = new Dictionary<int, List<Sprite>>();

        void Initialize ()
        {
            this._sprites[0] = this.all;
            this._sprites[4] = this.no4;
            this._sprites[16] = this.no16;
            this._sprites[20] = this.no20;
            this._sprites[64] = this.no64;
            this._sprites[68] = this.no68;
            this._sprites[80] = this.no80;
            this._sprites[84] = this.no84;
            this._sprites[256] = this.no256;
            this._sprites[260] = this.no260;
            this._sprites[272] = this.no272;
            this._sprites[276] = this.no276;
            this._sprites[320] = this.no320;
            this._sprites[324] = this.no324;
            this._sprites[336] = this.no336;
            this._sprites[340] = this.no340;
        }

        #endregion
        public float groundChance;

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

        [SerializeField]
        protected List<BoardField> _allFields = new List<BoardField> ();

        [ContextMenu ("Add Field")]
        public void AddField (int x, int y)
        {
            var field = Instantiate<BoardField> (this.fieldPrefab);
            field.Initialize (this, new Vector2Int (x, y));
            field.transform.SetParent (this.transform, false);
            if (Chance.TryWithChance(this.groundChance))
            {
                field.image.sprite = this.sprites.GetRandom ();
                field.image.color = Color.white;
            }
            else
            {
                field.image.sprite = null;
                field.image.color = Camera.main.backgroundColor;
            }

            this._fields[x, y] = field;
            this._allFields.Add (field);

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

            foreach (var field in this._allFields)
            {
                if (field.image.sprite == null)
                    continue;
                int score = 0;
                BoardField north, west, east, south;

                north = field.GetNorthField ();
                west = field.GetWestField ();
                east = field.GetEastField ();
                south = field.GetSouthField ();

                if (north == null || north.image.sprite == null)
                    score += 4;

                if (west == null || west.image.sprite == null)
                    score += 16;

                if (east == null || east.image.sprite == null)
                    score += 64;

                if (south == null || south.image.sprite == null)
                    score += 256;

                field.image.sprite = this._sprites[score].GetRandom ();

                if(score == 0)
                {
                   
                }
            }
        }

        [ContextMenu ("Clear Board")]
        public void ClearBoard ()
        {
            this.transform.ClearChildren ();
            this._allFields.Clear ();

        }

        [ContextMenu ("Rebuild Board")]
        public void RebuildBoard ()
        {
            ClearBoard ();
            Initialize ();
            PopulateBoard ();
        }

        private void Awake ()
        {
            RebuildBoard ();
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

        #region Mono
        float _timer = 0f;

        private void Update ()
        {
            this._timer += Time.deltaTime;
            if (this._timer >= 0.1f)
            {
                this._timer = 0f;
                var random = this._allFields.GetRandom ();
                random.image.sprite = this.sprites.GetRandom ();
                random.image.color = Color.white;

            }
        }

        #endregion


    }

}


//Ovde pisemo vezbice
public class Box
{
    public Vector2 dimension;

    public enum State
    {
        Open,
        Closed,
        Locked,
    }

    bool hasContent = false;
    Color color = Color.green;
    List<Content> content = new List<Content> ();

    public bool TryAddContentToBox (Content contentToAdd)
    {

        return false;
    }
}

public abstract class Content
{
    public Box containingBox;
    public Vector2 dimension;
}

public class Notebook : Content
{



    public bool IsInsideBox ()
    {
        if (this.containingBox == null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }


}

public class Pen : Content
{

}

public class Headphones : Content
{

}

public class Jar
{
    bool isClosed = true;
    bool hasContent;
}