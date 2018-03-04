using Guvernal.CardGame;
using PofyTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryEditor : MonoBehaviour, IContentProvider<List<string>>
{
    [Header ("UI Resources")]
    public CategoryPlate platePrefab;
    public ScrollRect scrollPrefab;

    [Header ("UI Componenets")]
    public ScrollRect categoryView;

    [Header ("Game Data")]
    public List<CategoryDefinition> categoryDefs;

    #region Selected Data

    protected CategoryData _currentCategory;
    protected CategoryDefinition _currentDefinition
    {
        get
        {
            if (this._currentCategory != null)
                return this._currentCategory.definition;
            else
                return null;
        }
    }

    #endregion

    #region Selected Definition View

    [Header ("SelectedCategory")]
    public CanvasGroup definitionCanvasGroup;

    public InputField displayName, categoryDescription;

    public ScrollRect baseCategories;
    public ScrollRect subcategories;

    public AutocompleteInputField addBaseCategory;

    #endregion

    #region API
    [ContextMenu ("Load Definitions")]
    public void LoadDefinitions ()
    {
        GameDefinitions.Init ();
        GameDefinitions.ReloadData ();

        this.categoryDefs = GameDefinitions.Categories.GetContent ();

        this.addBaseCategory.SetProvider (this);

        this.addBaseCategory.inputField.onEndEdit.AddListener (this.OnEndEdit);

        ClearAll ();
        RefreshCategories ();
    }

    public void RefreshCategories ()
    {
        foreach (var data in GameDefinitions.CategoryData.GetContent ())
        {
            //Debug.Log (data.id);
            AddCategoryPlate (data.id, CategoryPlate.Type.Category);
        }
    }

    public List<string> GetContent ()
    {
        List<string> result = GameDefinitions.CategoryData.GetKeys ();
        result.Remove (this._currentCategory.id);
        foreach (var baseCategory in this._currentCategory.definition.baseCategories)
        {
            result.Remove (baseCategory);
        }
        return result;
    }

    void OnEndEdit (string value)
    {
        if (!string.IsNullOrEmpty (value)
            && GameDefinitions.CategoryData.GetKeys ().Contains (value)
            && !this._currentCategory.definition.baseCategories.Contains (value))
        {
            this._currentCategory.definition.baseCategories.Add (value);
        }
        ClearAll ();
        RefreshCategories ();
        SelectData (this._currentCategory);
    }

    public void SelectData (CategoryData data)
    {
        this._currentCategory = data;
        this.displayName.text = data.definition.displayName;
        this.categoryDescription.text = data.definition.categoryDescription;

        foreach (var baseCategory in data.definition.baseCategories)
        {
            if (GameDefinitions.CategoryData.GetValue (baseCategory) != null)
                AddCategoryPlate (baseCategory, CategoryPlate.Type.BaseCategory);
            else
                Debug.LogWarning ("Category \"" + baseCategory + "\" does not exist.");
        }

        foreach (var subcategory in data.subcategories)
        {
            if (GameDefinitions.CategoryData.GetValue (subcategory) != null)
                AddCategoryPlate (subcategory, CategoryPlate.Type.Subcategory);
            else
                Debug.LogWarning ("Category \"" + subcategory + "\" does not exist.");
        }

        this.definitionCanvasGroup.interactable = true;
        this.definitionCanvasGroup.alpha = 1;
    }

    protected void AddCategoryPlate (string categoryKey, CategoryPlate.Type type)
    {
        CategoryPlate plate = Instantiate (this.platePrefab);
        plate.type = type;

        plate.buttonSelectCategory.onClick.AddListener (delegate ()
        {
            SaveDefinitions ();
            this.baseCategories.content.ClearChildren ();
            this.subcategories.content.ClearChildren ();
            SelectCategory (categoryKey);
        });

        switch (type)
        {
            case CategoryPlate.Type.BaseCategory:
                plate.buttonRemoveCategory.onClick.AddListener (delegate ()
                {
                    this._currentCategory.definition.baseCategories.Remove (categoryKey);
                    SaveDefinitions ();
                    ClearAll ();
                    RefreshCategories ();
                    SelectData (this._currentCategory);
                });

                plate.transform.SetParent (this.baseCategories.content, false);
                break;
            case CategoryPlate.Type.Category:
                plate.buttonRemoveCategory.onClick.AddListener (delegate ()
                {
                    this.categoryDefs.Remove (GameDefinitions.CategoryData.GetValue (categoryKey).definition);
                    SaveDefinitions ();
                    ClearAll ();
                    RefreshCategories ();
                });
                plate.transform.SetParent (this.categoryView.content, false);
                break;
            case CategoryPlate.Type.Subcategory:
                plate.buttonRemoveCategory.interactable = false;
                plate.transform.SetParent (this.subcategories.content, false);
                break;
            default:
                break;
        }


        plate.buttonSelectCategory.GetComponentInChildren<Text> ().text = categoryKey;
    }

    public void SelectCategory (string category)
    {
        var data = GameDefinitions.CategoryData.GetValue (category);
        SelectData (data);
    }

    public void ClearAll ()
    {
        this.categoryView.content.ClearChildren ();
        this.baseCategories.content.ClearChildren ();
        this.subcategories.content.ClearChildren ();
    }

    public InputField newCategory;

    public void AddCategory ()
    {
        string key = newCategory.text.ToLower ().Trim ();
        if (!string.IsNullOrEmpty (key))
        {
            if (GameDefinitions.CategoryData.GetValue (key) != null)
            {
                Debug.LogWarning ("Key \"" + key + " \" already present in categories.");
            }
            else
            {
                this.categoryDefs.Add (new CategoryDefinition (key));
                SaveDefinitions ();
                //LoadDefinitions ();
                ClearAll ();
                RefreshCategories ();
                SelectCategory (key);
            }
        }
    }

    [ContextMenu ("Save Definitions")]
    public void SaveDefinitions ()
    {
        if (this._currentCategory != null)
        {
            this._currentCategory.definition.displayName = this.displayName.text;
            this._currentCategory.definition.categoryDescription = this.categoryDescription.text;
        }

        this.categoryDefs.Sort ((x, y) => x.id.CompareTo (y.id));

        foreach (var def in this.categoryDefs)
        {
            def.baseCategories.Sort ();
        }

        GameDefinitions.Categories.SetContent (this.categoryDefs);
        GameDefinitions.Categories.Save ();

        GameDefinitions.ReloadData ();
    }
    #endregion

    #region Mono

    private void Awake ()
    {
        LoadDefinitions ();
        this.definitionCanvasGroup.interactable = false;
        this.definitionCanvasGroup.alpha = 0;

    }

    #endregion

}
