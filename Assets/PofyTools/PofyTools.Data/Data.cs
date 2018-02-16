namespace PofyTools.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;

    /// <summary>
    /// Collection of keyable values obtainable via key or index.
    /// </summary>
    /// <typeparam name="TKey"> Key Type.</typeparam>
    /// <typeparam name="TValue">Value Type.</typeparam>
    [System.Serializable]
    public abstract class DataSet<TKey, TValue> : IInitializable
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
        #endregion

        #region IO

        public static void LoadDefinitionSet (DefinitionSet<T> definitionSet)
        {
            string fullPath = Application.dataPath + definitionSet._path;
            if (!File.Exists (fullPath))
            {
                //SaveData();
                return;
            }

            var json = File.ReadAllText (fullPath);
            //            json = UnScramble(json);
            //            json = DecodeFrom64(json);
            JsonUtility.FromJsonOverwrite (json, definitionSet);

            //            _data.PostLoad();
        }

        public static void SaveDefinitionSet (DefinitionSet<T> definitionSet)
        {
            if (definitionSet != null)
            {
                string fullPath = Application.dataPath + definitionSet._path;

                var json = JsonUtility.ToJson (definitionSet, false);

                File.WriteAllText (fullPath, json);
            }
            else
            {
                Debug.LogError ("Saving Failed! Definitions Set is null.");
            }
        }

        #endregion
    }

    //TODO
    //    [System.Serializable]
    //    public class DataSet<T>:IInitializable where T:Data
    //    {
    //        [SerializeField]
    //        protected List<T> _data = new List<T>();
    //
    //        [System.NonSerialized]
    //        public Dictionary<string,T> data;
    //        public string path;
    //
    //        public DefinitionSet(string path)
    //        {
    //            this.path = path;
    //        }
    //
    //        #region IInitializable implementation
    //
    //        public virtual bool Initialize()
    //        {
    //            if (!this.isInitialized)
    //            {
    //                DefinitionSet<T>.LoadDefinitionSet(this);
    //                this.definitions = new Dictionary<string, T>(this._definitions.Count);
    //
    //                foreach (var def in this._definitions)
    //                {
    //                    this.definitions[def.id] = def;
    //                }
    //
    //                this.isInitialized = true;
    //                return true;
    //            }
    //            return false;
    //        }
    //
    //        public bool isInitialized
    //        {
    //            get;
    //            protected set;
    //        }
    //
    //        #endregion
    //
    //        #region API
    //
    //        public T GetDefinitionById(string id)
    //        {
    //            T result = default(T);
    //
    //            if (!this.isInitialized)
    //            {
    //                Debug.LogWarning("Definitions not initialized! " + typeof(T).ToString());
    //                return result;
    //            }
    //
    //            if (!this.definitions.TryGetValue(id, out result))
    //                Debug.LogWarning("Definition not found: " + id);
    //
    //            return result;
    //        }
    //
    //        #endregion
    //
    //        #region IO
    //
    //        public static void LoadDefinitionSet(DefinitionSet<T> definitionSet)
    //        {
    //            string fullPath = Application.dataPath + definitionSet.path;
    //            if (!File.Exists(fullPath))
    //            {
    //                //SaveData();
    //                return;
    //            }
    //
    //            var json = File.ReadAllText(fullPath);
    //            //            json = UnScramble(json);
    //            //            json = DecodeFrom64(json);
    //            JsonUtility.FromJsonOverwrite(json, definitionSet);
    //
    //            //            _data.PostLoad();
    //        }
    //
    //        #endregion
    //    }

    public abstract class Definition
    {
        public string id;
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

    public abstract class Data
    {
        public string id;
    }

    public interface IDatable<T> where T : Data
    {
        T data { get; }

        void AppendData (T data);

        void ReleaseData ();
    }

}