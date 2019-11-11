using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public List<Card> units = new List<Card>();
    public List<Card> champions = new List<Card>();
    public List<Card> spells = new List<Card>();

    public Region(List<Card> cards)
    {
        for(int i = 0; i < cards.Count; ++i)
        {
            if(cards[i].type == "Spell")
            {
                spells.Add(cards[i]);
            }
            else
            {
                if(cards[i].supertype == "Champion")
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

    public override string ToString()
    {
        return string.Format("Units (Not including champions: {0}, Spells: {1}, Champions: {2}", units.Count, spells.Count, champions.Count);
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

    public override string ToString()
    {
        return name + " " + cardCode + " " + type;
    }
}


