namespace Guvernal.CardGame
{
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using PofyTools;
    using PofyTools.Distribution;
    using PofyTools.NameGenerator;
    using Extensions;
    using PofyTools.Data;

    public class Board : Panel
    {
        #region Components

        [SerializeField]
        protected RectTransform _rectTransform;

        [SerializeField]
        protected GridLayoutGroup _grid;

        #endregion

        #region Runtimes
        [Header ("Runtimes")]
        public Image selector;
        public Image iconHome;
        protected BoardField _currentField;
        #endregion

        #region IInitializable

        public override bool Initialize ()
        {
            if (base.Initialize())
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

                return true;
            }
            return false;
        }

        #endregion

        #region ISubscribable

        public override bool Subscribe ()
        {
            if (base.Subscribe ())
            {
                Unsubscribe ();

                Board.requestGameStart += this.OnGameStartRequest;
                Board.requestMove += this.OnMoveRequest;
                return true;
            }
            return false;
        }

        public override bool Unsubscribe ()
        {
            if (base.Unsubscribe ())
            {
                Board.requestGameStart -= this.OnGameStartRequest;
                Board.requestMove -= OnMoveRequest;
                return true;
            }
            return false;
        }

        #endregion

        #region Listeners

        public void OnMoveRequest (Direction direction)
        {
            BoardField nextField = null;

            switch (direction)
            {
                case Direction.South:
                    nextField = this._currentField.GetSouthField ();
                    break;
                case Direction.West:
                    nextField = this._currentField.GetWestField ();
                    break;
                case Direction.East:
                    nextField = this._currentField.GetEastField ();
                    break;
                case Direction.North:
                    nextField = this._currentField.GetNorthField ();
                    break;

                default:
                    break;
            }

            if (nextField != null && (int)nextField.type >= 2)
                MoveToField (nextField);
        }

        public void OnGameStartRequest ()
        {
            int lastRandom = -1;
            BoardField homeField = null;
            while (homeField == null || homeField.type != BoardField.Type.Land)
            {
                homeField = this._allFields.GetNextRandom (ref lastRandom);
            }

            MoveToField (homeField);
            MakeHome (homeField);

            Board.gameStarted ();
        }

        #endregion

        //#region Data Editor
        //[Header ("Game Data")]
        //public List<CategoryDefinition> categoryDefs;
        //public List<EncounterCardDefinition> encountersDefs;
        //public List<LocationCardDefinition> locationsDefs;

        //public SemanticData semanticData;

        //[ContextMenu ("Load Definitions")]
        //public void LoadDefinitions ()
        //{
        //    GameDefinitions.Init ();

        //    this.semanticData = GameDefinitions.Semantics;

        //    this.categoryDefs = GameDefinitions.Categories.GetContent ();
        //    this.encountersDefs = GameDefinitions.Encounters.GetContent ();
        //    this.locationsDefs = GameDefinitions.Locations.GetContent ();
        //}

        //[ContextMenu ("Save Definitions")]
        //public void SaveDefinitions ()
        //{
        //    GameDefinitions.Init ();

        //    GameDefinitions.Locations.SetContent (this.locationsDefs);
        //    GameDefinitions.Locations.Save ();
        //    GameDefinitions.Encounters.SetContent (this.encountersDefs);
        //    GameDefinitions.Encounters.Save ();
        //    GameDefinitions.Categories.SetContent (this.categoryDefs);
        //    GameDefinitions.Categories.Save ();

        //    GameDefinitions.Semantics = this.semanticData;

        //    GameDefinitions.Semantics.Save (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.SEMANTICS_PATH);
        //}
        //#endregion

        #region Map Generator

        [Space]
        [Header ("Island Parameters")]
        public Color groundColor;
        public Color visitedColor;

        public float groundBais;

        public bool usePerlin;
        public float perlinStrength;

        public bool useCosine;

        public bool useTexture;
        public Texture2D mapTexture;

        public Vector2Int boardSize = Vector2Int.one;
        protected BoardField[,] _fields;
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
                //Debug.Log ("Pixel: " + pixel);
                influencerCount++;
            }

            float chance = Mathf.Clamp01 (((chanceX + chanceY + perlin + pixel) / Mathf.Max (influencerCount, 1)) + this.groundBais);

            field.Initialize (this, new Vector2Int (x, y));
            field.groundChance = chance;
            field.transform.SetParent (this._rectTransform, false);

            if (x != 0 && y != 0 && x != this.boardSize.x - 1 && y != this.boardSize.y - 1 && Chance.TryWithChance (chance))
            {
                field.type = BoardField.Type.Land;

                field.image.sprite = this.sprites[0];
                field.image.color = this.groundColor;

            }
            else
            {
                field.type = BoardField.Type.Water;
                field.image.sprite = null;
                field.image.color = new Color ();
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

        private List<Sprite> _compareSprites = new List<Sprite> ();

        public void SetFieldSprite (BoardField field)
        {
            this._compareSprites.Clear ();

            if (field.image.sprite == null)
                return;
            int score = 0;
            BoardField north, west, east, south, northEast, northWest, southEast, southWest;

            north = field.GetNorthField ();
            west = field.GetWestField ();
            east = field.GetEastField ();
            south = field.GetSouthField ();

            northEast = field.GetNorthEastField ();
            northWest = field.GetNorthWestField ();
            southEast = field.GetSouthEastField ();
            southWest = field.GetSouthWestField ();

            Sprite fieldSprite = null;

            if (north == null || north.type == BoardField.Type.Water)
                score += 4;

            if (west == null || west.type == BoardField.Type.Water)
                score += 16;

            if (east == null || east.type == BoardField.Type.Water)
                score += 64;

            if (south == null || south.type == BoardField.Type.Water)
                score += 256;

            if (southWest != null) this._compareSprites.Add (southWest.image.sprite);
            if (south != null) this._compareSprites.Add (south.image.sprite);
            if (southEast != null) this._compareSprites.Add (southEast.image.sprite);
            if (west != null) this._compareSprites.Add (south.image.sprite);

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
                while (this._compareSprites.Contains (fieldSprite));

                field.image.sprite = fieldSprite;
            }
        }


        [ContextMenu ("Clear Board")]
        public void ClearBoard ()
        {
            this._rectTransform.ClearChildren ();
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

        //private Texture2D FlipTexture (Texture2D original)
        //{
        //    Texture2D flipped = new Texture2D (original.width, original.height);

        //    int width = original.width;
        //    int height = original.height;

        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            flipped.SetPixel (x, height - y - 1, original.GetPixel (x, y));
        //        }
        //    }
        //    flipped.Apply ();

        //    return flipped;

        //}

        [ContextMenu ("Reskin Board")]
        public void ReskinBoard ()
        {
            foreach (var field in this._allFields)
            {
                SetFieldSprite (field);
            }
        }

        [ContextMenu ("Save Distribution Map")]
        public void SaveDistributionMap ()
        {
            DataUtility.IncrementSaveToPNG (Application.dataPath, "map_", this.debugTexture);
        }

        #endregion

        #region Debug
        [Space]
        public Image debugImage;

        #endregion

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


        #endregion

        #region API

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

        public void MoveToField (BoardField field)
        {
            this._currentField = field;
            this.selector.gameObject.SetActive (true);
            this.selector.rectTransform.SetParent (field.image.rectTransform, true);
            this.selector.rectTransform.localPosition = Vector3.zero;
            //this.selector.rectTransform.SetPositionAndRotation (Vector3.zero, Quaternion.identity);
            this.selector.rectTransform.sizeDelta = field.image.rectTransform.sizeDelta;
            this.selector.rectTransform.ForceUpdateRectTransforms ();
            field.image.color = this.visitedColor;
            Board.movedToField (field);

        }

        public void MakeHome (BoardField field)
        {
            if (field.type == BoardField.Type.Land)
            {
                field.type = BoardField.Type.Home;
            }
            this.iconHome.gameObject.SetActive (true);
            this.iconHome.rectTransform.SetParent (field.image.rectTransform, true);
            this.iconHome.rectTransform.localPosition = Vector3.zero;
            this.iconHome.rectTransform.sizeDelta = field.image.rectTransform.sizeDelta;
            this.iconHome.rectTransform.ForceUpdateRectTransforms ();


        }

        #endregion

        #region Events

        public static VoidDelegate requestGameStart = VoidIdle;
        public static VoidDelegate gameStarted = VoidIdle;

        public static DirectionDelegate requestMove = DirectionIdle;
        public static BoardFieldDelegate movedToField = FieldIdle;

        public static void DirectionIdle (Direction direction) { }
        public static void FieldIdle (BoardField field) { }
        public static void VoidIdle () { }
        #endregion
    }

    public delegate void VoidDelegate ();
    public delegate void DirectionDelegate (Direction direction);
    public delegate void BoardFieldDelegate (BoardField field);


    public enum Direction
    {
        South = -10,
        West = -1,
        East = 1,
        North = 10,
    }
}
