﻿namespace PofyTools.NameGenerator
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System.IO;
    using System.Text;
    using System.Globalization;
    using PofyTools.Distribution;

    [System.Serializable]
    public class NameData:IInitializable
    {
        public const string TAG = "<color=green><b>NameData: </b></color>";

        #region Serializable Data

        [Header("Database Version")]
        public string dataVersion = "0.0";

        [Header("Name Sets")]
        public List<NameSet> setNames = new List<NameSet>();
        [Header("Title Sets")]
        public List<TitleSet> setTitles = new List<TitleSet>();
        [Header("Grammar Sets")]
        public List<GrammarSet> setGrammars = new List<GrammarSet>();

        [Header("Story Mode")]
        public List<string> subjectiveStory = new List<string>();
        [Header("Geolocation")]
        public List<string> subjectiveGeolocation = new List<string>();

        [Header("Syllable Generator")]
        public List<string> vowels = new List<string>();
        public List<string> vowelPairs = new List<string>();

        public List<string> consonantStart = new List<string>();
        public List<string> consonantOpen = new List<string>();
        public List<string> consonantClose = new List<string>();

        public List<string> maleEndSyllablesOpen = new List<string>();
        public List<string> maleEndSyllablesClose = new List<string>();
        public List<string> femaleEndSyllablesOpen = new List<string>();
        public List<string> femaleEndSyllablesClose = new List<string>();

        [Header("Numbers")]
        public List<string> numberOrdinals = new List<string>();
        public List<string> numberCardinals = new List<string>();

        #endregion

        #region Runtimes

        [System.NonSerialized]
        protected Dictionary<string,NameSet> _setNames = new Dictionary<string, NameSet>();
        [System.NonSerialized]
        protected List<string> _setNameIds = new List<string>();

        [System.NonSerialized]
        protected Dictionary<string,TitleSet> _setTitles = new Dictionary<string, TitleSet>();
        [System.NonSerialized]
        protected List<string> _setTitleIds = new List<string>();

        [System.NonSerialized]
        protected Dictionary<string,GrammarSet> _setGrammars = new Dictionary<string, GrammarSet>();
        [System.NonSerialized]
        protected List<string> _setGrammarIds = new List<string>();

        [System.NonSerialized]
        protected List<string> _allAdjectives = new List<string>();
        [System.NonSerialized]
        protected List<string> _allNouns = new List<string>();

        protected void CreateRuntimeCollections()
        {
            this._allNouns.Clear();
            this._allAdjectives.Clear();

            this._setNames.Clear();
            this._setNameIds.Clear();

            this._setTitles.Clear();
            this._setTitleIds.Clear();

            this._setGrammars.Clear();
            this._setGrammarIds.Clear();

            this._allNouns.AddRange(this.subjectiveGeolocation);

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

                foreach (var adjective in titleSet.adjectives)
                {
                    this._allAdjectives.Add(adjective);
                }

                foreach (var subjective in titleSet.subjectivesCons)
                {
                    this._allNouns.Add(subjective);
                }

                foreach (var subjective in titleSet.subjectivesPros)
                {
                    this._allNouns.Add(subjective);
                }

                foreach (var subjective in titleSet.subjectivesNeutral)
                {
                    this._allNouns.Add(subjective);
                }

                foreach (var genetive in titleSet.genetives)
                {
                    this._allNouns.Add(genetive);
                }

            }
        
            foreach (var grammarset in this.setGrammars)
            {
                if (this._setGrammars.ContainsKey(grammarset.nounSingular))
                    Debug.LogWarning(TAG + "Id " + grammarset.nounSingular + " already present in Grammer Sets. Owerwritting...");

                this._setGrammars[grammarset.nounSingular] = grammarset;
                this._setGrammarIds.Add(grammarset.nounSingular);
            }
        }

        #endregion

        #region API

        //FIXME
        //        public string GenerateName(string nameSetId = "", string titleSetId = "", bool useAdjective = true, bool useSubjective = true, bool useGenetive = true, bool male = true)
        //        {
        //
        //            NameSet nameSet = null;
        //            TitleSet titleSet = null;
        //            string final = string.Empty;
        //            CultureInfo cultureInfo = new CultureInfo("en-US", false);
        //            TextInfo textInfo = cultureInfo.TextInfo;
        //
        //            if (string.IsNullOrEmpty(nameSetId))
        //            {
        //                nameSetId = this._setNameIds[Random.Range(0, this._setNameIds.Count - 1)];
        //            }
        //
        //            if (this._setNames.TryGetValue(nameSetId, out nameSet))
        //            {
        //                final = textInfo.ToTitleCase(nameSet.prefixes[Random.Range(0, nameSet.prefixes.Count - 1)].ToLower(cultureInfo));
        //                final += nameSet.sufixes[Random.Range(0, nameSet.sufixes.Count - 1)];
        //            }
        //
        //            if (this._setTitles.TryGetValue(titleSetId, out titleSet))
        //            {
        //                if (useAdjective || useSubjective)
        //                {
        //                    final += " the ";
        //
        //                    if (useAdjective)
        //                    {
        //                        final += textInfo.ToTitleCase(titleSet.adjectives[Random.Range(0, titleSet.adjectives.Count - 1)].ToLower(cultureInfo));
        //                        final += " ";
        //                    }
        //
        //                    if (useSubjective)
        //                    {
        //                        TitleSet opposingSet = null;
        //
        //                        bool opposing = Random.Range(0f, 1f) > 0.5f && this._setTitles.TryGetValue(titleSet.opposingId, out opposingSet);
        //                        if (opposing)
        //                        {
        //                            final += textInfo.ToTitleCase(opposingSet.objectivesNeutral[Random.Range(0, opposingSet.objectivesNeutral.Count - 1)].ToLower(cultureInfo));
        //                            final += textInfo.ToTitleCase(this.subjectiveCons[Random.Range(0, this.subjectiveCons.Count - 1)].ToLower(cultureInfo));
        //                        }
        //                        else
        //                        {
        //                            final += textInfo.ToTitleCase(titleSet.objectivesNeutral[Random.Range(0, titleSet.objectivesNeutral.Count - 1)].ToLower(cultureInfo));
        //                            final += textInfo.ToTitleCase(this.subjectivePros[Random.Range(0, this.subjectivePros.Count - 1)].ToLower(cultureInfo));
        //                        }
        //                    }
        //                }
        //
        //                if (useGenetive)
        //                    final += " of " + textInfo.ToTitleCase(titleSet.genetives[Random.Range(0, titleSet.genetives.Count - 1)].ToLower(cultureInfo));
        //            }
        //
        //
        //            //final = textInfo.ToTitleCase(final);
        //
        //            return final;
        //        }
        //TODO

        public NameSet GetNameSet(string id)
        {
            NameSet nameset = null;
            this._setNames.TryGetValue(id, out nameset);
            return nameset;
        }

        public TitleSet GetTitleSet(string id)
        {
            TitleSet titleset = null;
            this._setTitles.TryGetValue(id, out titleset);
            return titleset;
        }

        public string GenerateStoryName(bool useAdjective = true, bool useSubjective = true, bool useGenetive = true)
        {
            string result = string.Empty;

            if (useSubjective)
            {
                result = "the ";
                if (useAdjective)
                {
                    result += GetAdjective();
                }

                result += this.subjectiveStory.GetRandom();
            }
            if (useGenetive)
            {
                //TODO: pick genetive from other titlesets
//                result += " of the ";
//                result += this._allNouns.GetRandom();
                result += " " + GetGenetive();
            }
            result = result.Trim();
            if (string.IsNullOrEmpty(result))
                result = "NULL(story)";
            return result;
        }

        public string GetGenetive()
        {
            GrammarSet grammarset = null;
            string result = "of ";
            string genetive = string.Empty;
            bool plural = Chance.FiftyFifty;

            bool useAdjective = Chance.TryWithChance(0.3f);

            if (!plural)
            {

                grammarset = this.setGrammars.GetRandom();
                if (grammarset.useDeterminer || useAdjective)
                {
                    result += "the ";
                    genetive = grammarset.nounSingular;
                }

                grammarset = this.setGrammars.GetRandom();
                result += (Chance.FiftyFifty) ? this.numberOrdinals.GetRandom() + " " : "";
                if (useAdjective)
                    result += grammarset.adjectives.GetRandom() + " ";
                result += genetive;
            }
            else
            {
                result += (Chance.FiftyFifty) ? this.numberCardinals.GetRandom() + " " : "";
                grammarset = this.setGrammars.GetRandom();
                while (grammarset.nounPlurals.Count == 0)
                {
                    grammarset = this.setGrammars.GetRandom();
                }

                genetive = grammarset.nounPlurals.GetRandom();
                if (useAdjective)
                {
                    grammarset = this.setGrammars.GetRandom();
                    result += grammarset.adjectives.GetRandom() + " ";
                }
                result += genetive;
            }
            return result;
        }

        public string GetRandomOrdinalNumber(int max = 1000)
        {
            return (Chance.TryWithChance(0.3f)) ? GetOrdinalNumber(Random.Range(4, max + 1)) : this.numberOrdinals.GetRandom();
        }

        public string GetAdjective()
        {
            string result = string.Empty;
            if (Chance.FiftyFifty)
            {
                string name = GetAnyName(Chance.FiftyFifty);
                if (!string.IsNullOrEmpty(name))
                    result += NameToAdjective(name) + " ";
                else
                {
                    Debug.LogError(TAG + "Empty name from GetAnyName!");
                    result += this._allAdjectives.GetRandom() + " ";
                }
            }
            else
                result += this._allAdjectives.GetRandom() + " ";

            return result;
        }

        public string GetAnyName(bool isMale = true)
        {
            if (Chance.FiftyFifty)
            {
                Debug.LogError(TAG + "Getting Name from name data...");
                return this.setNames.GetRandom().GetRandomName(isMale);
            }
            Debug.LogError(TAG + "Generating true random name...");
            return GenerateTrueRandomName(3, isMale);
        }

        public static string NameToAdjective(string name)
        {
            return name + "\'s";
        }

        #region Syllable Generator

        public string GenerateTrueRandomName(int maxSyllables = 3, bool isMale = true)
        {
            if (maxSyllables == 0)
                return "[zero syllables]";
            
            int syllableCount = Random.Range(1, maxSyllables + 1);
            Debug.LogError(TAG + syllableCount + " syllables.");
            int[] syllableLengths = GetSyllableLenghts(syllableCount);
            bool[] syllablesTypes = GetSyllableTypes(syllableLengths);
            string[] syllablesStrings = GetSyllableStrings(syllablesTypes, syllableLengths, isMale);

            string name = ConcatanateSyllables(syllablesStrings);
            return name;
        }

        public int[] GetSyllableLenghts(int syllableCount = 1)
        {
            int[] lenghts = new int[syllableCount];

            for (int i = 0; i < lenghts.Length; i++)
            {
                lenghts[i] = Random.Range(2, 4);
                Debug.LogError(lenghts[i].ToString());
            }

            return lenghts;
        }

        public bool[] GetSyllableTypes(int[] syllableLengths)
        {
            bool[] syllableTypes = new bool[syllableLengths.Length];

            for (var i = 0; i < syllableLengths.Length; i++)
            {
                if (syllableLengths[i] < 3 || Chance.FiftyFifty)
                {
                    syllableTypes[i] = true;
                }
                else
                {
                    syllableTypes[i] = false;
                }
                Debug.LogError(syllableTypes[i].ToString());
            }
            return syllableTypes;
        }

        public string[] GetSyllableStrings(bool[] types, int[] lengths, bool isMale = true)
        {
            string[] syllableStrings = new string[types.Length];

            for (var i = 0; i < types.Length; i++)
            {
                string result = string.Empty;

                //if it's a first syllable
                if (i == 0)
                {
                    //Try for vowel on start
                    if (types[i])
                    {
                        if (types.Length > 1 && Chance.TryWithChance(0.3f))
                        {
                            result = this.vowels.GetRandom();
                            syllableStrings[i] = result;
                            continue;
                        }
                        result = this.consonantStart.GetRandom();
                        result += this.vowels.GetRandom();
                        syllableStrings[i] = result;
                        continue;
                    }

                    if (lengths[i] > 2)
                    {
                        result = this.consonantOpen.GetRandom();
                        result += this.vowels.GetRandom();
                        result += this.consonantClose.GetRandom();
                        syllableStrings[i] = result;
                        continue;
                    }

                    result = this.vowels.GetRandom();
                    result += this.consonantClose.GetRandom();
                    syllableStrings[i] = result;
                    continue;
                }
                //if it's last
                else if (i == (types.Length - 1))
                {

                    if (isMale)
                        result = (types[i - 1]) ? this.maleEndSyllablesOpen.GetRandom() : this.maleEndSyllablesClose.GetRandom();
                    else
                        result = (types[i - 1]) ? this.femaleEndSyllablesOpen.GetRandom() : this.femaleEndSyllablesClose.GetRandom();

                    syllableStrings[i] = result;
                    continue;
                }
                //middle syllables
                if (types[i])
                {
                    result = this.consonantOpen.GetRandom();
                    result += this.vowels.GetRandom();
                    syllableStrings[i] = result;
                    continue;
                }

                if (lengths[i] > 2)
                {
                    result = this.consonantOpen.GetRandom();
                    result += this.vowels.GetRandom();
                    result += this.consonantClose.GetRandom();
                    syllableStrings[i] = result;
                    continue;
                }

                result = this.vowels.GetRandom();
                result += this.consonantClose.GetRandom();
                syllableStrings[i] = result;
                continue;

            }
            foreach (var value in syllableStrings)
            {
                Debug.LogError(value);
            }
            return syllableStrings;
        }

        protected string ConcatanateSyllables(string[] syllables)
        {
            string result = string.Empty;
            string left, right;
            for (int i = 0; i < syllables.Length; ++i)
            {
                
                if (i > 0)
                {
                    left = syllables[i - 1];
                    right = syllables[i];
                    if (left[left.Length - 1] == right[0])
                    {
                        right.PadRight(1);
                    }
                }

                result += syllables[i];
            }

            return result;
        }

        #endregion

        public static string GetOrdinalNumber(int number)
        {
            int remainder = number % 10;
            if (number < 10 || number > 20)
            {
                if (remainder == 1)
                {
                    return number + "st";
                }
                else if (remainder == 2)
                {
                    return number + "nd";
                }
                else if (remainder == 3)
                {
                    return number + "rd";
                }
                else
                {
                    return number + "th";
                }
            }
            return number + "th";
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

                for (int i = 0; i < titleset.objectivesNeutral.Count; i++)
                {
                    titleset.objectivesNeutral[i] = titleset.objectivesNeutral[i].ToLower();
                }

                titleset.objectivesNeutral.Sort();

                for (int i = 0; i < titleset.objectivesNeutral.Count; i++)
                {
                    titleset.objectivesNeutral[i] = titleset.objectivesNeutral[i].ToLower();
                }

                titleset.objectivesNeutral.Sort();
            }

//            this.subjectiveCons.Sort();
//            this.subjectivePros.Sort();
            this.subjectiveStory.Sort();
            this.subjectiveGeolocation.Sort();

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
            Optimize(this.subjectiveStory);
            Optimize(this.subjectiveGeolocation);

            Optimize(this.vowels);
            Optimize(this.vowelPairs);

            Optimize(this.consonantStart);
            Optimize(this.consonantOpen);
            Optimize(this.consonantClose);

            Optimize(this.maleEndSyllablesOpen);
            Optimize(this.maleEndSyllablesClose);
            Optimize(this.femaleEndSyllablesOpen);
            Optimize(this.femaleEndSyllablesClose);

            this.setNames.Sort((x, y) => x.id.CompareTo(y.id));
            foreach (var nameset in this.setNames)
            {
                Optimize(nameset.prefixes);
                Optimize(nameset.sufixes);
                Optimize(nameset.namesMale);
                Optimize(nameset.namesFemale);
            }

            this.setTitles.Sort((x, y) => x.id.CompareTo(y.id));
            foreach (var titleset in this.setTitles)
            {
                Optimize(titleset.adjectives);
                Optimize(titleset.genetives);
                Optimize(titleset.objectivePros);
                Optimize(titleset.objectivesNeutral);
                Optimize(titleset.subjectivesCons);
                Optimize(titleset.subjectivesNeutral);
                Optimize(titleset.subjectivesPros);

            }

            this.setGrammars.Sort((x, y) => x.nounSingular.CompareTo(y.nounSingular));
            foreach (var grammarset in this.setGrammars)
            {
                OptimizeString(grammarset.nounSingular);

                Optimize(grammarset.nounPlurals);
                Optimize(grammarset.adjectives);
            }
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

        public static List<string> Optimize(List<string>toOptimize)
        {
            toOptimize.Sort();
            for (int i = toOptimize.Count - 1; i >= 0; --i)
            {
                toOptimize[i] = toOptimize[i].Trim().ToLower();
                if (i < toOptimize.Count - 1)
                {
                    var left = toOptimize[i];
                    var right = toOptimize[i + 1];
                    if (left == right)
                    {
                        toOptimize.RemoveAt(i);
                    }
                }
            }
            return toOptimize;
        }

        public static string OptimizeString(string toOptimize)
        {
            return toOptimize.Trim().ToLower();
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
        public string GetRandomName(bool male = true)
        {
            if (this.prefixes.Count + this.sufixes.Count == 0)
            {
                Debug.LogError("No prefixes or sufixes in name set " + this.id);
                return GetName(male);
            }

            if ((this.namesMale.Count + this.namesFemale.Count == 0) || Chance.FiftyFifty)
                return GeneratePseudoName(male);    
            return GetName(male);   
        }

        /// <summary>
        /// Gets the real name from name set database.
        /// </summary>
        /// <returns>A real name from the database.</returns>
        /// <param name="male">Should real name be male or female name.</param>
        public string GetName(bool male = true)
        {
            List<string> list = (male) ? this.namesMale : this.namesFemale;
            if (list.Count != 0)
                return list.GetRandom();
            return "NULL(" + id + ")";
        }

        /// <summary>
        /// Generates a random pseudo name by concatenating prefix and sufix and by applying grammer rules.
        /// </summary>
        /// <returns>A pseudo name.</returns>
        /// <param name="male">Should pseudo name be male or female name.</param>
        public string GeneratePseudoName(bool male = true)
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
                                prefix = prefix.Insert(prefix.Length - 1, rule.affix);
                                break;
                            case GrammarRule.Type.ReplaceRight:
                                sufix = sufix.Remove(0);
                                sufix = sufix.Insert(0, rule.affix);
                                break;
                            case GrammarRule.Type.Insert:
                                prefix += rule.affix;
                                break;
                            case GrammarRule.Type.Append:
                                sufix += rule.affix;
                                break;
                            case GrammarRule.Type.MergeInto:
                                prefix = prefix.Remove(prefix.Length - 1);
                                sufix = sufix.Remove(0);
                                prefix += rule.affix;
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
                                prefix = prefix.Insert(prefix.Length - 1, rule.affix);
                                break;
                            case GrammarRule.Type.ReplaceRight:
                                sufix = sufix.Remove(0);
                                sufix = sufix.Insert(0, rule.affix);
                                break;
                            case GrammarRule.Type.Insert:
                                prefix += rule.affix;
                                break;
                            case GrammarRule.Type.Append:
                                sufix += rule.affix;
                                break;
                            case GrammarRule.Type.MergeInto:
                                prefix = prefix.Remove(prefix.Length - 1);
                                sufix = sufix.Remove(0);
                                prefix += rule.affix;
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

        //TODO:
        //        public string FeminizeName(string maleName){
        //
        //        }
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

    [System.Serializable]
    public class GrammarSet
    {
        //also used as id
        public string nounSingular;
        public bool useDeterminer = true;
        public List<string> nounPlurals;
        public List<string> adjectives;
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
        public string affix;
        public Type type;
    }
}