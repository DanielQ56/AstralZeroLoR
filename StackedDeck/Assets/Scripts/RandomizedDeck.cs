using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomizedDeck : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deckCode;
    [SerializeField] Queries query;

    public void RandomDeck()
    {
        deckCode.text = query.GenerateRandomDeck();
    }

    public void ClearQuery()
    {
        deckCode.text = "";
    }

    public void ViewDeck()
    {
        query.ViewDeck();
    }
}

