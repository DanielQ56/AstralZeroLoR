using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LoRDeckCodes;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    #region InputQueries
    [SerializeField] TMP_InputField numInRegion1;
    [SerializeField] TMP_InputField numInRegion2;
    [SerializeField] TextMeshProUGUI region1;
    [SerializeField] TextMeshProUGUI region2;
    [SerializeField] TextMeshProUGUI deckCode;
    #endregion



    [SerializeField] CardPanel panel;
    [SerializeField] GameObject update;
    
    #region LoadingDeckObjects
    [SerializeField] LoadDecks deckLoader;
    [SerializeField] TMP_InputField deckName;
    [SerializeField] GameObject SaveDeckPanel;
    [SerializeField] GameObject SaveButton;
    [SerializeField] GameObject LoadButton;
    #endregion
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

    #region Loading previously saved decks
    public void LoadSavedDecks()
    {
        deckLoader.SetupLoadedDecks(UserManager.instance.GetLoadedDecks());
    }

    public void LoadDeck(string[] info)
    {
        ClearAllEntries();
        cards.AddRange(LoRDeckEncoder.GetDeckFromCode(info[0]));
        deckCode.text = info[0];
        RegionOne(info[1]);
        RegionTwo(info[2]);
        numInRegion1.text = info[3];
    }

    public void OpenSavePanel()
    {
        if(deckCode.text.Length > 0)
        {
            SaveDeckPanel.SetActive(true);
        }
        else
        {
            SaveDeckPanel.SetActive(false);
        }
    }

    public void SaveDeck()
    {
        if (deckCode.text.Length > 0)
        {
            string name = "";
            if (deckName.text.Length == 0)
            {
                deckName.text = "NewDeck";
            }
            name += deckName.text;
            while(name.Length < 20)
            {
                name += ',';
            }
            int numR1, numR2;
            int.TryParse(numInRegion1.text, out numR1);
            numR2 = 40 - numR1;
            string deckstring = deckCode.text + Utilities.RegionToIndex[r1] + Utilities.RegionToIndex[r2] + (numR1 < 10 ? "0" + numR1.ToString() : numR1.ToString()) + (numR2 < 10 ? "0" + numR2.ToString() : numR2.ToString()) + name;
            deckLoader.SaveDeck(deckstring);
        }
        
    }

    #endregion


    #region OtherMethods
    //Function called when the View deck button is pressed. Preps all the cards that were generated
    public void DisplayCardsInDeck()
    {
        if(cards.Count > 0)
        {
            panel.gameObject.SetActive(true);
            panel.Setup(cards);
        }
    }

    //Emptying every query 
    public void ClearAllEntries()
    {
        r1 = "";
        r2 = "";
        region1.text = r1;
        region2.text = r2;
        numInRegion1.text = "";
        numInRegion2.text = "";
        deckCode.text = "";
        cards.Clear();
        panel.ClosePanel();
        deckLoader.CloseAllPanels();
    }

    void CloseAllOpenPanels()
    {

    }

    //self-explanatory; called by user manager I believe
    public void ShouldAllowUpdate()
    {
        if(UserManager.instance.player == null)
        {
            update.SetActive(false);
            LoadButton.SetActive(false);
            SaveButton.SetActive(false);
        }
        else
        {
            update.SetActive(true);
            SaveButton.SetActive(true);
            LoadButton.SetActive(true);
        }
    }



    //Function that copies the deck code to clipboard for easy copy paste
    public void CopyToClipBoard()
    {
        Utilities.CopyToClipBoard(deckCode.text);
    }

    #endregion
}

