using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoRDeckCodes;
using System.Linq;

public class CardManager : MonoBehaviour
{
    public static CardManager instance = null;

    const int maxNumChampions = 6;

    Region[] allRegions;

    List<CardCodeAndCount> cards = new List<CardCodeAndCount>();
    Dictionary<string, CardCodeAndCount> cardCodeAndAmount = new Dictionary<string, CardCodeAndCount>();
    Dictionary<string, string> names = new Dictionary<string, string>();
    int totalCardsFromR1;
    int totalCardsFromR2;
    Region r1;
    Region r2;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    //Loads all cards from the LoadJSON class into the respective regions
    public void LoadAllCards(PlayerData player = null)
    {
        allRegions = LoadJson.LoadResourceTextFile(player);
    }

    #region Actual Deck Generation Methods
    public List<CardCodeAndCount> GenerateInfoForDeck(string region1, string region2, int numReg1, int numReg2)
    {
        r1 = allRegions[Utilities.RegionToIndex[region1]];
        r2= allRegions[Utilities.RegionToIndex[region2]];
        totalCardsFromR1 = numReg1;
        totalCardsFromR2 = numReg2;

        cards.Clear();
        cardCodeAndAmount.Clear();
        names.Clear();
        return GenerateDeck();

    }

    List<CardCodeAndCount> GenerateDeck()
    {
        int numChampions = 0;
        int numR1 = 0;
        int numR2 = 0;
        bool isChampion;
        CardCodeAndCount c;
        string name;
        int numOwned;
        while (numR1 + numR2 < 40)
        {
            int index = Random.Range(0, 2);
            if (index == 0 && numR1 < totalCardsFromR1)
            {
                while (true)
                {
                    c = r1.GetRandomCardAndAmount(maxNumChampions - numChampions, totalCardsFromR1 - numR1, out isChampion, out name, out numOwned);
                    if (cardCodeAndAmount.ContainsKey(c.CardCode))
                    {
                        if (cardCodeAndAmount[c.CardCode].Count < numOwned) //Assuming the max number of cards owned is 3 since you can only have max 3 of each card in a deck
                        {
                            int remainingNum = numOwned - cardCodeAndAmount[c.CardCode].Count;
                            c.Count = (c.Count < remainingNum ? c.Count : remainingNum);
                            cardCodeAndAmount[c.CardCode].Count += c.Count;
                            break;
                        }

                    }
                    else
                    {
                        cardCodeAndAmount.Add(c.CardCode, c);
                        break;
                    }
                }
                if (isChampion)
                    numChampions += c.Count;
                numR1 += c.Count;

            }
            else if (numR2 < totalCardsFromR2)
            {
                while (true)
                {
                    c = r2.GetRandomCardAndAmount(maxNumChampions - numChampions, totalCardsFromR2 - numR2, out isChampion, out name, out numOwned);
                    if (cardCodeAndAmount.ContainsKey(c.CardCode))
                    {
                        if (cardCodeAndAmount[c.CardCode].Count < numOwned)
                        {
                            int remainingNum = numOwned - cardCodeAndAmount[c.CardCode].Count;
                            c.Count = (c.Count < remainingNum ? c.Count : remainingNum);
                            cardCodeAndAmount[c.CardCode].Count += c.Count;
                            break;
                        }

                    }
                    else
                    {
                        cardCodeAndAmount.Add(c.CardCode, c);
                        break;
                    }
                }
                if (isChampion)
                    numChampions += c.Count;
                numR2 += c.Count;
            }
        }
        foreach (KeyValuePair<string, CardCodeAndCount> pair in cardCodeAndAmount)
        {
            cards.Add(pair.Value);
        }
        return cards;
    }

    #endregion

    public void UpdateAmountOfCard(string cardCode, int amount)
    {
        bool shouldUpdate = false;
        foreach(Region r in allRegions)
        {
            if(r.UpdateCardAmount(cardCode, amount))
            {
                shouldUpdate = true;
            }
        }
        if (shouldUpdate)
            UserManager.instance.UpdateExistingUser();
    }

    public int GetNumCopiesOfCard(string cardCode)
    {
        int c = -1;
        foreach (Region r in allRegions)
        {
            c = r.NumCopiesOfCard(cardCode);
            if (c >= 0)
            {
                return c;
            }
        }
        return c;
    }

    //Used for saving the player's cards onto database
    public string[] GetAllCardsAsStrings()
    {
        return LoadJson.ConvertCardsToString(allRegions);
    }

}


