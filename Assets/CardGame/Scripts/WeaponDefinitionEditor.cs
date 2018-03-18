namespace Guvernal.CardGame
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class WeaponDefinitionEditor : MonoBehaviour
    {
        public List<WeaponCardDefinition> _content;

        [ContextMenu ("Load")]
        public void Load ()
        {
            GameDefinitions.Init ();
            this._content = GameDefinitions.Weapons.GetContent ();
        }

        [ContextMenu ("Save")]
        public void Save ()
        {
            GameDefinitions.Init ();
            GameDefinitions.Weapons.SetContent (this._content);
            GameDefinitions.Weapons.Save ();
        }
    }

}