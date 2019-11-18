using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using LoRDeckCodes;

public class UpdateCard : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    [SerializeField] TMP_Dropdown dropdown;

    [SerializeField] CardPanel cardPanel;
    [SerializeField] GameObject errorPanel;

    List<CardCodeAndCount> cards = new List<CardCodeAndCount>();

    public void CheckForCard()
    {
        TypeOfCard card;
        System.Enum.TryParse<TypeOfCard>(dropdown.options[dropdown.value].text, out card);
        cards = CardManager.instance.GetCardsWithName(input.text, card);
        if (cards.Count == 0)
        {
            errorPanel.SetActive(true);
        }
        else
        {
            cardPanel.gameObject.SetActive(true);
            cardPanel.Setup(cards);
        }
    }
}
