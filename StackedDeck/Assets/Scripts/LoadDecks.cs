using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoRDeckCodes;
using UnityEngine.UI;
using TMPro;

public class LoadDecks : MonoBehaviour
{
    [SerializeField] Queries query;
    [SerializeField] List<ToggleDescriptions> toggles;
    [SerializeField] GameObject LoadPanel;
    [SerializeField] GameObject SavePanel;
    [SerializeField] TMP_InputField newDeck;

    string newDeckString;

    bool isSaving = false;

    public void LoadSavedDecks()
    {
        LoadPanel.SetActive(true);
        SetupLoadedDecks(UserManager.instance.GetLoadedDecks());
    }

    void SetupLoadedDecks(List<string> decks)
    {
        for(int i = 0; i < 3; ++i)
        {
            if (decks[i].Length > 0)
            {
                string code = decks[i];
                string information = code.Substring(code.Length - 26);
                string deckName = information.Substring(information.Length - 20).Trim(',');
                int region1, region2;
                int.TryParse(information[0].ToString(), out region1);
                int.TryParse(information[1].ToString(), out region2);
                int numR1, numR2;
                int.TryParse(information.Substring(2, 2), out numR1);
                int.TryParse(information.Substring(4, 2), out numR2);
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
                        query.LoadDeck(toggles[i].GetDetails());
                    }
                }
            }
        }
    }

    public void OpenSavePanel(string deck)
    {
        SavePanel.SetActive(true);
        newDeckString = deck;
    }

    public void SaveDeck()
    {
        string name = "";
        if (newDeck.text.Length == 0)
        {
            newDeck.text = "NewDeck";
        }
        name += newDeck.text;
        while (name.Length < 20)
        {
            name += ',';
        }
        isSaving = true;
        newDeckString += name;
        LoadSavedDecks();
        SavePanel.SetActive(false);
    }

    public void ExitedPanel()
    {
        isSaving = false;
        newDeckString = "";
        LoadPanel.SetActive(false);
    }

    public void CloseAllPanels()
    {
        SavePanel.SetActive(false);
        LoadPanel.SetActive(false);
    }


    

    


}
