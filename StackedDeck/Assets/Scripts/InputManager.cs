using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    [SerializeField] TMP_InputField numInRegion1;
    [SerializeField] TMP_InputField numInRegion2;

    string r1 = "";
    string r2 = "";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void UpdateOtherNumber(int num)
    {
        if(num == 1)
        {
            numInRegion2.text = (numInRegion1.text == "" ? "" : (40 - int.Parse(numInRegion1.text)).ToString());
        }
        else
        {
            numInRegion1.text = (numInRegion2.text == "" ? "" : (40 - int.Parse(numInRegion2.text)).ToString());
        }
    }

    public void RegionOne(string region)
    {
        r1 = (r1 == region ? "" : region);
        Debug.Log("REGION 1 IS: " + r1);
    }

    public void RegionTwo(string region)
    {
        r2 = (r2 == region ? "" : region);
        Debug.Log("REGION 2 IS: " + region);
    }

    public void GenerateDeck()
    {
        CardManager.instance.GenerateInfoForDeck(r1, r2, (numInRegion1.text == "" ? -1 : int.Parse(numInRegion1.text)), (numInRegion2.text == "" ? -1 : int.Parse(numInRegion2.text)));
    }
}
