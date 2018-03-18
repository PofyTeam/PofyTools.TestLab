namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using PofyTools;
    using PofyTools.Data;

    public class DefinitionEditor : MonoBehaviour
    {
        public List<CategoryDefinition> categoryDefs;
        public List<EncounterCardDefinition> encountersDefs;
        public List<LocationCardDefinition> locationsDefs;

        [ContextMenu ("Load Definitions")]
        public void LoadDefinitions ()
        {
            GameDefinitions.Init ();
            this.categoryDefs = GameDefinitions.Categories.GetContent ();
            this.encountersDefs = GameDefinitions.Encounters.GetContent ();
            this.locationsDefs = GameDefinitions.Locations.GetContent ();
        }

        [ContextMenu ("Save Definitions")]
        public void SaveDefinitions ()
        {
            GameDefinitions.Locations.SetContent (this.locationsDefs);
            GameDefinitions.Locations.Save ();
            GameDefinitions.Encounters.SetContent (this.encountersDefs);
            GameDefinitions.Encounters.Save ();
            GameDefinitions.Categories.SetContent (this.categoryDefs);
            GameDefinitions.Categories.Save ();
        }

    }

}