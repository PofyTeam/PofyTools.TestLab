using System.Text;
using System.Globalization;

namespace PofyTools.NameGenerator
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;

    [System.Serializable]
    public class NameData:IInitializable
    {
        public const string TAG = "<color=green><b>NameData: </b></color>";

        #region Serializable Data

        public string dataVersion = "0.0";
        public List<NameSet> setNames = new List<NameSet>();
        public List<TitleSet> setTitles = new List<TitleSet>();

        public List<string> subjectivePros = new List<string>();
        public List<string> subjectiveCons = new List<string>();

        #endregion

        #region API

        [System.NonSerialized]
        protected Dictionary<string,NameSet> _setNames = new Dictionary<string, NameSet>();
        [System.NonSerialized]
        protected List<string> _setNameIds = new List<string>();

        [System.NonSerialized]
        protected Dictionary<string,TitleSet> _setTitles = new Dictionary<string, TitleSet>();
        [System.NonSerialized]
        protected List<string> _setTitleIds = new List<string>();

        protected void CreateRuntimeCollections()
        {
            this._setNames.Clear();
            this._setNameIds.Clear();

            this._setTitles.Clear();
            this._setTitleIds.Clear();

            foreach (var nameSet in this.setNames)
            {
                if (this._setNames.ContainsKey(nameSet.id))
                    Debug.LogWarning(TAG + "Id " + nameSet.id + " already present Name Sets. Owerwritting...");
                
                this._setNames[nameSet.id] = nameSet;
                this._setNameIds.Add(nameSet.id);
            }

            foreach (var titleSet in this.setTitles)
            {
                if (this._setTitles.ContainsKey(titleSet.id))
                    Debug.LogWarning(TAG + "Id " + titleSet.id + " already present in Title Sets. Owerwritting...");
                
                this._setTitles[titleSet.id] = titleSet;
                this._setTitleIds.Add(titleSet.id);
            }

        }

        public string GenerateName(string nameSetId = "", string titleSetId = "", bool useAdjective = true, bool useSubjective = true, bool useGenetive = true, bool male = true)
        {

            NameSet nameSet = null;
            TitleSet titleSet = null;
            string final = string.Empty;
            CultureInfo cultureInfo = new CultureInfo("en-US", false);
            TextInfo textInfo = cultureInfo.TextInfo;

            if (string.IsNullOrEmpty(nameSetId))
            {
                nameSetId = this._setNameIds[Random.Range(0, this._setNameIds.Count - 1)];
            }

            if (this._setNames.TryGetValue(nameSetId, out nameSet))
            {
                final = textInfo.ToTitleCase(nameSet.start[Random.Range(0, nameSet.start.Count - 1)].ToLower(cultureInfo));
                final += nameSet.end[Random.Range(0, nameSet.end.Count - 1)];
            }

            if (this._setTitles.TryGetValue(titleSetId, out titleSet))
            {
                if (useAdjective || useSubjective)
                {
                    final += " the ";

                    if (useAdjective)
                    {
                        final += textInfo.ToTitleCase(titleSet.adjectives[Random.Range(0, titleSet.adjectives.Count - 1)].ToLower(cultureInfo));
                        final += " ";
                    }

                    if (useSubjective)
                    {
                        TitleSet opposingSet = null;

                        bool opposing = Random.Range(0f, 1f) > 0.5f && this._setTitles.TryGetValue(titleSet.opposingId, out opposingSet);
                        if (opposing)
                        {
                            final += textInfo.ToTitleCase(opposingSet.objectives[Random.Range(0, opposingSet.objectives.Count - 1)].ToLower(cultureInfo));
                            final += textInfo.ToTitleCase(this.subjectiveCons[Random.Range(0, this.subjectiveCons.Count - 1)].ToLower(cultureInfo));
                        }
                        else
                        {
                            final += textInfo.ToTitleCase(titleSet.objectives[Random.Range(0, titleSet.objectives.Count - 1)].ToLower(cultureInfo));
                            final += textInfo.ToTitleCase(this.subjectivePros[Random.Range(0, this.subjectivePros.Count - 1)].ToLower(cultureInfo));
                        }
                    }
                }

                if (useGenetive)
                    final += " of " + textInfo.ToTitleCase(titleSet.genetives[Random.Range(0, titleSet.genetives.Count - 1)].ToLower(cultureInfo));
            }


            //final = textInfo.ToTitleCase(final);

            return final;
        }


        #endregion

        #region Initialize

        public bool Initialize()
        {
            if (!this.isInitialized)
            {
                NameData.LoadData(this);
                CreateRuntimeCollections();
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

        #region File IO

        public static void LoadData(NameData data)
        {
            var json = File.ReadAllText(Application.persistentDataPath + "/name_data.json");
//            json = UnScramble(json);
//            json = DecodeFrom64(json);
            JsonUtility.FromJsonOverwrite(json, data);
            data.PostLoad();
        }

        public void PostLoad()
        {
            foreach (var nameset in this.setNames)
            {
                for (int i = 0; i < nameset.start.Count; i++)
                {
                    nameset.start[i] = nameset.start[i].ToLower();
                }

                nameset.start.Sort();


                for (int i = 0; i < nameset.end.Count; i++)
                {
                    nameset.end[i] = nameset.end[i].ToLower();
                }

                nameset.end.Sort();
            }

            foreach (var titleset in this.setTitles)
            {
                for (int i = 0; i < titleset.adjectives.Count; i++)
                {
                    titleset.adjectives[i] = titleset.adjectives[i].ToLower();
                }

                titleset.adjectives.Sort();

                for (int i = 0; i < titleset.genetives.Count; i++)
                {
                    titleset.genetives[i] = titleset.genetives[i].ToLower();
                }

                titleset.genetives.Sort();

                for (int i = 0; i < titleset.objectives.Count; i++)
                {
                    titleset.objectives[i] = titleset.objectives[i].ToLower();
                }

                titleset.objectives.Sort();

                for (int i = 0; i < titleset.subjectives.Count; i++)
                {
                    titleset.subjectives[i] = titleset.subjectives[i].ToLower();
                }

                titleset.subjectives.Sort();
            }

            this.subjectiveCons.Sort();
            this.subjectivePros.Sort();

        }

        public static void SaveData(NameData data)
        {
            data.PreSave();
            string json = JsonUtility.ToJson(data);
//            json = EncodeTo64(json);
//            json = Scramble(json);
            File.WriteAllText(Application.persistentDataPath + "/name_data.json", json);
        }

        public void PreSave()
        {
            this.subjectivePros.Sort();
            this.subjectiveCons.Sort();
        }

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.Unicode.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.Encoding.Unicode.GetString(encodedDataAsBytes);
            return returnValue;
        }

        static string Scramble(string toScramble)
        {
            StringBuilder toScrambleSB = new StringBuilder(toScramble);
            StringBuilder scrambleAddition = new StringBuilder(toScramble.Substring(0, toScramble.Length / 2 + 1));
            for (int i = 0, j = 0; i < toScrambleSB.Length; i = i + 2, ++j)
            {
                scrambleAddition[j] = toScrambleSB[i];
                toScrambleSB[i] = 'c';
            }

            StringBuilder finalString = new StringBuilder();
            int totalLength = toScrambleSB.Length;
            string length = totalLength.ToString();
            finalString.Append(length);
            finalString.Append("!");
            finalString.Append(toScrambleSB.ToString());
            finalString.Append(scrambleAddition.ToString());

            return finalString.ToString();
        }

        static string UnScramble(string scrambled)
        {
            int indexOfLenghtMarker = scrambled.IndexOf("!");
            string strLength = scrambled.Substring(0, indexOfLenghtMarker);
            int lengthOfRealData = int.Parse(strLength);
            StringBuilder toUnscramble = new StringBuilder(scrambled.Substring(indexOfLenghtMarker + 1, lengthOfRealData));
            string substitution = scrambled.Substring(indexOfLenghtMarker + 1 + lengthOfRealData);
            for (int i = 0, j = 0; i < toUnscramble.Length; i = i + 2, ++j)
                toUnscramble[i] = substitution[j];

            return toUnscramble.ToString();
        }

        #endregion
    }


    [System.Serializable]
    public class NameSet
    {
        public string id;
        public List<string> start = new List<string>();
        public List<string> end = new List<string>();

        public List<GrammarRule> concatenateRules = new List<GrammarRule>();
        public List<GrammarRule> genderConversionRules = new List<GrammarRule>();

        public List<string> subjectives = new List<string>();
    }

    [System.Serializable]
    public class GrammarRule
    {
        public enum Type:int
        {
            RemoveLeft,
            RemoveRight,
            Replace,
            Append,
        }

        public string left;
        public string right;
        public string addition;
        public Type rule;
    }

    [System.Serializable]
    public class TitleSet
    {
        public string id;
        public string opposingId;

        public List<string> adjectives = new List<string>();
        public List<string> objectives = new List<string>();
        public List<string> genetives = new List<string>();
        public List<string> subjectives = new List<string>();
    }
}