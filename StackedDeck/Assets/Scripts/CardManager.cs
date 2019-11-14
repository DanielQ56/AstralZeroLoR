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

    LoRDeckEncoder encoder;

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
        allRegions = LoadJson.LoadResourceTextFile();
        encoder = new LoRDeckEncoder();
    }

    public void GenerateInfoForDeck(string region1, string region2, int numReg1, int numReg2)
    {
        GenerateRegions(region1, region2);
        
        GenerateAmountPerRegion(numReg1, numReg2);
        Debug.Log(totalCardsFromR1 + " " + totalCardsFromR2);
        Debug.Log(r1.ToString() + " " + r2.ToString());

        cards.Clear();
        cardCodeAndAmount.Clear();
        names.Clear();
        GenerateDeck();

    }

    void GenerateAmountPerRegion(int numReg1, int numReg2)
    {
        if(numReg1 == -1)
        {
            totalCardsFromR1 = Random.Range(0, 41);
            totalCardsFromR2 = 40 - totalCardsFromR1;
        }
        else
        {
            totalCardsFromR1 = numReg1;
            totalCardsFromR2 = numReg2;
        }
    }

    void GenerateRegions(string region1, string region2)
    {
        if(region1 == "" && region2 == "")
        {
            r1 = allRegions[Random.Range(0, 6)];
            do
            {
                r2 = allRegions[Random.Range(0, 6)];
            } while (r1 == r2);
        }
        else if(region1 == "")
        {
            do
            {
                r1 = allRegions[Random.Range(0, 6)];
            } while (r1 == r2);
        }
        else if(region2 == "" || region1 == region2)
        {
            do
            {
                r2 = allRegions[Random.Range(0, 6)];
            } while (r1 == r2);
        }
        else
        {
            r1 = allRegions[Utilities.RegionToIndex[region1]];
            r2 = allRegions[Utilities.RegionToIndex[region2]];
        }
    }

    public void GenerateDeck()
    {
        int numChampions = 0;
        int numR1 = 0;
        int numR2 = 0;
        bool isChampion;
        CardCodeAndCount c;
        string name;
        while (numR1 + numR2 < 40)
        {
            int index = Random.Range(0, 2);
            if (index == 0 && numR1 < totalCardsFromR1)
            {
                while (true)
                {
                    c = r1.GetRandomCardAndAmount(maxNumChampions - numChampions, totalCardsFromR1 - numR1, out isChampion, out name);
                    if (cardCodeAndAmount.ContainsKey(c.CardCode))
                    {
                        if (cardCodeAndAmount[c.CardCode].Count < 3)
                        {
                            //Debug.Log("BEFORE MOD: " + c.Count);
                            int remainingNum = 3 - cardCodeAndAmount[c.CardCode].Count;
                            c.Count = (c.Count < remainingNum ? c.Count : remainingNum);
                            cardCodeAndAmount[c.CardCode].Count += c.Count;
                            //Debug.Log("AFTER MOD: " + c.Count);
                            break;
                        }

                    }
                    else
                    {
                        cardCodeAndAmount.Add(c.CardCode, c);
                        //try
                        //{
                        //    names.Add(c.CardCode, name);
                        //}
                        //catch(System.ArgumentException)
                        //{
                        //    Debug.Log("BROKEN HERE " + name);
                        //}
                        break;
                    }
                }
                //Debug.Log("AFTER WHILE LOOP: " + c.Count);
                if (isChampion)
                    numChampions += c.Count;
                numR1 += c.Count;

            }
            else if (numR2 < totalCardsFromR2)
            {
                while (true)
                {
                    c = r2.GetRandomCardAndAmount(maxNumChampions - numChampions, totalCardsFromR2 - numR2, out isChampion, out name);
                    if (cardCodeAndAmount.ContainsKey(c.CardCode))
                    {
                        if (cardCodeAndAmount[c.CardCode].Count < 3)
                        {
                            //.Log("BEFORE MOD: " + c.Count);
                            int remainingNum = 3 - cardCodeAndAmount[c.CardCode].Count;
                            c.Count = (c.Count < remainingNum ? c.Count : remainingNum);
                            cardCodeAndAmount[c.CardCode].Count += c.Count;
                            ///Debug.Log("AFTER MOD: " + c.Count);
                            break;
                        }

                    }
                    else
                    {
                        //Debug.Log("ADDING CARD: " + c.Count);
                        cardCodeAndAmount.Add(c.CardCode, c);
                        //try
                        //{
                        //    names.Add(c.CardCode, name);
                        //}
                        //catch (System.ArgumentException)
                        //{
                        //    Debug.Log("BROKEN HERE " + name);
                        //}
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
            //Debug.Log(names[pair.Key] + " " + pair.Value.Count);
            cards.Add(pair.Value);
        }
        string code = LoRDeckEncoder.GetCodeFromDeck(cards);
        Debug.Log(code);
    }

}


