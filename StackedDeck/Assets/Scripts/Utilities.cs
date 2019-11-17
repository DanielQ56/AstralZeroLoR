using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Dictionary<string, int> RegionToIndex = new Dictionary<string, int>
    {
        {"Demacia", 0 },
        {"Freljord", 1 },
        {"Ionia", 2 },
        {"Noxus", 3 },
        {"PiltoverZaun", 4 },
        {"ShadowIsles", 5 },
        {"", -1 }
    };

    public static Dictionary<int, string> IndexToRegion = new Dictionary<int, string>
    {
        {0, "Demacia"},
        {1, "Freljord"},
        {2, "Ionia" },
        {3, "Noxus" },
        {4, "PiltoverZaun"},
        {5, "ShadowIsles"},
        {-1, "" }
    };

    public static void CopyToClipBoard(this string s) //Taken from answers.unity.questions
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }
}
