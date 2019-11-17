using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoRDeckCodes;

public class CardPanel : MonoBehaviour
{
    public static CardPanel instance = null;

    [SerializeField] GameObject cardPanel;

    [SerializeField] ZoomInCard zoom;


    string currentZoomedInCard;

    private void Awake()
    {
        instance = this;
    }

    public void Setup(List<CardCodeAndCount> cards)
    {
        int i = 0;
        CardCodeAndCount c;
        foreach (Transform t in cardPanel.transform)
        {
            if (i < cards.Count)
            {
                t.gameObject.SetActive(true);
                c = cards[i];
                t.GetComponent<CardDisplay>().Setup(c.CardCode, c.Count);
            }
            else
            {
                t.gameObject.SetActive(false);
            }
            i += 1;
        }
    }


    public void ZoomIn(Sprite s, string cardCode)
    {
        zoom.gameObject.SetActive(true);
        zoom.SetImage(s, cardCode);
    }

    public void UpdateAmountOfCard(int amount, string cardCode)
    {
        CardManager.instance.UpdateAmountOfCard(cardCode, amount);
    }
}
