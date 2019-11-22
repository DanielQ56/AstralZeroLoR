using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LoRDeckCodes;

public class Queries : MonoBehaviour
{
    #region InputQueries
    [SerializeField] TMP_InputField numInRegion1;
    [SerializeField] TMP_InputField numInRegion2;
    [SerializeField] TextMeshProUGUI region1;
    [SerializeField] TextMeshProUGUI region2;
    [SerializeField] TextMeshProUGUI deckCode;
    #endregion

    #region Buttons for User if Logged in
    [SerializeField] GameObject update;
    [SerializeField] GameObject SaveButton;
    [SerializeField] GameObject LoadButton;
    #endregion

    [SerializeField] LoadDecks load;

    List<CardCodeAndCount> cards = new List<CardCodeAndCount>();

    #region Autofilling in Number Of Cards
    public void UpdateOtherNumber(int num)
    {
        if (num == 1)
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
        region1.text = (region1.text == region ? "" : region);
    }

    public void RegionTwo(string region)
    {
        region2.text = (region2.text == region ? "" : region);
    }
    #endregion

    #region Deck Generation when the Generate button is clicked

    public void GenerateDeck()
    {
        CheckForEmptyFields();
        deckCode.text = InputManager.instance.GenerateDeck(region1.text, region2.text, numInRegion1.text, numInRegion2.text);
        cards = LoRDeckEncoder.GetDeckFromCode(deckCode.text);

    }

    void CheckForEmptyFields()
    {
        GenerateAmountPerRegion();
        GenerateRegions();
    }

    public string GenerateRandomDeck()
    {
        RandomizeValues();
        Debug.Log(region1.text + " " + region2.text + " " + numInRegion1.text + " " + numInRegion2.text);
        GenerateDeck();
        return deckCode.text;
    }

    public void RandomizeValues()
    {
        region1.text = "";
        region2.text = "";
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
        if (region1.text == "" && region2.text == "")
        {
            RegionOne(Utilities.IndexToRegion[Random.Range(0, 6)]);
            do
            {
                RegionTwo(Utilities.IndexToRegion[Random.Range(0, 6)]);
            } while (region1.text == region2.text);
        }
        else if (region1.text == "")
        {
            do
            {
                RegionOne(Utilities.IndexToRegion[Random.Range(0, 6)]);
            } while (region1.text == region2.text);
        }
        else if (region2.text == "")
        {
            do
            {
                RegionTwo(Utilities.IndexToRegion[Random.Range(0, 6)]);
            } while (region1.text == region2.text);
        }
    }
    #endregion

    #region Loading previously saved decks

    public void LoadDeck(string[] info)
    {
        ClearAllEntries();
        cards.AddRange(LoRDeckEncoder.GetDeckFromCode(info[0]));
        deckCode.text = info[0];
        RegionOne(info[1]);
        RegionTwo(info[2]);
        numInRegion1.text = info[3];
    }

    public void TryToSave()
    {
        if (deckCode.text.Length > 0)
        {
            int numR1 = int.Parse(numInRegion1.text);
            int numR2 = int.Parse(numInRegion2.text);
            string deckstring = deckCode.text + Utilities.RegionToIndex[region1.text] + Utilities.RegionToIndex[region2.text] + (numR1 < 10 ? "0" + numR1.ToString() : numR1.ToString()) + (numR2 < 10 ? "0" + numR2.ToString() : numR2.ToString());
            load.OpenSavePanel(deckstring);
        }
    }

    #endregion

    #region OtherMethods
    //Function called when the View deck button is pressed. Preps all the cards that were generated
    public void ViewDeck()
    {
        InputManager.instance.DisplayCardsInDeck(cards);
    }

    public void ShouldAllowUpdates(bool shouldUpdate)
    {
        if(shouldUpdate)
        {
            update.SetActive(true);
            SaveButton.SetActive(true);
            LoadButton.SetActive(true);
        }
        else
        {
            update.SetActive(false);
            LoadButton.SetActive(false);
            SaveButton.SetActive(false);
        }
    }

    //Emptying every query 
    public void ClearAllEntries()
    {
        region1.text = "";
        region2.text = "";
        numInRegion1.text = "";
        numInRegion2.text = "";
        deckCode.text = "";
        cards.Clear();
        load.CloseAllPanels();
    }

    public void CopyToClipBoard()
    {
        InputManager.instance.CopyToClipBoard(deckCode.text);
    }
    #endregion
}
