using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoRDeckCodes;
using UnityEngine.UI;
using TMPro;

public class LoadDecks : MonoBehaviour
{
    [SerializeField] List<ToggleDescriptions> toggles;
    [SerializeField] GameObject LoadPanel;
    [SerializeField] GameObject SavePanel;

    string newDeckString;

    bool isSaving = false;

    public void SetupLoadedDecks(List<string> decks)
    {
        for(int i = 0; i < 3; ++i)
        {
            if (decks[i].Length > 0)
            {
                string code = decks[i];
                string information = code.Substring(code.Length - 26);
                Debug.Log("INFORMATION: " + information);
                string deckName = information.Substring(information.Length - 20).Trim(',');
                Debug.Log(deckName);
                int region1, region2;
                int.TryParse(information[0].ToString(), out region1);
                int.TryParse(information[1].ToString(), out region2);
                Debug.Log(region1 + " " + region2);
                int numR1, numR2;
                int.TryParse(information.Substring(2, 2), out numR1);
                int.TryParse(information.Substring(4, 2), out numR2);
                Debug.Log(numR1 + " " + numR2);
                toggles[i].Setup(deckName, region1, region2, numR1, numR2, code.Replace(information, ""));
                
            }
            else
            {
                if (!isSaving)
                    toggles[i].IsInteractable(false);
                else
                    toggles[i].IsInteractable(true);
            }
            
        }
    }


    public void SelectDeck()
    {
        bool selectedADeck = false;
        foreach(ToggleDescriptions t in toggles)
        {
            if (t.Selected())
            {
                selectedADeck = true;
            }
        }
        if (selectedADeck)
        {
            if (isSaving)
            {
                List<string> decks = new List<string>();
                foreach (ToggleDescriptions t in toggles)
                {
                    if (t.Selected())
                    {
                        decks.Add(newDeckString);
                    }
                    else
                    {
                        decks.Add(t.GetDeckAsString());
                    }
                }
                UserManager.instance.UpdateSavedDecks(decks);
                ExitedPanel();

            }
            else
            {
                for (int i = 0; i < toggles.Count; ++i)
                {
                    if (toggles[i].Selected())
                    {
                        InputManager.instance.LoadDeck(toggles[i].GetDetails());
                    }
                }
            }
        }
    }

    public void SaveDeck(string deckstring)
    {
        isSaving = true;
        newDeckString = deckstring;
        InputManager.instance.LoadSavedDecks();
    }

    public void ExitedPanel()
    {
        isSaving = false;
        newDeckString = "";
    }

    public void CloseAllPanels()
    {
        SavePanel.SetActive(false);
        LoadPanel.SetActive(false);
    }


    

    


}
