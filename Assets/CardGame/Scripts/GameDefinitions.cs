namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PofyTools;
    using PofyTools.Data;
    using System.IO;

    public class GameDefinitions : IInitializable
    {
        #region Game Constants

        public const string DEFINITIONS_PATH = "/definitions";
        public const string LOCATIONS_PATH = "/locations.json";
        public const string ENCOUNTERS_PATH = "/encounters.json";
        public const string CATEGORIES_PATH = "/categories.json";

        #endregion

        public const string TAG = "<color=red><b>GameDefinitions: </b></color>";

        public static DefinitionSet<LocationCardDefinition> Locations = new DefinitionSet<LocationCardDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.LOCATIONS_PATH);
        public static DefinitionSet<EncounterCardDefinition> Encounters = new DefinitionSet<EncounterCardDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.ENCOUNTERS_PATH);
        public static DefinitionSet<CategoryDefinition> Categories = new DefinitionSet<CategoryDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.CATEGORIES_PATH);

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
                Categories.Initialize ();
                Encounters.Initialize ();
                Categories.Initialize ();

                _instance = new GameDefinitions ();
                _instance.Initialize ();
            }
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
        public string displayName;
        [TextArea]
        public string categoryDescription;

        public List<string> baseCategories = new List<string> ();

    }

    public class CategoryData : Data, IDefinable<CategoryDefinition>
    {
        #region IDefinable
        public CategoryDefinition definition
        {
            get;
            protected set;
        }

        public bool isDefined { get {return this.definition != null; } }

        public void Define (CategoryDefinition definition)
        {
            this.definition = definition;
        }

        public void Undefine ()
        {
            this.definition = null;
        }
        #endregion

        #region Runtime Data
        


        #endregion
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