using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoRDeckCodes;
using UnityEngine.UI;

public class CardPanel : MonoBehaviour
{
    public static CardPanel instance = null;

    [SerializeField] GameObject cardPanel;

    [SerializeField] ZoomInCard zoom;

    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    List<CardCodeAndCount> currentCards = new List<CardCodeAndCount>();


    string currentZoomedInCard;

    int currentIndex = 0;
    int currentPage = 0;

    private void Awake()
    {
        instance = this;
    }

    //Called after this object has been enabled, sets up the entire grid of cards for display
    public void Setup(List<CardCodeAndCount> cards)
    {
        currentIndex = 0;
        CardCodeAndCount c;
        currentCards = cards;
        Debug.Log(cards.Count);
        foreach (Transform t in cardPanel.transform)
        {
            if (currentIndex < cards.Count)
            {
                t.gameObject.SetActive(true);
                c = cards[currentIndex];
                t.GetComponent<CardDisplay>().Setup(c.CardCode, c.Count);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
            currentIndex += 1;
        }
        if(cards.Count > 27)
        {
            leftButton.gameObject.SetActive(true);
            leftButton.interactable = false;
            rightButton.gameObject.SetActive(true);
        }
        else
        {
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
        }
        Debug.Log(currentIndex);
    }

    //Only called when searching for a certain card via the UpdateCard button. It just loads the next set of cards to display
    public void NextSetOfCards()
    { 
        CardCodeAndCount c;
        foreach(Transform t in cardPanel.transform)
        {
            if (currentIndex < currentCards.Count)
            {
                t.gameObject.SetActive(true);
                c = currentCards[currentIndex];
                t.GetComponent<CardDisplay>().Setup(c.CardCode, c.Count);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
            currentIndex += 1;
        }
        currentPage += 1;
        if(currentIndex > currentCards.Count)
        {
            rightButton.interactable = false;
        }
        leftButton.interactable = true;
        Debug.Log(currentIndex);
    }

    //Retrieves the earlier set of cards
    public void PreviousSetOfCards()
    {
        CardCodeAndCount c;
        currentPage -= 1;
        currentIndex = currentPage * 27;
        Debug.Log(currentIndex);
        foreach (Transform t in cardPanel.transform)
        {
            if (currentIndex < currentCards.Count)
            {
                t.gameObject.SetActive(true);
                c = currentCards[currentIndex];
                t.GetComponent<CardDisplay>().Setup(c.CardCode, c.Count);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
            currentIndex += 1;
        }
        if (currentPage == 0)
        {
            leftButton.interactable = false;
        }
        rightButton.interactable = true;
    }


    //Zooms in on a certain card
    public void ZoomIn(Sprite s, string cardCode)
    {
        zoom.gameObject.SetActive(true);
        zoom.SetImage(s, cardCode);
    }

    //Sends it to the cardmanager to update an amount of a card
    public void UpdateAmountOfCard(int amount, string cardCode)
    {
        CardManager.instance.UpdateAmountOfCard(cardCode, amount);
    }
}
