namespace Guvernal.CardGame
{
    using PofyTools;
    using PofyTools.Data;
    using PofyTools.NameGenerator;

    using System.Collections.Generic;
    using UnityEngine;

    public class GameDefinitions : IInitializable
    {
        #region Game Constants

        public const string DEFINITIONS_PATH = "/definitions";

        public const string LOCATIONS_PATH = "/locations.json";
        public const string ENCOUNTERS_PATH = "/encounters.json";
        public const string CATEGORIES_PATH = "/categories.json";
        public const string SEMANTICS_PATH = "/semantic_data.json";
        #endregion

        public const string TAG = "<color=red><b>GameDefinitions: </b></color>";

        public static DefinitionSet<LocationCardDefinition> Locations = new DefinitionSet<LocationCardDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.LOCATIONS_PATH);
        public static DefinitionSet<EncounterCardDefinition> Encounters = new DefinitionSet<EncounterCardDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.ENCOUNTERS_PATH);
        public static DefinitionSet<CategoryDefinition> Categories = new DefinitionSet<CategoryDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.CATEGORIES_PATH);
        public static SemanticData Semantics = new SemanticData (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.SEMANTICS_PATH);

        public static CategoryDataSet CategoryData = null;

        #region Singleton

        private static GameDefinitions _instance;

        public static GameDefinitions All
        {
            get
            {
                return GameDefinitions._instance;
            }
        }

        public static void Init ()
        {
            if (_instance == null)
            {
                Locations.Initialize ();
                Encounters.Initialize ();
                Categories.Initialize ();
                Semantics.Initialize ();

                CategoryData = new CategoryDataSet (Categories);

                _instance = new GameDefinitions ();
                _instance.Initialize ();
            }
        }

        public static void ReloadData ()
        {

            Locations.Reload ();
            Encounters.Reload ();
            Categories.Reload ();

            //TODO: Reload Semantics?
            //Semantics.Load();

            CategoryData = new CategoryDataSet (Categories);
        }

        #endregion

        #region IInitializable implementation

        public virtual bool Initialize ()
        {
            if (!this.isInitialized)
            {

                Debug.Log (TAG + "Initialized!");
                this.isInitialized = true;
                return true;
            }
            return false;
        }

        public bool isInitialized
        {
            get;
            protected set;
        }

        #endregion
    }

    #region Definitions
    [System.Serializable]
    public class CategoryDefinition : Definition
    {
        public CategoryDefinition (string key)
        {
            this.id = key;
        }

        [Header ("Display Name")]
        public string displayName;
        [TextArea]
        [Header ("Category Description")]
        public string categoryDescription;
        [Header ("Base Categories")]
        public List<string> baseCategories = new List<string> ();
        [Header ("NameSet")]
        public NameSet nameSet;
        public NameSet influenceSet;
    }

    public class CategoryData : Data, IDefinable<CategoryDefinition>
    {
        public CategoryData (CategoryDefinition definition)
        {
            this.Define (definition);
        }

        #region API
        public void AddSubcategory (CategoryData data)
        {
            this.subcategories.Add (data.id);
        }

        #endregion

        #region IDefinable
        public CategoryDefinition definition
        {
            get;
            protected set;
        }

        public bool isDefined { get { return this.definition != null; } }

        public void Define (CategoryDefinition definition)
        {
            this.definition = definition;
            this.id = definition.id;
        }

        public void Undefine ()
        {
            this.definition = null;
        }
        #endregion

        #region Runtime Data
        public List<string> subcategories = new List<string> ();

        #endregion
    }

    public class CategoryDataSet : DataSet<string, CategoryData>
    {
        public CategoryDataSet (DefinitionSet<CategoryDefinition> categoryDefinitionSet)
        {
            Initialize (categoryDefinitionSet.GetContent ());
        }

        /// <summary>
        /// Topmost categories.
        /// </summary>
        public List<CategoryData> rootCategories = new List<CategoryData> ();

        public bool Initialize (List<CategoryDefinition> categoryDefs)
        {
            if (!this.isInitialized)
            {
                this.content = new Dictionary<string, CategoryData> (categoryDefs.Count);

                foreach (var category in categoryDefs)
                {
                    CategoryData data = new CategoryData (category);

                    //list
                    this._content.Add (data);

                    //dictionary
                    this.content[data.id] = data;

                    if (category.baseCategories.Count == 0)
                    {
                        this.rootCategories.Add (data);
                    }
                }

                Initialize ();

                return true;
            }
            return false;
        }

        public override bool Initialize ()
        {
            if (!this.isInitialized)
            {

                foreach (var data in this._content)
                {
                    foreach (var baseCategory in data.definition.baseCategories)
                    {
                        CategoryData baseData;
                        if (this.content.TryGetValue (baseCategory, out baseData))
                        {
                            baseData.AddSubcategory (data);
                        }
                    }
                }

                this.isInitialized = true;
                return true;
            }
            return false;
        }
    }

    [System.Serializable]
    public class LocationCardDefinition : Definition
    {
        [TextArea]
        public string descriptionView;
        [TextArea]
        public string descriptionLand;
        [TextArea]
        public string descriptionActivate;

        public List<string> encounters = new List<string> ();
    }

    [System.Serializable]
    public class EncounterCardDefinition : Definition
    {
        public string displayName;

        [TextArea]
        public string description;

        public List<string> categories;

        public StatsModifier attackStats, defenceStats;
    }

    [System.Serializable]
    public class StatsModifier
    {
        public float alphaModifier = 0f;

        //Elements
        public float fireModifier = 0f;
        public float frostModifier = 0f;
        public float shockModifier = 0f;

        public float poisonModifier = 0f;
        public float bleedModifier = 0f;

        public float sessionVariation = 0f;
    }

    #endregion

}