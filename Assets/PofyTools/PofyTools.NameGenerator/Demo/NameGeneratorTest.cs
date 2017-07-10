using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools.NameGenerator;
using UnityEngine.UI;

public class NameGeneratorTest : MonoBehaviour
{

    public NameData data;

    public Text label;

    void Awake()
    {
        this.data.Initialize();
    }

    public void Generate()
    {
        this.label.text = this.data.GenerateName("angel", "good");
    }

    public void GenerateStory()
    {
        this.label.text = this.data.GenerateStoryName();
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
