﻿namespace Guvernal.CardGame
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
        public const string WEAPONS_PATH = "/weapons.json";

        public const string SEMANTICS_PATH = "/semantic_data.json";

        #endregion

        public const string TAG = "<color=red><b>GameDefinitions: </b></color>";

        public static DefinitionSet<LocationCardDefinition> Locations = new DefinitionSet<LocationCardDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.LOCATIONS_PATH);
        public static DefinitionSet<EncounterCardDefinition> Encounters = new DefinitionSet<EncounterCardDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.ENCOUNTERS_PATH);
        public static DefinitionSet<CategoryDefinition> Categories = new DefinitionSet<CategoryDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.CATEGORIES_PATH);
        public static DefinitionSet<WeaponCardDefinition> Weapons = new DefinitionSet<WeaponCardDefinition> (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.WEAPONS_PATH);

        public static SemanticData Semantics = new SemanticData (GameDefinitions.DEFINITIONS_PATH + GameDefinitions.SEMANTICS_PATH);

        public static CategoryDataSet CategoryData = null;
        public static InventoryData Inventory = null;
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
                Weapons.Initialize ();

                Semantics.Initialize ();

                CategoryData = new CategoryDataSet (Categories);
                Inventory = new InventoryData ();

                _instance = new GameDefinitions ();
                _instance.Initialize ();
            }
        }

        public static void ReloadData ()
        {

            Locations.Reload ();
            Encounters.Reload ();
            Categories.Reload ();
            Weapons.Reload ();
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
    public class PlayerDefiniton : Definition
    {
        public Range healthRange, staminaRange, focusRange;

    }

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

    [System.Serializable]
    public class LocationCardDefinition : CardDefinition
    {
        [TextArea]
        public string escriptionPeek;
        [TextArea]
        public string descriptionActivate;

        public List<string> encounters = new List<string> ();
    }

    public enum CardRarity
    {
        None = 0,
        Common = 1000,
        Rare = 100,
        Epic = 10,
        Legendary = 1,
    }

    [System.Serializable]
    public class EncounterCardDefinition : CardDefinition
    {
        public StatsModifier attackStats, defenceStats;
    }

    public class CardDefinition : Definition
    {
        public string displayName;

        [TextArea]
        public string description;
        public CardRarity rarity;
    }

    public class CategoryCardDefiniton : CardDefinition
    {
        public string categoryId;
    }

    [System.Serializable]
    public class ProficiencyDefinition : Definition
    {
        public ProficiencyDefinition (string id)
        {
            this.id = id;
        }

        public List<ProficiencyLevel> levels = new List<ProficiencyLevel> ();

        [System.Serializable]
        public class ProficiencyLevel
        {
            public enum Perk
            {
                None = 0,
                Consume = 1,
                Assassinate = 2,
                Command = 3,
            }

            public int requiredPoints;

            public StatsModifier attackModifier;
            public StatsModifier defenceModifier;
        }
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

    [System.Serializable]
    public class WeaponCardDefinition : CardDefinition
    {
        public List<WeaponCharge> weaponCharges;
    }

    [System.Serializable]
    public class ResourceCost
    {
        public ResourceType resource = ResourceType.None;
        public float cost = 0f;
    }

    [System.Serializable]
    public class WeaponCharge
    {
        public int proficiencyRequirement = 0;
        public List<ResourceCost> costs = new List<ResourceCost> ();

        //TODO: Extend on damage
        public int damage = 0;
        public Target target = Target.Single;
    }

    #endregion

    #region Data


    public class CategoryData : DefinableData<CategoryDefinition>
    {
        public CategoryData (CategoryDefinition definition) : base (definition) { }

        #region API
        public void AddSubcategory (CategoryData data)
        {
            this.subcategories.Add (data.id);
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
                    foreach (var baseCategory in data.Definition.baseCategories)
                    {
                        CategoryData baseData;
                        if (this.content.TryGetValue (baseCategory, out baseData))
                        {
                            baseData.AddSubcategory (data);
                        }
                    }
                }
                PofyTools.UI.NotificationView.Show ("Game Definitions Initialized!", null, -1f);
                this.isInitialized = true;
                return true;
            }
            return false;
        }
    }

    public class InventoryCardData : DefinableData<CardDefinition>
    {
        public InventoryCardData (CardDefinition definiton) : base (definiton)
        {
        }
    }

    public class WeaponCardData : InventoryCardData
    {
        public WeaponCardData (CardDefinition definiton) : base (definiton)
        {
        }
    }

    public class InventoryData
    {
        public List<WeaponCardData> weapons = new List<WeaponCardData> ();

        public void AddWeapon (WeaponCardData weapon)
        {
            if (!weapons.Contains (weapon))
            {
                this.weapons.Add (weapon);
                PofyTools.UI.NotificationView.Show ("Weapon added to inventory: " + weapon.Definition.displayName, null, -1f);
            }
        }
    }

    #endregion

    public enum ResourceType
    {
        None = 0,
        Health = 1,
        Stamina = 2,
        Focus = 3,
    }

    public enum Target
    {
        RandomSome = -2,
        RandomSingle = -1,
        None = 0,
        Single = 1,
        All = 2,
    }
}