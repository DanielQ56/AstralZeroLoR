using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class LoadJson
{
    private const string path = "/cardsbyregion.dat"; //path the file we store the cards in for easy retrieval ish
    private const string jsonName = "set1-en_us.json"; //file with all the cards

    #region Loading the cards
    //Parent function that handles parsing the JSON or retrieving the data from the datapath
    public static Region[] LoadResourceTextFile(PlayerData player)
    {
        if (GeneratedCards())
        {
            Debug.Log("Already Loaded.");
            return AllCardsByRegion(player);
        }
        else
        {
            string filePath = jsonName.Replace(".json", "");
            TextAsset asset = Resources.Load<TextAsset>(filePath);
            Region[] r =  SortCardsByRegion(asset.text, player);
            SaveCardsOntoPath(r);
            return r;
        }
    }

    //Called if we have already read the JSON file
    static Region[] AllCardsByRegion(PlayerData player)
    {
        string p = Application.persistentDataPath + path;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(p, FileMode.Open);
        RegionsJSON r = formatter.Deserialize(stream) as RegionsJSON;
        stream.Close();
        return CreateRegions(r.AllCardsByRegion, player);
    }

    static bool GeneratedCards()
    {
        string p = Application.persistentDataPath + path;
        if(File.Exists(p))
        {
            return true;
        }
        return false;
    }

    //Method that calls the SortCard functions
    static Region[] SortCardsByRegion(string text, PlayerData player)
    {
        char[] charsToTrim = { '[', ']' };
        string[] stringToSplit = { "}," };
        string allCards = text.Trim(charsToTrim).Trim();
        allCards = allCards.Replace("},", "}},");
        string[] cards = allCards.Split(stringToSplit, System.StringSplitOptions.None);
        //Debug.Log(cards.Length);
        return SortCards(cards, player);
    }

    //Method used to generate Region objects if we have NOT read the JSON file yet (ONE TIME USE)
    static Region[] SortCards(string[] cards, PlayerData player)
    {
        List<Card> d = new List<Card>();
        List<Card> f = new List<Card>();
        List<Card> i = new List<Card>();
        List<Card> n = new List<Card>();
        List<Card> p = new List<Card>();
        List<Card> s = new List<Card>();
        for (int a = 0; a < cards.Length; ++a)
        {
            Card c = JsonUtility.FromJson<Card>(cards[a]);
            switch(c.regionRef)
            {
                case "Demacia":
                    d.Add(c);
                    break;
                case "Freljord":
                    f.Add(c);
                    break;
                case "Ionia":
                    i.Add(c);
                    break;
                case "Noxus":
                    n.Add(c);
                    break;
                case "PiltoverZaun":
                    p.Add(c);
                    break;
                case "ShadowIsles":
                    s.Add(c);
                    break;
            }
        }
        Region dem = new Region("Demacia",  d, (player == null ? "" : player.r1));
        Region fre = new Region("Freljord", f, (player == null ? "" : player.r2));
        Region ion = new Region("Ionia", i, (player == null ? "" : player.r3));
        Region nox = new Region("Noxus", n, (player == null ? "" : player.r4));
        Region pil = new Region("Piltover", p, (player == null ? "" : player.r5));
        Region sha = new Region("ShadowIsles", s, (player == null ? "" : player.r6));
        Region[] r = { dem, fre, ion, nox, pil, sha };
        return r;
    }

    //Method used to generate Region objects if we have already read the JSON file and stored it onto a different data file
    static Region[] CreateRegions(string[][] allCards, PlayerData player)
    {
        Region dem = new Region("Demacia", ConvertStringToCard(allCards[0]), (player == null ? "" : player.r1));
        Region fre = new Region("Freljord", ConvertStringToCard(allCards[1]), (player == null ? "" : player.r2));
        Region ion = new Region("Ionia", ConvertStringToCard(allCards[2]), (player == null ? "" : player.r3));
        Region nox = new Region("Noxus", ConvertStringToCard(allCards[3]), (player == null ? "" : player.r4));
        Region pil = new Region("Piltover", ConvertStringToCard(allCards[4]), (player == null ? "" : player.r5));
        Region sha = new Region("ShadowIsles", ConvertStringToCard(allCards[5]), (player == null ? "" : player.r6));
        Region[] r = { dem, fre, ion, nox, pil, sha };
        return r;

    }

    //Converts the a given string into a list of Card Objects
    static List<Card> ConvertStringToCard(string[] stringCards)
    {
        List<Card> cards = new List<Card>();
        foreach(string c in stringCards)
        {
            cards.Add(JsonUtility.FromJson<Card>(c));
        }
        return cards;
    }

    #endregion

    //Used for getting all the cards per region as strings for saving onto database
    public static string[] ConvertCardsToString(Region[] regions)
    {
        string[] s = new string[6];
        for (int i = 0; i < 6; ++i)
        {
            s[i] = regions[i].GetAllCardsAsString();
        }
        return s;
    }

    #region Storing Cards onto .dat file
    //Deletes the data file we stored cards on
    public static void DeleteCardsOnPath()
    {
        Debug.Log("testing for deleting");
        string p = Application.persistentDataPath + path;
        if(File.Exists(p))
        {
            Debug.Log("deleting");
            File.Delete(p);
        }
    }


    //Saves the cards onto the data path
    static void SaveCardsOntoPath(Region[] regions)
    {
        List<string[]> allCards = new List<string[]>();
        foreach (Region r in regions)
        {
            allCards.Add(r.AllCardsInRegion());
        }
        BinaryFormatter formatter = new BinaryFormatter();
        string p = Application.persistentDataPath + path;
        FileStream stream = new FileStream(p, FileMode.Create);
        RegionsJSON rJson = new RegionsJSON(allCards.ToArray());
        formatter.Serialize(stream, rJson);
        stream.Close();
    }
    #endregion



}
