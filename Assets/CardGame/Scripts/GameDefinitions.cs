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

        #endregion

        public const string TAG = "<color=red><b>GameDefinitions: </b></color>";

        public DefinitionSet<LocationCardDefinition> locations = new DefinitionSet<LocationCardDefinition> (DEFINITIONS_PATH + LOCATIONS_PATH);

        public DefinitionSet<EncounterCardDefinition> encounters = new DefinitionSet<EncounterCardDefinition> (DEFINITIONS_PATH + ENCOUNTERS_PATH);

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
                this.locations.Initialize ();
                this.encounters.Initialize ();

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
    public class LocationCardDefinition : Definition
    {
        public string descriptionView;
        public string descriptionLand;
        public string descriptionActivate;

        public List<string> encounters = new List<string> ();
    }

    [System.Serializable]
    public class EncounterCardDefinition : Definition
    {
        public string displayName, description;
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