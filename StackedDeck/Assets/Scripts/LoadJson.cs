using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class LoadJson
{
    private const string path = "/cardsbyregion.dat";
    private const string jsonName = "set1-en_us.json";
       
    public static Region[] LoadResourceTextFile()
    {
        if (GeneratedCards())
        {
            Debug.Log("Already Loaded.");
            return AllCardsByRegion();
        }
        else
        {
            string filePath = jsonName.Replace(".json", "");
            TextAsset asset = Resources.Load<TextAsset>(filePath);
            Region[] r =  SortCardsByRegion(asset.text);
            SaveCardsOntoPath(r);
            return r;
        }
    }

    static Region[] AllCardsByRegion()
    {
        string p = Application.persistentDataPath + path;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(p, FileMode.Open);
        RegionsJSON r = formatter.Deserialize(stream) as RegionsJSON;
        stream.Close();
        return CreateRegions(r.AllCardsByRegion);
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

    static Region[] SortCardsByRegion(string text)
    {
        char[] charsToTrim = { '[', ']' };
        string[] stringToSplit = { "}," };
        string allCards = text.Trim(charsToTrim).Trim();
        allCards = allCards.Replace("},", "}},");
        string[] cards = allCards.Split(stringToSplit, System.StringSplitOptions.None);
        //Debug.Log(cards.Length);
        return SortCards(cards);
    }

    static Region[] SortCards(string[] cards)
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
        Region dem = new Region(d);
        Region fre = new Region(f);
        Region ion = new Region(i);
        Region nox = new Region(n);
        Region pil = new Region(p);
        Region sha = new Region(s);
        Region[] r = { dem, fre, ion, nox, pil, sha };
        return r;
    }

    static Region[] CreateRegions(string[][] allCards)
    {
        Region dem = new Region(ConvertStringToCard(allCards[0]));
        Region fre = new Region(ConvertStringToCard(allCards[1]));
        Region ion = new Region(ConvertStringToCard(allCards[2]));
        Region nox = new Region(ConvertStringToCard(allCards[3]));
        Region pil = new Region(ConvertStringToCard(allCards[4]));
        Region sha = new Region(ConvertStringToCard(allCards[5]));
        Region[] r = { dem, fre, ion, nox, pil, sha };
        return r;

    }

    static List<Card> ConvertStringToCard(string[] stringCards)
    {
        List<Card> cards = new List<Card>();
        foreach(string c in stringCards)
        {
            cards.Add(JsonUtility.FromJson<Card>(c));
        }
        return cards;
    }

    

    public static void DeleteCardsOnPath()
    {
        string p = Application.persistentDataPath + path;
        if(File.Exists(p))
        {
            File.Delete(p);
        }
    }


}
