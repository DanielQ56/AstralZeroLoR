using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static Dictionary<string, int> RegionToIndex = new Dictionary<string, int>
    {
        {"Demacia", 0 },
        {"Freljord", 1 },
        {"Ionia", 2 },
        {"Noxus", 3 },
        {"PiltoverZaun", 4 },
        {"ShadowIsles", 5 }
    };

    public static Dictionary<int, string> IndexToRegion = new Dictionary<int, string>
    {
        {0, "Demacia"},
        {1, "Freljord"},
        {2, "Ionia" },
        {3, "Noxus" },
        {4, "PiltoverZaun"},
        {5, "ShadowIsles"}
    };
}
