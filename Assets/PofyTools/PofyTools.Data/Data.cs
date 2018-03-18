namespace PofyTools.Data
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Collection of keyable values obtainable via key or index.
    /// </summary>
    /// <typeparam name="TKey"> Key Type.</typeparam>
    /// <typeparam name="TValue">Value Type.</typeparam>
    [System.Serializable]
    public abstract class DataSet<TKey, TValue> : IInitializable, IContentProvider<List<TValue>>
    {
        [SerializeField]
        protected List<TValue> _content = new List<TValue> ();

        public Dictionary<TKey, TValue> content = null;

        public abstract bool Initialize ();

        public virtual bool isInitialized { get; protected set; }

        /// <summary>
        /// Gets content's element via key.
        /// </summary>
        /// <param name="key">Element's key.</param>
        /// <returns>Content's element.</returns>
        public TValue GetValue (TKey key)
        {
            TValue result = default (TValue);

            if (!this.isInitialized)
            {
                Debug.LogWarning ("Data Set Not Initialized! " + typeof (TValue).ToString ());
                return result;
            }

            if (!this.content.TryGetValue (key, out result))
                Debug.LogWarning ("Value Not Found For Key: " + key);

            return result;
        }

        /// <summary>
        /// Gets random element from content.
        /// </summary>
        /// <returns>Random element</returns>
        public TValue GetRandom ()
        {
            return this._content.GetRandom ();
        }

        /// <summary>
        /// Gets random element different from the last random pick.
        /// </summary>
        /// <param name="lastRandomIndex">Index of previously randomly obtained element.</param>
        /// <returns>Random element different from last random.</returns>
        public TValue GetNextRandom (ref int lastRandomIndex)
        {
            int newIndex = lastRandomIndex;
            int length = this._content.Count;

            if (length > 1)
            {
                do
                {
                    newIndex = Random.Range (0, length);
                }
                while (lastRandomIndex == newIndex);
            }

            lastRandomIndex = newIndex;

            return this._content[newIndex];
        }

        /// <summary>
        /// Content's element count.
        /// </summary>
        public int Count
        {
            get { return this._content.Count; }
        }

        public void SetContent (List<TValue> content)
        {
            this._content = content;
        }

        public List<TValue> GetContent ()
        {
            return this._content;
        }

        public List<TKey> GetKeys ()
        {
            return new List<TKey> (this.content.Keys);
        }
    }

    /// <summary>
    /// Collection of definitions obtainable via key or index
    /// </summary>
    /// <typeparam name="T">Definition Type</typeparam>
    [System.Serializable]
    public class DefinitionSet<T> : DataSet<string, T> where T : Definition
    {
        /// <summary>
        /// Definition set file path.
        /// </summary>
        protected string _path;

        /// <summary>
        /// Definition Set via file path
        /// </summary>
        /// <param name="path">Definition set file path.</param>
        public DefinitionSet (string path)
        {
            this._path = path;
        }

        #region IInitializable implementation

        public override bool Initialize ()
        {
            if (!this.isInitialized)
            {
                Load ();
                this.isInitialized = true;
                return true;
            }
            return false;
        }

        #endregion

        #region Instance Methods
        public void Save ()
        {
            SaveDefinitionSet (this);
        }
        
        public void Load ()
        {
            //Read the list content from file
            DefinitionSet<T>.LoadDefinitionSet (this);

            //Create set's dictionary same size as list
            this.content = new Dictionary<string, T> (this._content.Count);

            //Add definitions from list to dicionary
            foreach (var def in this._content)
            {
                if (this.content.ContainsKey (def.id))
                    Debug.LogWarning ("Key " + def.id + " present in the set. Overwriting...");
                this.content[def.id] = def;
            }
        }

        public void Reload ()
        {
            DefinitionSet<T>.LoadDefinitionSet (this);
            this.content.Clear ();
            //Add definitions from list to dicionary
            foreach (var def in this._content)
            {
                if (this.content.ContainsKey (def.id))
                    Debug.LogWarning ("Key " + def.id + " present in the set. Overwriting...");
                this.content[def.id] = def;
            }
        }
        #endregion

        #region IO

        public static void LoadDefinitionSet (DefinitionSet<T> definitionSet)
        {
            string fullPath = Application.dataPath + definitionSet._path;
            DataUtility.LoadOverwrite (fullPath, definitionSet);
        }

        public static void SaveDefinitionSet (DefinitionSet<T> definitionSet)
        {
            string fullPath = Application.dataPath + definitionSet._path;
            DataUtility.Save (fullPath, definitionSet);
        }

        #endregion
    }

    public abstract class Data
    {
        public string id;
    }

    public abstract class Definition
    {
        public string id;
    }

    public class DefinableData<T> : Data, IDefinable<T> where T : Definition
    {
        public DefinableData (T definition)
        {
            Define (definition);
        }

        #region IDefinable

        public T definition
        {
            get;
            protected set;
        }

        public bool isDefined { get { return this.definition != null; } }

        public void Define (T definition)
        {
            this.definition = definition;
            this.id = this.definition.id;
        }

        public void Undefine ()
        {
            this.definition = null;
            this.id = string.Empty;
        }

        #endregion
    }

    public interface IDefinable<T> where T : Definition
    {
        T definition
        {
            get;
        }

        bool isDefined { get; }

        void Define (T definition);

        void Undefine ();
    }

    public interface IDatable<T> where T : Data
    {
        T data { get; }

        void AppendData (T data);

        void ReleaseData ();
    }
}