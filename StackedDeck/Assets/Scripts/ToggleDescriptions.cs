using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToggleDescriptions : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deckName;
    [SerializeField] TextMeshProUGUI deckDetails;

    Toggle t = null;
    string[] details = new string[5];
    bool hasDeck = false;

    public void Setup(string name, int region1, int region2, int numR1, int numR2, string c)
    {
        hasDeck = true;
        deckName.text = name;
        deckDetails.text = string.Format("{0}: {1}, {2}: {3}", Utilities.IndexToRegion[region1], numR1, Utilities.IndexToRegion[region2], numR2);
        details[0] = c;
        details[1] = Utilities.IndexToRegion[region1];
        details[2] = Utilities.IndexToRegion[region2];
        details[3] = numR1.ToString();
        details[4] = numR2.ToString();
        IsInteractable(true);
    }

    public void IsInteractable(bool interact)
    {
        if (t == null)
        {
            t = GetComponent<Toggle>();
        }
        if (interact)
        {
            t.interactable = true;
        }
        else
        {
            t.interactable = false;
        }
        t.isOn = false;
    }

    public string[] GetDetails()
    {
        return details;
    }

    public bool Selected()
    {
        return t.isOn;
    }

    public string GetDeckAsString()
    {
        if (hasDeck)
        {
            string name = "";
            name += deckName.text;
            while (name.Length < 20)
            {
                name += ',';
            }
            int numR1, numR2;
            int.TryParse(details[3], out numR1);
            numR2 = 40 - numR1;
            return details[0] + Utilities.RegionToIndex[details[1]] + Utilities.RegionToIndex[details[2]] + (numR1 < 10 ? "0" + numR1.ToString() : numR1.ToString()) + (numR2 < 10 ? "0" + numR2.ToString() : numR2.ToString()) + name;
        }
        else
            return "";


    }

    public void ClearDetails()
    {
        deckName.text = "";
        deckDetails.text = "";
        hasDeck = false;
    }

}