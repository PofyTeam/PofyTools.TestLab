using System.Text;
using System.Globalization;
using PofyTools.Distribution;

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

        [Header("Story Mode")]
        public List<string> subjectiveStory = new List<string>();
        [Header("Geolocation")]
        public List<string> subjectiveGeolocation = new List<string>();

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
                final = textInfo.ToTitleCase(nameSet.prefixes[Random.Range(0, nameSet.prefixes.Count - 1)].ToLower(cultureInfo));
                final += nameSet.sufixes[Random.Range(0, nameSet.sufixes.Count - 1)];
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
                            final += textInfo.ToTitleCase(opposingSet.objectivesNeutral[Random.Range(0, opposingSet.objectivesNeutral.Count - 1)].ToLower(cultureInfo));
                            final += textInfo.ToTitleCase(this.subjectiveCons[Random.Range(0, this.subjectiveCons.Count - 1)].ToLower(cultureInfo));
                        }
                        else
                        {
                            final += textInfo.ToTitleCase(titleSet.objectivesNeutral[Random.Range(0, titleSet.objectivesNeutral.Count - 1)].ToLower(cultureInfo));
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

        public string GenerateStoryName(bool useAdjective = true, bool useGenetive = true)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US", false);
            TextInfo textInfo = cultureInfo.TextInfo;
            string final = "The ";
            if (useAdjective)
            {
                final += "Creepy ";
            }
            final += textInfo.ToTitleCase(this.subjectiveStory[Random.Range(0, this.subjectiveStory.Count - 1)].ToLower(cultureInfo));
            if (useGenetive)
            {
                //TODO: pick genetive from other titlesets
                final += " of the ";
                final += "Sorrow";
            }
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
                for (int i = 0; i < nameset.prefixes.Count; i++)
                {
                    nameset.prefixes[i] = nameset.prefixes[i].ToLower();
                }

                nameset.prefixes.Sort();


                for (int i = 0; i < nameset.sufixes.Count; i++)
                {
                    nameset.sufixes[i] = nameset.sufixes[i].ToLower();
                }

                nameset.sufixes.Sort();
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
            this.subjectiveStory.Sort();

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
            this.subjectiveStory.Sort();
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

        /// <summary>
        /// The prefixes for pseudo names.
        /// </summary>
        public List<string> prefixes = new List<string>();
        /// <summary>
        /// The sufixes for pseudo names.
        /// </summary>
        public List<string> sufixes = new List<string>();

        /// <summary>
        /// The concatenation rules for generating pseudo names.
        /// </summary>
        public List<GrammarRule> concatenationRules = new List<GrammarRule>();

        /// <summary>
        /// The gender conversion rules for generating pseudo names.
        /// </summary>
        public List<GrammarRule> genderConversionRules = new List<GrammarRule>();

        /// <summary>
        /// The real male name database.
        /// </summary>
        public List<string> namesMale = new List<string>();

        /// <summary>
        /// The real female name database.
        /// </summary>
        public List<string> namesFemale = new List<string>();

        /// <summary>
        /// Gets eather a random real or pseudo name.
        /// </summary>
        /// <returns>The random real or pseudo name.</returns>
        /// <param name="male">Should random name be male or female name.</param>
        public string GetRandom(bool male = true)
        {
            if (Chance.FiftyFifty)
                return GenerateRandom(male);    
            return GetName(male);   
        }

        /// <summary>
        /// Gets the real name from name set database.
        /// </summary>
        /// <returns>A real name from the database.</returns>
        /// <param name="male">Should real name be male or female name.</param>
        public string GetName(bool male = true)
        {
            return(male) ? this.namesMale.GetRandom() : this.namesFemale.GetRandom();
        }

        /// <summary>
        /// Generates a random pseudo name by concatenating prefix and sufix and by applying grammer rules.
        /// </summary>
        /// <returns>A pseudo name.</returns>
        /// <param name="male">Should pseudo name be male or female name.</param>
        public string GenerateRandom(bool male = true)
        {
            //string result = string.Empty;

            string prefix = this.prefixes.GetRandom();
            string sufix = this.sufixes.GetRandom();

            char prefixEnd = default(char);
            char sufixStart = default(char);

            bool dirty = this.concatenationRules.Count > 0;
            while (dirty)
            {
                dirty = false;
                foreach (var rule in this.concatenationRules)
                {
                    prefixEnd = prefix[prefix.Length - 1];
                    sufixStart = sufix[0];

                    if (rule.left == prefixEnd && rule.right == sufixStart)
                    {
                        switch (rule.type)
                        {
                            case GrammarRule.Type.RemoveLeft:
                                prefix = prefix.Remove(prefix.Length - 1, 1);
                                break;
                            case GrammarRule.Type.RemoveRight:
                                sufix = sufix.Remove(0, 1);
                                break;
                            case GrammarRule.Type.ReplaceLeft:
                                prefix = prefix.Remove(prefix.Length - 1);
                                prefix = prefix.Insert(prefix.Length - 1, rule.addition);
                                break;
                            case GrammarRule.Type.ReplaceRight:
                                sufix = sufix.Remove(0);
                                sufix = sufix.Insert(0, rule.addition);
                                break;
                            case GrammarRule.Type.Insert:
                                prefix += rule.addition;
                                break;
                            case GrammarRule.Type.Append:
                                sufix += rule.addition;
                                break;
                            case GrammarRule.Type.MergeInto:
                                prefix = prefix.Remove(prefix.Length - 1);
                                sufix = sufix.Remove(0);
                                prefix += rule.addition;
                                break;
                            default:
                                break;
                        }
                        dirty = true;
                        break;
                    }
                }
            }

            dirty = !male && this.genderConversionRules.Count > 0;
            while (dirty)
            {
                dirty = false;
                foreach (var rule in this.genderConversionRules)
                {
                    prefixEnd = prefix[prefix.Length - 1];
                    sufixStart = sufix[0];

                    if (rule.left == prefixEnd && rule.right == sufixStart)
                    {
                        switch (rule.type)
                        {
                            case GrammarRule.Type.RemoveLeft:
                                prefix = prefix.Remove(prefix.Length - 1, 1);
                                break;
                            case GrammarRule.Type.RemoveRight:
                                sufix = sufix.Remove(0, 1);
                                break;
                            case GrammarRule.Type.ReplaceLeft:
                                prefix = prefix.Remove(prefix.Length - 1);
                                prefix = prefix.Insert(prefix.Length - 1, rule.addition);
                                break;
                            case GrammarRule.Type.ReplaceRight:
                                sufix = sufix.Remove(0);
                                sufix = sufix.Insert(0, rule.addition);
                                break;
                            case GrammarRule.Type.Insert:
                                prefix += rule.addition;
                                break;
                            case GrammarRule.Type.Append:
                                sufix += rule.addition;
                                break;
                            case GrammarRule.Type.MergeInto:
                                prefix = prefix.Remove(prefix.Length - 1);
                                sufix = sufix.Remove(0);
                                prefix += rule.addition;
                                break;
                            default:
                                break;
                        }
                        dirty = true;
                        break;
                    }
                }
            }

            return prefix + sufix;
        }
    }

    [System.Serializable]
    public class GrammarRule
    {
        public enum Type:int
        {
            RemoveLeft,
            RemoveRight,
            ReplaceLeft,
            ReplaceRight,
            Insert,
            Append,
            MergeInto,
        }

        public char left;
        public char right;
        public string addition;
        public Type type;
    }

    [System.Serializable]
    public class TitleSet
    {
        public string id;
        public string opposingId;

        public List<string> adjectives = new List<string>();

        public List<string> objectivePros = new List<string>();
        public List<string> objectivesNeutral = new List<string>();

        public List<string> subjectivesPros = new List<string>();
        public List<string> subjectivesCons = new List<string>();
        public List<string> subjectivesNeutral = new List<string>();

        public List<string> genetives = new List<string>();
    }
}