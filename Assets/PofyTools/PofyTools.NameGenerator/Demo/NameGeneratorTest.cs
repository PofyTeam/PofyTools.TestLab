using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools.NameGenerator;
using UnityEngine.UI;
using PofyTools;
using PofyTools.Distribution;

public class NameGeneratorTest : MonoBehaviour
{

    public NameData data;

    public Text label;

    void Awake()
    {
        this.data.Initialize();
    }

    //    public void Generate()
    //    {
    //        this.label.text = this.data.GenerateName("angel", "good");
    //    }

    public void GenerateStory()
    {
        bool useAdjective = Chance.FiftyFifty;
//        Debug.LogError("useAdjective " + useAdjective);
        bool useSubjective = Chance.FiftyFifty;
//        Debug.LogError("useSubjective " + useSubjective);
        bool useGenetive = !useSubjective || Chance.FiftyFifty;
//        Debug.LogError("useGenetive " + useGenetive);
        string story = this.data.GenerateStoryName(true, true, true);
//        Debug.LogError(story);
        this.label.text = story.ToTitle();

    }

    public void GenerateTrueRandom()
    {
        this.label.text = this.data.GenerateTrueRandomName().ToTitle();
    }

    [ContextMenu("Save")]
    public void Save()
    {
        NameData.SaveData(this.data);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        NameData.LoadData(this.data);
    }
}
