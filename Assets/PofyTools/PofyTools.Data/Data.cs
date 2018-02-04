namespace PofyTools.Data
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;

    [System.Serializable]
    public abstract class DataSet<TKey,TValue>:IInitializable
    {
        [SerializeField]
        protected List<TValue> _content = new List<TValue>();

        public Dictionary<TKey,TValue> content = null;

        public abstract bool Initialize();

        public virtual bool isInitialized{ get; protected set; }

        public TValue GetValue(TKey key)
        {
            TValue result = default(TValue);

            if (!this.isInitialized)
            {
                Debug.LogWarning("Data Set Not Initialized! " + typeof(TValue).ToString());
                return result;
            }

            if (!this.content.TryGetValue(key, out result))
                Debug.LogWarning("Value Not Found For Key: " + key);

            return result;
        }

        public TValue GetRandom()
        {
            return this._content.GetRandom();
        }

        public TValue GetNextRandom(ref int lastRandomIndex)
        {
            int newIndex = lastRandomIndex;
            int length = this._content.Count;

            if (length > 1)
            {
                while (lastRandomIndex == newIndex)
                {
                    newIndex = Random.Range(0, length);
                }
            }

            lastRandomIndex = newIndex;

            return this._content[newIndex];
        }

        public int Count
        {
            get{ return this._content.Count; }
        }
    }

    [System.Serializable]
    public class DefinitionSet<T>:DataSet<string,T> where T:Definition
    {
        protected string _path;

        public DefinitionSet(string path)
        {
            this._path = path;
        }

        #region IInitializable implementation

        public override bool Initialize()
        {
            if (!this.isInitialized)
            {
                DefinitionSet<T>.LoadDefinitionSet(this);
                this.content = new Dictionary<string, T>(this._content.Count);

                foreach (var def in this._content)
                {
                    if (this.content.ContainsKey(def.id))
                        Debug.LogWarning("Key " + def.id + " present in the set. Overwriting...");
                    this.content[def.id] = def;
                }

                this.isInitialized = true;
                return true;
            }
            return false;
        }

        #endregion

        #region IO

        public static void LoadDefinitionSet(DefinitionSet<T> definitionSet)
        {
            string fullPath = Application.dataPath + definitionSet._path; 
            if (!File.Exists(fullPath))
            {
                //SaveData();
                return;
            }

            var json = File.ReadAllText(fullPath);
            //            json = UnScramble(json);
            //            json = DecodeFrom64(json);
            JsonUtility.FromJsonOverwrite(json, definitionSet);

            //            _data.PostLoad();
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

    public interface IDefinable<T> where T:Definition
    {
        T definition
        {
            get;
        }

        bool isDefined{ get; }

        void Define(T definition);

        void Undefine();
    }

    public abstract class Data
    {

        public string id;
    }

    public interface IDatable<T>where T:Data
    {
        T data{ get; }

        void AppendData(T data);
    }

}