using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryPlate : MonoBehaviour
{
    public enum Type
    {
        None =0,
        BaseCategory = 1,
        Category = 2,
        Subcategory =3,
    }
    public Button buttonSelectCategory, buttonRemoveCategory;
    public Type type;
}
