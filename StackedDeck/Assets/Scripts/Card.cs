using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LoRDeckCodes;


//Enum used for updating a card
[System.Serializable] 
public enum TypeOfCard
{
    Spell,
    Unit,
    Champion
}

//This is the class that I use to send the Web Requests to update the datbase
[System.Serializable]
public class PlayerData
{
    public string id;
    public string password;
    public string r1;
    public string r2;
    public string r3;
    public string r4;
    public string r5;
    public string r6;
    public string d1;
    public string d2;
    public string d3;

    public PlayerData(string i, string pass, string R1, string R2, string R3, string R4, string R5, string R6, string D1 = "", string D2 = "", string D3 = "")
    {
        id = i;
        password = pass;
        r1 = R1;
        r2 = R2;
        r3 = R3;
        r4 = R4;
        r5 = R5;
        r6 = R6;
        d1 = D1;
        d2 = D2;
        d3 = D3;
    }
}


//I think this is used in LoadJSON when we already have a datapath set up 
[System.Serializable]
public class RegionsJSON
{
    public string[][] AllCardsByRegion;

    public RegionsJSON(string[][] cards)
    {
        AllCardsByRegion = cards;
    }
    
}

//Holds all the cards belonging to that Region for easy access
[System.Serializable]
public class Region
{
    public string name;
    public List<Card> units = new List<Card>();
    public List<Card> champions = new List<Card>();
    public List<Card> spells = new List<Card>();
    public Dictionary<string, int> CountOfAllCards = new Dictionary<string, int>();
    public Dictionary<string, string> CardCodeToName = new Dictionary<string, string>();

    //Constructor
    public Region(string n, List<Card> cards, string cardsWithCount = "")
    {
        bool loadPlayerData = (cardsWithCount != "");
        if(loadPlayerData)
        {
            SetupCardCountDictionary(cardsWithCount);
        }
        for (int i = 0; i < cards.Count; ++i)
        {
            cards[i].name = cards[i].name.Replace(",", string.Empty).ToLower();
            if (cards[i].cardCode.Length <= 7)
            {
                if (cards[i].type == "Spell")
                {

                    spells.Add(cards[i]);
                }
                else if (cards[i].type == "Unit")
                {
                    if (cards[i].supertype == "Champion")
                    {
                        champions.Add(cards[i]);
                    }
                    else
                    {
                        units.Add(cards[i]);
                    }
                }
            }
            CardCodeToName.Add(cards[i].cardCode, cards[i].name);
            if(!loadPlayerData)
                CountOfAllCards.Add(cards[i].name, 3);
        }
        
    }

    //This is to set up the dictionary that holds how many of each card the player has. 0 if the player does not own the card
    void SetupCardCountDictionary(string cards)
    {
        string cardstring = cards.Trim(',');
        string[] allCards = cardstring.Split(',');
        int amount;
        foreach(string s in allCards)
        {
            amount = int.Parse(s.Substring(0, 1));
            CountOfAllCards.Add(s.Substring(1), amount);
        }
    }

    //Used by loadJSON to store cards onto the data path. Kinda redundent might clean this up later
    public string[] AllCardsInRegion()
    {
        string[] allCards = new string[units.Count + champions.Count + spells.Count];
        int i = 0;
        for(int a = 0; i < units.Count; ++i, ++a)
        {
            allCards[i] = JsonUtility.ToJson(units[a]);
        }
        for (int a = 0; i < (units.Count + champions.Count); ++i, ++a)
        {
            allCards[i] = JsonUtility.ToJson(champions[a]);
        }
        for (int a = 0; i < units.Count + champions.Count + spells.Count; ++i, ++a)
        {
            allCards[i] = JsonUtility.ToJson(spells[a]);
        }
        return allCards;
    }
    
    //Retrieves a random number of a random card to add to the deck. It Outs different variables needed in UserManager to determine whether or not to add them to the deck
    public CardCodeAndCount GetRandomCardAndAmount(int champsRemaining, int cardsNeeded, out bool isChampion, out string name, out int numOwned)
    {
        int randNum = Random.Range(0, (champsRemaining == 0 ? 2 : 3));
        int randAmount = Random.Range(1, (cardsNeeded < 5 ? cardsNeeded : 4));
        int randAmountChampions = Random.Range(1, (champsRemaining < cardsNeeded ? (champsRemaining < 5? champsRemaining : 4) : randAmount));
        CardCodeAndCount CCC = new CardCodeAndCount();
        Card c;
        isChampion = false;
        if (randNum == 0)
        {
            c = units[Random.Range(0, units.Count)];
            name = c.name;
            CCC.CardCode = c.cardCode;
            CCC.Count = randAmount;
            numOwned = CountOfAllCards[c.name];
        }
        else if(randNum == 1)
        {
            c = spells[Random.Range(0, spells.Count)];
            name = c.name;
            CCC.CardCode = c.cardCode;
            CCC.Count = randAmount;
            numOwned = CountOfAllCards[c.name];
        }
        else
        {
            c = champions[Random.Range(0, champions.Count)];
            name = c.name;
            CCC.CardCode = c.cardCode;
            CCC.Count = randAmountChampions;
            isChampion = true;
            numOwned = CountOfAllCards[c.name];
        }
        return CCC;
    }

    //Used for saving info onto database
    public string GetAllCardsAsString()
    {
        string s = "";
        foreach(KeyValuePair<string, int> pair in CountOfAllCards)
        {
            s += (pair.Value.ToString() + pair.Key) + ",";
        }
        return s;
    }

    //Updates the amount of a certain card
    public bool UpdateCardAmount(string code, int amount)
    {
        if (CardCodeToName.ContainsKey(code))
        {
            if (CountOfAllCards.ContainsKey(CardCodeToName[code]))
            {
                Debug.Log(CardCodeToName[code] + " " + amount);
                CountOfAllCards[CardCodeToName[code]] = amount;
                return true;
            }
        }
        return false;
    }


    //Returns the number of a certain card
    public int NumCopiesOfCard(string code)
    {
        if (CardCodeToName.ContainsKey(code))
        {
            if (CountOfAllCards.ContainsKey(CardCodeToName[code]))
            {
                return CountOfAllCards[CardCodeToName[code]];
            }
        }
        return -1;
    }

    //Returns a list of cards that have the input string within the name
    public List<CardCodeAndCount> CardsWithName(string name, TypeOfCard type)
    {
        List<CardCodeAndCount> cards = new List<CardCodeAndCount>();
        switch(type)
        {
            case TypeOfCard.Champion:
                foreach(Card c in champions)
                {
                    if(c.name.Contains(name))
                    {
                        CardCodeAndCount card = new CardCodeAndCount();
                        card.CardCode = c.cardCode;
                        card.Count = CountOfAllCards[c.name];
                        cards.Add(card);
                    }
                }
                break;
            case TypeOfCard.Spell:
                foreach (Card c in spells)
                {
                    if (c.name.Contains(name))
                    {
                        CardCodeAndCount card = new CardCodeAndCount();
                        card.CardCode = c.cardCode;
                        card.Count = CountOfAllCards[c.name];
                        cards.Add(card);
                    }
                }
                break;
            case TypeOfCard.Unit:
                foreach (Card c in units)
                {
                    if (c.name.Contains(name))
                    {
                        CardCodeAndCount card = new CardCodeAndCount();
                        card.CardCode = c.cardCode;
                        card.Count = CountOfAllCards[c.name];
                        cards.Add(card);
                    }
                }
                break;
        }
        return cards;
    }

    public override string ToString()
    {
        return name;
    }

}

//The Card class that JSONUtility uses to parse the contents of the JSON file
[System.Serializable]
public class Card
{
    public string[] associatedCards;
    public string[] associatedCardRefs;
    public string region;
    public string regionRef;
    public string attack;
    public string cost;
    public string health;
    public string description;
    public string descriptionRaw;
    public string flavorText;
    public string artistName;
    public string name;
    public string cardCode;
    public string[] keywordRefs;
    public string spellSpeed;
    public string spellSpeedRef;
    public string rarity;
    public string rarityRef;
    public string subtype;
    public string supertype;
    public string type;
    public string collectible;

}


