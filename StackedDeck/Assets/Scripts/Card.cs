using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using LoRDeckCodes;

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

    public PlayerData(string i, string pass, string R1, string R2, string R3, string R4, string R5, string R6)
    {
        id = i;
        password = pass;
        r1 = R1;
        r2 = R2;
        r3 = R3;
        r4 = R4;
        r5 = R5;
        r6 = R6;
    }
}



[System.Serializable]
public class RegionsJSON
{
    public string[][] AllCardsByRegion;

    public RegionsJSON(string[][] cards)
    {
        AllCardsByRegion = cards;
    }
    
}


[System.Serializable]
public class Region
{
    public string name;
    public List<Card> units = new List<Card>();
    public List<Card> champions = new List<Card>();
    public List<Card> spells = new List<Card>();
    public List<Card> Uunits = new List<Card>();
    public List<Card> Uchampions = new List<Card>();
    public List<Card> Uspells = new List<Card>();

    public Region(string n, List<Card> cards)
    {
        name = n;
        for(int i = 0; i < cards.Count; ++i)
        {
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
        }
    }

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
    
    public CardCodeAndCount GetRandomCardAndAmount(int champsRemaining, int cardsNeeded, out bool isChampion, out string name)
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
        }
        else if(randNum == 1)
        {
            c = spells[Random.Range(0, spells.Count)];
            name = c.name;
            CCC.CardCode = c.cardCode;
            CCC.Count = randAmount;
        }
        else
        {
            c = champions[Random.Range(0, champions.Count)];
            name = c.name;
            CCC.CardCode = c.cardCode;
            CCC.Count = randAmountChampions;
            isChampion = true;
        }
        return CCC;
    }

    public string GetAllCardsAsString()
    {
        string s = "";
        foreach(Card c in units)
        {
            s += c.name + ",";
        }
        s += "|";
        foreach(Card c in spells)
        {
            s += c.name + ",";
        }
        s += "|";
        foreach(Card c in champions)
        {
            s += c.name + ",";
        }
        s += "&";
        foreach (Card c in Uunits)
        {
            s += c.name + ",";
        }
        s += "|";
        foreach (Card c in Uspells)
        {
            s += c.name + ",";
        }
        s += "|";
        foreach (Card c in Uchampions)
        {
            s += c.name + ",";
        }
        return s;
    }

    public override string ToString()
    {
        return name;
    }

}

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


