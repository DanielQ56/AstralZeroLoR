using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LoRDeckCodes;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    [SerializeField] TMP_InputField numInRegion1;
    [SerializeField] TMP_InputField numInRegion2;
    [SerializeField] TextMeshProUGUI region1;
    [SerializeField] TextMeshProUGUI region2;
    [SerializeField] TextMeshProUGUI deckCode;
    [SerializeField] CardPanel panel;

    string r1 = "";
    string r2 = "";

    List<CardCodeAndCount> cards = new List<CardCodeAndCount>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    #region Autofilling in Number Of Cards
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
    #endregion

    #region Handling displaying of region selected
    public void RegionOne(string region)
    {
        r1 = (r1 == region ? "" : region);
        region1.text = r1;
    }

    public void RegionTwo(string region)
    {
        r2 = (r2 == region ? "" : region);
        region2.text = r2;
    }
    #endregion

    #region Deck Generation when the Generate button is clicked
    public void GenerateDeck()
    {
        CheckForEmptyFields();
        cards = CardManager.instance.GenerateInfoForDeck(r1, r2, int.Parse(numInRegion1.text), (int.Parse(numInRegion2.text)));
        deckCode.text = LoRDeckEncoder.GetCodeFromDeck(cards);
    }

    void CheckForEmptyFields()
    {
        GenerateAmountPerRegion();
        GenerateRegions();
    }

    public void RandomizeValues()
    {
        r1 = "";
        r2 = "";
        numInRegion1.text = "";
        numInRegion2.text = "";
        GenerateAmountPerRegion();
        GenerateRegions();
    }

    void GenerateAmountPerRegion()
    {
        if (numInRegion1.text == "")
        {
            numInRegion1.text = Random.Range(0, 41).ToString();
        }
    }

    void GenerateRegions()
    {
        if (r1 == "" && r2 == "")
        {
            RegionOne(Utilities.IndexToRegion[Random.Range(0, 6)]);
            do
            {
                RegionTwo(Utilities.IndexToRegion[Random.Range(0, 6)]);
            } while (r1 == r2);
        }
        else if (r1 == "")
        {
            do
            {
                RegionOne(Utilities.IndexToRegion[Random.Range(0, 6)]);
            } while (r1 == r2);
        }
        else if (r2 == "")
        {
            do
            {
                RegionTwo(Utilities.IndexToRegion[Random.Range(0, 6)]);
            } while (r1 == r2);
        }
        Debug.Log(r1 + " " + r2);
    }
    #endregion

    public void DisplayCardsInDeck()
    {
        if(cards.Count > 0)
        {
            panel.gameObject.SetActive(true);
            panel.Setup(cards);
        }
    }

    //Function that copies the deck code to clipboard for easy copy paste
    public void CopyToClipBoard()
    {
        Utilities.CopyToClipBoard(deckCode.text);
    }
}
