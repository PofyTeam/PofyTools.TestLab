using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestConsole : MonoBehaviour
{
    [SerializeField]
    protected Text _text;
    protected static TestConsole _instance;
    public static void WriteLine(object arg)
    {
        _instance._text.text += "\n" + arg.ToString();
    }
    //public bool isMale = true;
    public string[] names;
    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {


        string pera = "Hello World";
        char[] charArrayPera = pera.ToCharArray();
        Array.Reverse(charArrayPera);

        pera = "";
        foreach (var chart in charArrayPera)
        {
            pera += chart;
        }

        TestConsole.WriteLine(pera);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
