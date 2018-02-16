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

        public Dictionary<int, List<Sprite>> _sprites = new Dictionary<int, List<Sprite>> ();

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
        [Header ("Island Parameters")]
        public float groundBais;

        public bool usePerlin;
        public float perlinStrength;

        public bool useCosine;
        public Image debugImage;

        public bool useTexture;
        public Texture2D mapTexture;

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

        public void AddField (int x, int y)
        {
            var field = Instantiate<BoardField> (this.fieldPrefab);

            float chanceX = 0;
            float chanceY = 0;
            float perlin = 0;
            int influencerCount = 0;

            float pixel = 0;

            if (this.useCosine)
            {
                chanceX = Mathf.Cos (((x - (this.boardSize.x) * 0.5f) / ((this.boardSize.x) * 0.5f)) * Mathf.PI);
                chanceY = Mathf.Cos (((y - (this.boardSize.y) * 0.5f) / ((this.boardSize.y) * 0.5f)) * Mathf.PI);
                influencerCount++;
            }

            if (this.usePerlin)
            {
                perlin = this.perlinStrength * (0.5f - Mathf.PerlinNoise (((float)x / this.boardSize.x) * 8, (((float)y / this.boardSize.y)) * 8));

                influencerCount++;
            }

            if (this.useTexture && this.mapTexture != null)
            {
                pixel = this.mapTexture.GetPixelBilinear ((float)x / this.boardSize.x, (float)y / this.boardSize.y).grayscale;
                Debug.Log ("Pixel: " + pixel);
                influencerCount++;
            }

            float chance = Mathf.Clamp01 (((chanceX + chanceY + perlin + pixel) / Mathf.Max (influencerCount, 1)) + this.groundBais);

            field.Initialize (this, new Vector2Int (x, y));
            field.groundChance = chance;
            field.transform.SetParent (this.transform, false);

            if (x != 0 && y != 0 && x != this.boardSize.x - 1 && y != this.boardSize.y - 1 && Chance.TryWithChance (chance))
            {
                field.image.sprite = this.sprites[0];
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

        public void SetFieldSprite (BoardField field)
        {
            if (field.image.sprite == null)
                return;
            int score = 0;
            BoardField north, west, east, south;

            north = field.GetNorthField ();
            west = field.GetWestField ();
            east = field.GetEastField ();
            south = field.GetSouthField ();

            Sprite spriteNorth = null, spriteWest = null, fieldSprite = null;

            if (north == null || north.image.sprite == null)
                score += 4;

            if (west == null || west.image.sprite == null)
                score += 16;

            if (east == null || east.image.sprite == null)
                score += 64;

            if (south == null || south.image.sprite == null)
                score += 256;

            if (north != null)
                spriteNorth = north.image.sprite;
            if (west != null)
                spriteWest = west.image.sprite;

            if (score == 0)
            {
                fieldSprite = this._sprites[score].GetRandom ();
            }
            else
            {
                do
                {
                    fieldSprite = this._sprites[score].GetRandom ();
                }
                while (fieldSprite == spriteNorth || fieldSprite == spriteWest);

                field.image.sprite = fieldSprite;
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
            DrawDebugImage ();
            ReskinBoard ();
        }
        public Texture2D debugTexture;
        public void DrawDebugImage ()
        {
            //Create NewTexture
            this.debugTexture = new Texture2D (this.boardSize.x, this.boardSize.y);



            for (int y = 0; y < this.boardSize.y; ++y)
            {
                for (int x = 0; x < this.boardSize.x; ++x)
                {
                    var field = this.GetField (x, y);
                    debugTexture.SetPixel (x, y, new Color (field.groundChance, field.groundChance, field.groundChance, 1f));
                }
            }

            //Refresh Texture for displaying
            this.debugTexture.Apply ();
            //this.debugTexture = FlipTexture (this.debugTexture);

            //Set Properties
            this.debugTexture.name = "DebugTexture";
            this.debugTexture.filterMode = FilterMode.Point;
            this.debugTexture.wrapMode = TextureWrapMode.Clamp;

            var sprite = Sprite.Create (this.debugTexture, new Rect (0, 0, this.boardSize.x, this.boardSize.y), Vector2.zero, 100);
            sprite.name = "DebugSprite";

            this.debugImage.sprite = sprite;
        }

        private Texture2D FlipTexture (Texture2D original)
        {
            Texture2D flipped = new Texture2D (original.width, original.height);

            int width = original.width;
            int height = original.height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    flipped.SetPixel (x, height - y - 1, original.GetPixel (x, y));
                }
            }
            flipped.Apply ();

            return flipped;

        }

        [ContextMenu ("Reskin Board")]
        public void ReskinBoard ()
        {
            foreach (var field in this._allFields)
            {
                SetFieldSprite (field);
            }
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