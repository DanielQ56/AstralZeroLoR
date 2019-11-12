using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoRDeckCodes;
using System.Linq;

public class CardManager : MonoBehaviour
{
    Region[] allRegions;

    LoRDeckEncoder encoder;


    // Start is called before the first frame update
    void Awake()
    {
        //LoadJson.DeleteCardsOnPath();
        allRegions = LoadJson.LoadResourceTextFile();
        //for(int i = 0; i < allRegions.Length; ++i)
        //{
        //    Debug.Log(Utilities.IndexToRegion[i] + ":\n" + allRegions[i].ToString());
        //}
        encoder = new LoRDeckEncoder();
    }

    private void Start()
    {
        //GenerateRandomDeck();
    }

    public void GenerateRandomDeck()
    {
        Region[] regions = new Region[2];
        regions[0] = allRegions[Random.Range(0, 6)];
        do
        {
            regions[1] = allRegions[Random.Range(0, 6)];
        }
        while (regions[0] == regions[1]);
        int totalCardsFromR1 = Random.Range(0, 41);
        int totalCardsFromR2 = 40 - totalCardsFromR1;
        Debug.Log(totalCardsFromR1 + " " + totalCardsFromR2);
        int numChampions = 0;
        int numR1 = 0;
        int numR2 = 0;
        List<CardCodeAndCount> cards = new List<CardCodeAndCount>();
        Dictionary<string, int> cardCodeAndAmount = new Dictionary<string, int>();
        bool isChampion;
        CardCodeAndCount c;
        bool added = false;
        string name;
        while(numR1 + numR2 < 40)
        {
            added = false;
            int index = Random.Range(0, 2);
            if (index == 0 && numR1 < totalCardsFromR1)
            {
                do
                {
                    c = regions[index].GetRandomCardAndAmount((numChampions >= 6), 6 - numChampions, totalCardsFromR1 - numR1, out isChampion, out name);
                    if (cardCodeAndAmount.ContainsKey(c.CardCode))
                    {
                        if (cardCodeAndAmount[c.CardCode] < 3)
                        {
                            int remainingNum = 3 - cardCodeAndAmount[c.CardCode];
                            c.Count = (c.Count < remainingNum ? c.Count : remainingNum);
                            cardCodeAndAmount[c.CardCode] += c.Count;
                            added = true;
                        }
                    }
                    else
                    {
                        cardCodeAndAmount.Add(c.CardCode, c.Count);
                        added = true;
                    }
                } while (!added);
                //Debug.Log(name + " " + cardCodeAndAmount[c.CardCode]);
                if (isChampion)
                    numChampions += c.Count;
                numR1 += c.Count;
                
            } 
            else if(numR2 < totalCardsFromR2)
            {
                do
                {
                    c = regions[index].GetRandomCardAndAmount((numChampions >= 6), 6 - numChampions, totalCardsFromR2 - numR2, out isChampion, out name);
                    if (cardCodeAndAmount.ContainsKey(c.CardCode))
                    {
                        if (cardCodeAndAmount[c.CardCode] < 3)
                        {
                         
                            int remainingNum = 3 - cardCodeAndAmount[c.CardCode];
                            c.Count = (c.Count < remainingNum ? c.Count : remainingNum);
                            cardCodeAndAmount[c.CardCode] += c.Count;
                            added = true;
                        }
                    }
                    else
                    {
                        cardCodeAndAmount.Add(c.CardCode, c.Count);
                        added = true;
                    }
                } while (!added);
                //Debug.Log(name + " " + cardCodeAndAmount[c.CardCode]);
                if (isChampion)
                    numChampions += c.Count;
                numR2 += c.Count;
            }
        }
        int sum = 0;
        foreach (KeyValuePair<string, int> pair in cardCodeAndAmount)
        {
            CardCodeAndCount card = new CardCodeAndCount();
            card.CardCode = pair.Key;
            card.Count = pair.Value;
            sum += pair.Value;
            cards.Add(card);
        }
        string code = LoRDeckEncoder.GetCodeFromDeck(cards);
        Debug.Log(code);
        if(sum < 40)
        {
            Debug.Log("YIKES");
        }
    }

}
