using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PofyTools.NameGenerator;
using UnityEngine.UI;

public class NameGeneratorTest : MonoBehaviour
{

    public NameData data;

    public Text label;
    public Button button;

    void Awake()
    {
        this.data.Initialize();
        this.button.onClick.AddListener(this.Generate);
    }

    public void Generate()
    {
        this.label.text = this.data.GenerateName("angel", "good");
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
