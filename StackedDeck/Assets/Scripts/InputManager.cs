using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LoRDeckCodes;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    [SerializeField] Queries queries;
    [SerializeField] RandomizedDeck random;


    [SerializeField] CardPanel panel;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    #region Deck Generation when the Generate button is clicked
    public string GenerateDeck(string r1, string r2, string num1Text, string num2Text)
    {
        List<CardCodeAndCount> cards;
        cards = CardManager.instance.GenerateInfoForDeck(r1, r2, int.Parse(num1Text), (int.Parse(num2Text)));
        return LoRDeckEncoder.GetCodeFromDeck(cards);
    }
    #endregion


    #region OtherMethods
    //Function called when the View deck button is pressed. Preps all the cards that were generated
    public void DisplayCardsInDeck(List<CardCodeAndCount>cards)
    {
        if(cards.Count > 0)
        {
            panel.gameObject.SetActive(true);
            panel.Setup(cards);
        }
    }

    //Emptying every query 
    public void ClearAndCloseAll()
    {
        queries.ClearAllEntries();
        queries.DeleteLoadedDecks();
        random.ClearQuery();
    }


    //self-explanatory; called by user manager I believe
    public void ShouldAllowUpdate()
    {
        queries.ShouldAllowUpdates(UserManager.instance.player != null);
    }



    //Function that copies the deck code to clipboard for easy copy paste
    public void CopyToClipBoard(string text)
    {
        Utilities.CopyToClipBoard(text);
    }

    #endregion
}

